using AppDTOs.Enumerations;
using ITI.Baseline.Util;
using ValueObjects;

namespace AppServices;

public class PageAppService : ApplicationService, IPageAppService
{
    private readonly IAppPermissions _perms;
    private readonly IPageRepository _repo;
    private readonly IPageQueries _queries;
    private readonly ISheetTypeRepository _sheetTypeRepo;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;

    public PageAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IPageRepository repo,
        IPageQueries queries,
        ISheetTypeRepository sheetTypeRepo,
        IFileStore fileStore,
        IFilePathBuilder path
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _repo = repo;
        _queries = queries;
        _sheetTypeRepo = sheetTypeRepo;
        _fileStore = fileStore;
        _path = path;
    }

    public Task<string?> GetPdfAsync(PageId id, Stream outputStream)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var pageDto = await _queries.GetAsync(id);
                if (pageDto == null)
                    return null;

                Authorize.Require(await _perms.CanViewPagesAsync(pageDto.OrganizationId));

                await _fileStore.GetAsync(_path.ForPage(pageDto.Pdf.FileId), outputStream);

                return "application/pdf";
            }
        );
    }

    public Task<string?> GetThumbnailAsync(PageId id, Stream outputStream)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var pageDto = await _queries.GetAsync(id);
                if (pageDto == null)
                    return null;

                Authorize.Require(await _perms.CanViewPagesAsync(pageDto.OrganizationId));

                await _fileStore.GetAsync(_path.ForThumbnail(pageDto.Thumbnail.FileId), outputStream);

                return pageDto.Thumbnail.FileType;
            }
        );
    }

    public Task<List<PageSummaryDto>> ListAsync(ProjectId projectId, ActiveFilter activeFilter)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(projectId)),
            () => _queries.ListAsync(projectId, activeFilter)
        );
    }

    private async Task<Page> GetDomainEntityAsync(PageId id)
    {
        var page = await _repo.GetAsync(id);
        Require.NotNull(page, "Page not found.");
        Authorize.Require(await _perms.CanManagePagesAsync(page.OrganizationId));

        return page;
    }

    public Task DuplicateAsync(PageId id)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var page = await GetDomainEntityAsync(id);

                var newPdfId = new FileId();
                var newThumbId = new FileId();

                await _fileStore.CopyAsync(_path.ForPage(page.Pdf.FileId), _path.ForPage(newPdfId));
                await _fileStore.CopyAsync(_path.ForThumbnail(page.Thumbnail.FileId), _path.ForThumbnail(newThumbId));

                var pdfRef = new FileRef(newPdfId, page.Pdf.FileType);
                var thumbRef = new FileRef(newThumbId, page.Thumbnail.FileType);
                var index = page.IsActive ? page.Index + 1 : 0;

                var newPage = new Page(page.OrganizationId, page.ProjectId, pdfRef, thumbRef, index);
                newPage.SetActive(page.IsActive);

                if (page.IsActive)
                    await _repo.IncrementIndicesAsync(page.ProjectId, index);

                _repo.Add(newPage);
            }
        );
    }

    public Task SetIndexAsync(PageId id, bool decreaseIndex)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageAsync(id)),
            () => _repo.SetIndexAsync(id, decreaseIndex)
        );
    }

    public Task SetActiveAsync(PageId id, bool isActive)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageAsync(id)),
            async () =>
            {
                await _repo.SetIndexBeforeSetActiveAsync(id, isActive);

                // Must be the first line since it does authorization
                var page = await GetDomainEntityAsync(id);
                page.SetActive(isActive);
            }
        );
    }

    public Task SetSheetNumberAndNameAsync(
        PageId id,
        SheetTypeId sheetTypeId,
        string sheetNumberSuffix,
        string? sheetNameSuffix
    )
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var page = await GetDomainEntityAsync(id);
                var sheetType = await _sheetTypeRepo.GetAsync(sheetTypeId);
                Require.NotNull(sheetType, "Sheet type not found.");

                page.SetSheetNumberAndName(sheetType, sheetNumberSuffix, sheetNameSuffix);
            }
        );
    }
}
