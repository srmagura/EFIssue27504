namespace TestDataLoader.Loaders;

internal class OrganizationOptionsLoader
{
    private readonly IOrganizationOptionsAppService _organizationOptionsAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    private readonly Bogus.DataSets.Lorem _lorem = new();
    private readonly Randomizer _randomizer = new();

    public OrganizationOptionsLoader(
        IOrganizationOptionsAppService organizationOptionsAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _organizationOptionsAppService = organizationOptionsAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddOrganizationOptionsAsync()
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
            if (_randomizer.Double() < 0.5)
                await _organizationOptionsAppService.SetDefaultProjectDescriptionAsync(organizationId, GetText());

            if (_randomizer.Double() < 0.5)
                await _organizationOptionsAppService.SetNotForConstructionDisclaimerTextAsync(organizationId, GetText());
        }

        _monitor.WriteCompletedMessage("Added organization options.");
    }

    private string GetText()
    {
        return _lorem.Sentences(sentenceCount: _randomizer.Number(1, 4), separator: " ");
    }
}
