using TestDataLoader.Helpers;
using TestUtilities;

namespace TestDataLoader.Loaders;

internal class PageLoader
{
    private readonly IImportAppService _importAppService;
    private readonly IPageAppService _pageAppService;
    private readonly ISheetTypeAppService _sheetTypeAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    public PageLoader(
        IImportAppService importAppService,
        ISheetTypeAppService sheetTypeAppService,
        IPageAppService pageAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _importAppService = importAppService;
        _pageAppService = pageAppService;
        _sheetTypeAppService = sheetTypeAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddPagesAsync()
    {
        OrganizationId organizationId;
        ProjectId projectId;

        using (var db = _dbFactory())
        {
            organizationId = await LoaderQueries.GetHostOrganizationAsync(db);
            projectId = await LoaderQueries.GetFirstHostOrganizationProjectAsync(db);
        }

        var filename = "floorplan.pdf";
        using var stream = IntegrationTest.GetResourceStream(filename);

        await _importAppService.ImportAsync(projectId, filename, stream);

        var pages = await _pageAppService.ListAsync(projectId, ActiveFilter.All);
        var sheetTypes = await _sheetTypeAppService.ListAsync(organizationId);

        var firstPageId = pages.First(p => p.Index == 0).Id;
        var secondPageId = pages.First(p => p.Index == 1).Id;

        var avSheetTypeId = sheetTypes.First(p => p.SheetNumberPrefix == "T1.2").Id;
        var lcAndShSheetTypeId = sheetTypes.First(p => p.SheetNumberPrefix == "T2.2").Id;

        await _pageAppService.SetSheetNumberAndNameAsync(firstPageId, avSheetTypeId, "00", "Basement Plan");
        await _pageAppService.SetSheetNumberAndNameAsync(secondPageId, lcAndShSheetTypeId, "01", sheetNameSuffix: null);

        _monitor.WriteCompletedMessage("Added pages.");
    }
}
