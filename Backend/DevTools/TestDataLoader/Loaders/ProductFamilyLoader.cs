namespace TestDataLoader.Loaders;

internal class ProductFamilyLoader
{
    private readonly IProductFamilyAppService _productFamilyAppService;
    private readonly TestDataMonitor _testDataMonitor;
    private readonly Func<AppDataContext> _dbFactory;

    public ProductFamilyLoader(
        IProductFamilyAppService productFamilyAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _productFamilyAppService = productFamilyAppService;
        _testDataMonitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddProductFamiliesAsync()
    {
        List<OrganizationId> organizationIds;
        using (var db = _dbFactory())
        {
            organizationIds = db.Organizations
                .Select(organization => new OrganizationId(organization.Id))
                .ToList();
        }

        foreach (var organizationId in organizationIds)
        {
            await _productFamilyAppService.AddAsync(organizationId, "HD TVs");
            await _productFamilyAppService.AddAsync(organizationId, "Linear Light Fixtures");
            await _productFamilyAppService.AddAsync(organizationId, "Product Family 1");

            var id = await _productFamilyAppService.AddAsync(organizationId, "Product Family 2");
            await _productFamilyAppService.SetActiveAsync(id, false);
        }

        _testDataMonitor.WriteCompletedMessage("Added product families.");
    }
}
