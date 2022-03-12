using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders;

internal class SymbolLoader
{
    private readonly ISymbolAppService _symbolAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;
    private Randomizer _randomizer = new();

    public SymbolLoader(
        ISymbolAppService symbolAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _symbolAppService = symbolAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddSymbolsAsync()
    {
        _randomizer = new Randomizer(2143);
        List<OrganizationId> organizationIds;

        using (var db = _dbFactory())
        {
            organizationIds = db.Organizations
                .Where(o => o.IsActive)
                .Select(o => new OrganizationId(o.Id))
                .ToList();
        }

        var svgs = GetSvgs();

        foreach (var organizationId in organizationIds)
        {
            foreach (var (Name, SvgText) in svgs)
            {
                var id = await _symbolAppService.AddAsync(organizationId, Name, SvgText);

                if (_randomizer.Double() < 0.1)
                {
                    await _symbolAppService.SetActiveAsync(id, false);
                }
            }
        }

        _monitor.WriteCompletedMessage("Added symbols.");
    }

    private static List<(string Name, string SvgText)> GetSvgs()
    {
        return new List<(string Name, string Filename)>()
        {
            ("65 Inch Television", "65-inch-television.svg"),
            ("Indoor Wireless Access Point", "indoor-wireless-access-point.svg"),
            ("Lighting Control Headend Equipment", "lighting-control-headend-equipment.svg"),
            ("Stewart Motorized Film Screen", "stewart-motorized-film-screen.svg"),
            ("Access Control Door Station", "access-control-door-station.svg"),
            ("2 Port Data Outlet", "2-port-data-outlet.svg"),
            ("8 Inch In-ceiling Speakers", "8-inch-in-ceiling-speakers.svg"),
            ("AV Receiver", "av-receiver.svg"),
            ("In-ground Subwoofer", "in-ground-subwoofer.svg"),
            ("In-wall LCR Speakers", "in-wall-lcr-speakers.svg"),
            ("In-wall Subwoofer", "in-wall-subwoofer.svg"),
            ("Large Motorized Bug Screen", "large-motorized-bug-screen.svg"),
            ("Low Voltage Equipment Rack", "low-voltage-equipment-rack.svg"),
            ("Lutron Drape Track", "lutron-drape-track.svg"),
            ("Lutron Homeworks QS Dimmer", "lutron-homeworks-qs-dimmer.svg"),
            ("Lutron Homeworks QS Swtich", "lutron-homeworks-qs-switch.svg"),
            ("Lutron SeeTouch Keypad", "lutron-seeTouch-keypad.svg"),
            ("Lutron Small Single Roller Shade", "lutron-small-single-roller-shade.svg"),
            ("Medium Motorized Bug Screen", "medium-motorized-bug-screen.svg"),
            ("Motorized Shade 10 Output Power Supply", "motorized-shade-10-output-power-supply.svg"),
            ("Outdoor Satellite Speakers", "outdoor-satellite-speakers.svg"),
            ("Outdoor Wireless Access Point", "outdoor-wireless-access-point.svg"),
            ("Small Aperture In-ceiling Speakers", "small-aperture-in-ceiling-speakers.svg"),
            ("Sonos Arc Soundbar", "sonos-arc-soundbar.svg"),
            ("Sonos Sub", "sonos-sub.svg"),
            ("Surface Mounted Outdoor Speakers", "surface-mounted-outdoor-speakers.svg"),
            ("Video Outlet", "video-outlet.svg"),
            ("Video Projection", "video-projection.svg")
        }
            .Select(s => (s.Name, ResourceUtil.GetResourceText($"Symbols.{s.Filename}")))
            .ToList();
    }
}
