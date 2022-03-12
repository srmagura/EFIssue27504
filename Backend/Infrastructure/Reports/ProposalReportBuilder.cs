using AppDTOs;
using AppDTOs.Enumerations;
using AppDTOs.Report;
using AppInterfaces;
using Aspose.Pdf;
using DataInterfaces.Queries;
using Identities;
using InfraInterfaces;
using ITI.Baseline.Util;
using ITI.DDD.Core;
using Reports.Shared;
using ValueObjects;

namespace Reports;

public class ProposalReportBuilder : IReportBuilder
{
    private readonly IProjectAppService _projectAppService;
    private readonly IProductKitQueries _productKitQueries;
    private readonly IPageAppService _pageAppService;
    private readonly IDesignerQueries _designerQueries;
    private readonly ISheetTypeAppService _sheetTypeAppService;
    private readonly ITermsDocumentQueries _termsDocumentQueries;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;
    private readonly IUnitOfWorkProvider _uowp;

    private readonly string _tempDirectory;

    public ProposalReportBuilder(
        IProjectAppService projectAppService,
        IProductKitQueries productKitQueries,
        IPageAppService pageAppService,
        IDesignerQueries designerQueries,
        ISheetTypeAppService sheetTypeAppService,
        ITermsDocumentQueries termsDocumentQueries,
        IFileStore fileStore,
        IFilePathBuilder path,
        IUnitOfWorkProvider uowp,
        ITempFileService tempFileService
    )
    {
        _projectAppService = projectAppService;
        _productKitQueries = productKitQueries;
        _pageAppService = pageAppService;
        _designerQueries = designerQueries;
        _sheetTypeAppService = sheetTypeAppService;
        _termsDocumentQueries = termsDocumentQueries;
        _fileStore = fileStore;
        _path = path;
        _uowp = uowp;

        _tempDirectory = tempFileService.GetTempDirectory(nameof(DrawingSetReportBuilder));
    }

    public async Task BuildAsync(
        ProjectId projectId,
        string filePath,
        Func<Percentage, Task> onProgressAsync,
        bool isDraft,
        bool devGenerateHtml,
        CancellationToken cancellationToken
    )
    {
        var project = await _projectAppService.GetAsync(projectId);
        Require.NotNull(project, "Project could not be found.");

        ProductKitReportDto[] productKits;
        DesignerDataReportDto[] designerData;
        TermsDocumentDto? termsDocument;
        using (_uowp.Begin())
        {
            productKits = await _productKitQueries.ListForReportAsync(projectId);
            designerData = await _designerQueries.ListForReportAsync(projectId);

            termsDocument = await _termsDocumentQueries.GetAsync(project.ReportOptions.TermsDocumentId);
            Require.NotNull(termsDocument, "Terms Document could not be found.");
        }

        var pageEntities = await _pageAppService.ListAsync(projectId, ActiveFilter.ActiveOnly);
        Require.NotNull(pageEntities, "Pages could not be found.");

        var sheetTypes = await _sheetTypeAppService.ListAsync(project.OrganizationId);
        Require.NotNull(sheetTypes, "Sheet Types could not be found.");

        var directoryPath = Path.Combine(_tempDirectory, Guid.NewGuid().ToString());
        var disposables = new List<IDisposable>();
        try
        {
            Directory.CreateDirectory(directoryPath);
            await PageBuilders.SetupTempFilesAsync(_fileStore, _path, directoryPath, project, productKits, designerData, termsDocument);

            List<Document> productLegendDocs = PageBuilders.GetProductLegendPages(
                productKits,
                directoryPath,
                disposables,
                isDraft,
                devGenerateHtml
            );

            // +4 for title page, terms docs, signature page, and merging everything together
            double totalSteps = productLegendDocs.Count + pageEntities.Count + 3;
            double step = productLegendDocs.Count;
            await onProgressAsync(new Percentage(step / totalSteps));

            var sheetIndex = PageBuilders.BuildSheetIndex(productLegendDocs.Count, pageEntities, sheetTypes);

            Document titlePageDoc = PageBuilders.GetTitlePage(
                project,
                sheetIndex,
                directoryPath,
                disposables,
                isDraft,
                devGenerateHtml
            );

            step++;
            await onProgressAsync(new Percentage(step / totalSteps));

            List<Document> floorplanDocs = PageBuilders.GetFloorplanPages(
                designerData,
                productKits,
                directoryPath,
                disposables,
                isDraft,
                devGenerateHtml
            );

            step += floorplanDocs.Count;
            await onProgressAsync(new Percentage(step / totalSteps));

            List<Document> termsDocs = PageBuilders.GetTermsDocuments(
                termsDocument.Id,
                directoryPath,
                disposables,
                isDraft,
                devGenerateHtml
            );

            step++;
            await onProgressAsync(new Percentage(step / totalSteps));

            Document signatureDoc = PageBuilders.GetSignaturePage(
                project,
                50000, // TODO:AQ Get actual budget
                disposables,
                isDraft,
                devGenerateHtml
            );

            step++;
            await onProgressAsync(new Percentage(step / totalSteps));

            using Document doc = new Document();
            doc.Pages.Add(titlePageDoc.Pages);
            productLegendDocs.ForEach(d => doc.Pages.Add(d.Pages));
            floorplanDocs.ForEach(d => doc.Pages.Add(d.Pages));
            termsDocs.ForEach(d => doc.Pages.Add(d.Pages));
            doc.Pages.Add(signatureDoc.Pages);

            using var memoryStream = new MemoryStream();
            doc.Save(memoryStream);

            await _fileStore.PutAsync(filePath, memoryStream);

            await onProgressAsync(new Percentage(1d));
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, recursive: true);

            disposables.ForEach(d => d.Dispose());
        }
    }
}
