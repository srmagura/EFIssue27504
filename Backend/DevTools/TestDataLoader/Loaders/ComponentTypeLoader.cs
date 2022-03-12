namespace TestDataLoader.Loaders;

internal class ComponentTypeLoader
{
    private readonly IComponentTypeAppService _componentTypeAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;
    private readonly Randomizer _randomizer = new();

    public ComponentTypeLoader(
        IComponentTypeAppService componentTypeAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _componentTypeAppService = componentTypeAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddComponentTypesAsync()
    {
        List<OrganizationId> organizationIds;
        using (var db = _dbFactory())
        {
            organizationIds = db.Organizations
                .Select(p => new OrganizationId(p.Id))
                .ToList();
        }

        foreach (var organizationId in organizationIds)
        {
            for (var i = 1; i <= 10; i++)
            {
                var id = await _componentTypeAppService.AddAsync(organizationId, $"Component type {i}");

                if (_randomizer.Double() < 0.1)
                    await _componentTypeAppService.SetActiveAsync(id, false);
            }
        }

        _monitor.WriteCompletedMessage("Added component types.");
    }
}
