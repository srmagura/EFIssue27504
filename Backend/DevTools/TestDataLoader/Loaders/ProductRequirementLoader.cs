namespace TestDataLoader.Loaders;

internal class ProductRequirementLoader
{
    private readonly IProductRequirementAppService _productRequirmentAppService;
    private readonly ISymbolAppService _symbolAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    public ProductRequirementLoader(
        IProductRequirementAppService productRequirementAppService,
        ISymbolAppService symbolAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _productRequirmentAppService = productRequirementAppService;
        _symbolAppService = symbolAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddProductRequirementsAsync()
    {
        List<OrganizationId> organizationIds;
        using (var db = _dbFactory())
        {
            organizationIds = db.Organizations
                .Where(o => o.IsActive)
                .Select(p => new OrganizationId(p.Id))
                .ToList();
        }

        foreach (var organizationId in organizationIds)
        {
            var symbols = await _symbolAppService.ListAsync(organizationId, 0, 10000, ActiveFilter.All, search: null);
            var powerOutlet = symbols.Items.First(p => p.Name.Contains("Data Outlet")).SvgText;
            var circuit = symbols.Items.First(p => p.Name.Contains("8 Inch In-ceiling Speakers")).SvgText;
            var hardWiredData = symbols.Items.First(p => p.Name.Contains("QS Dimmer")).SvgText;
            var multipleHardWiredData = symbols.Items.First(p => p.Name.Contains("Low Voltage Equipment Rack")).SvgText;
            var wirlessData = symbols.Items.First(p => p.Name.Contains("Wireless Access Point")).SvgText;
            var conduit = symbols.Items.First(p => p.Name.Contains("Sonos Sub")).SvgText;

            await _productRequirmentAppService.AddAsync(organizationId, "Requires 120V technical power outlet", powerOutlet);
            await _productRequirmentAppService.AddAsync(organizationId, "Requires 20A/120V decicated circuit (per rack or panel)", circuit);
            await _productRequirmentAppService.AddAsync(organizationId, "Requires hard-wired data network connection", hardWiredData);
            await _productRequirmentAppService.AddAsync(organizationId, "Requires multiple hard-wired data network connections", multipleHardWiredData);
            await _productRequirmentAppService.AddAsync(organizationId, "Requires wireless data network connection", wirlessData);
            await _productRequirmentAppService.AddAsync(organizationId, "Requires conduit originating from system headend location", conduit);
        }

        _monitor.WriteCompletedMessage("Added product requirements.");
    }
}
