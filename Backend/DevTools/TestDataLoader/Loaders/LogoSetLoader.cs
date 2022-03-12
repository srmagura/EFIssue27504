using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders;

internal class LogoSetLoader
{
    private readonly ILogoSetAppService _logoSetAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    public LogoSetLoader(
        ILogoSetAppService logoSetAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _logoSetAppService = logoSetAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddLogoSetsAsync()
    {
        OrganizationId[] organizationIds;

        using (var db = _dbFactory())
        {
            organizationIds = await db.Organizations
                .Select(o => new OrganizationId(o.Id))
                .ToArrayAsync();
        }

        foreach (var organizationId in organizationIds)
        {
            using var system7DarkStream = ResourceUtil.GetResourceStream("LogoSets.system7-dark.svg");
            using var system7LightStream = ResourceUtil.GetResourceStream("LogoSets.system7-light.svg");

            await _logoSetAppService.AddAsync(
                organizationId,
                "System 7",
                system7DarkStream,
                "image/svg+xml",
                system7LightStream,
                "image/svg+xml"
            );

            using var mwaDarkStream = ResourceUtil.GetResourceStream("LogoSets.mwa-dark.png");
            using var mwaLightStream = ResourceUtil.GetResourceStream("LogoSets.mwa-light.png");

            var id2 = await _logoSetAppService.AddAsync(
                organizationId,
                "Marchand Wright",
                mwaDarkStream,
                "image/png",
                mwaLightStream,
                "image/png"
            );

            if (organizationId != HostOrganizationLoader.HostOrganizationId)
                await _logoSetAppService.SetActiveAsync(id2, false);
        }

        _monitor.WriteCompletedMessage("Added logo sets.");
    }
}
