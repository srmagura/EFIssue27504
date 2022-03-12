using AppDTOs.Enumerations;
using AppInterfaces;
using AppInterfaces.System;
using Aspose.Pdf;
using Aspose.Pdf.Devices;
using DataInterfaces.Repositories;
using Enumerations;
using Identities;
using InfraInterfaces;
using ITI.DDD.Core;
using Settings;
using ValueObjects;

namespace FileImport;

public class ImportProcessor : IImportProcessor
{
    private readonly IImportSystemAppService _importSystemAppService;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;
    private readonly IPageAppService _pageAppService;
    private readonly IPageRepository _pageRepo;
    private readonly IUnitOfWorkProvider _uowp;
    private readonly PageSettings _pageSettings;

    public ImportProcessor(
        IImportSystemAppService importSystemAppService,
        IFileStore fileStore,
        IFilePathBuilder path,
        IPageAppService pageAppService,
        IPageRepository pageRepo,
        IUnitOfWorkProvider uowp,
        PageSettings pageSettings
    )
    {
        _importSystemAppService = importSystemAppService;
        _fileStore = fileStore;
        _path = path;
        _pageAppService = pageAppService;
        _pageRepo = pageRepo;
        _uowp = uowp;
        _pageSettings = pageSettings;
    }

    public async Task ProcessAsync(ImportId importId, CancellationToken cancellationToken)
    {
        var import = await _importSystemAppService.GetAsync(importId);
        if (import == null) throw new Exception("Import not found.");

        // Event handlers must be idempotent
        if (import.Status != ImportStatus.Pending) return;

        var createdFilePaths = new List<string>();
        var success = false;

        try
        {
            await _importSystemAppService.BeginProcessingAsync(importId);

            var pageDtos = await _pageAppService.ListAsync(import.ProjectId, ActiveFilter.ActiveOnly);
            var initialIndex = pageDtos.Count == 0 ? 0 : pageDtos[^1].Index;

            var pages = await SplitAsync(
                import.File.FileId,
                import.OrganizationId,
                import.ProjectId,
                initialIndex,
                createdFilePaths,
                percentComplete => _importSystemAppService.SetPercentCompleteAsync(importId, percentComplete),
                cancellationToken
            );

            using (var uow = _uowp.Begin())
            {
                foreach (var page in pages)
                    _pageRepo.Add(page);

                await uow.CommitAsync();
            }

            await _importSystemAppService.SetCompletedAsync(importId);
            success = true;
        }
        catch (TaskCanceledException)
        {
            await _importSystemAppService.SetCanceledAsync(importId);
        }
        catch (DomainException e)
        {
            await _importSystemAppService.SetErrorAsync(importId, e.Message);
            throw;
        }
        catch (Exception e)
        {
            await _importSystemAppService.SetErrorAsync(importId, e.ToString());
            throw;
        }
        finally
        {
            var pathsToDelete = new List<string> { _path.ForImport(import.File.FileId) };

            if (!success)
                pathsToDelete.AddRange(createdFilePaths);

            foreach (var path in pathsToDelete)
            {
                if (await _fileStore.ExistsAsync(path))
                    await _fileStore.RemoveAsync(path);
            }
        }
    }

    public async Task<List<Entities.Page>> SplitAsync(
       FileId inputFileId,
       OrganizationId organizationId,
       ProjectId projectId,
       int initialIndex,
       List<string> createdFilePaths,
       Func<Percentage, Task> onProgressAsync,
       CancellationToken cancellationToken
    )
    {
        var resultPages = new List<Entities.Page>();

        using var inputFileStream = new MemoryStream();
        await _fileStore.GetAsync(_path.ForImport(inputFileId), inputFileStream);

        using var doc = new Document(inputFileStream);

        foreach (var page in doc.Pages)
        {
            var heightInches = page.Rect.Height / 72.0;
            var widthInches = page.Rect.Width / 72.0;

            if (Math.Abs(heightInches - _pageSettings.HeightInches) > _pageSettings.HeightMarginOfError ||
                Math.Abs(widthInches - _pageSettings.WidthInches) > _pageSettings.WidthMarginOfError
            )
            {
                throw new DomainException(
                    $"The PDF contains a page that is not {_pageSettings.HeightInches}x{_pageSettings.WidthInches}.",
                    DomainException.AppServiceLogAs.None
                );
            }
        }

        for (var i = 1; i <= doc.Pages.Count; i++)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            using var newDoc = new Document();

            var newPage = newDoc.Pages.Add(doc.Pages[i]);

            var thumbFileId = await GenerateThumbnailAsync(newPage);
            createdFilePaths.Add(_path.ForThumbnail(thumbFileId));
            var thumbRef = new FileRef(thumbFileId, "image/jpeg");

            var pageFileId = await StorePagePdfAsync(newDoc);
            createdFilePaths.Add(_path.ForPage(pageFileId));
            var pageFileRef = new FileRef(pageFileId, "application/pdf");

            resultPages.Add(new Entities.Page(
                organizationId,
                projectId,
                pageFileRef,
                thumbRef,
                index: initialIndex + i - 1
            ));

            await onProgressAsync(new Percentage((double)i / doc.Pages.Count));
        }

        return resultPages;
    }

    private async Task<FileId> StorePagePdfAsync(Document newDoc)
    {
        var fileId = new FileId();

        using var memoryStream = new MemoryStream();

        newDoc.Save(memoryStream);
        memoryStream.Position = 0;

        await _fileStore.PutAsync(_path.ForPage(fileId), memoryStream);

        return fileId;
    }

    private async Task<FileId> GenerateThumbnailAsync(Page page)
    {
        var fileId = new FileId();

        using var memoryStream = new MemoryStream();

        GenerateThumbnail(page, memoryStream);
        memoryStream.Position = 0;

        await _fileStore.PutAsync(_path.ForThumbnail(fileId), memoryStream);

        return fileId;
    }

    private static void GenerateThumbnail(Page page, Stream outputStream)
    {
        var resolution = new Resolution(300);
        var jpegDevice = new JpegDevice(442, 286, resolution);
        jpegDevice.Process(page, outputStream);
    }
}
