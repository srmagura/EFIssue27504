using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders;

internal class ProductPhotoLoader
{
    private readonly IProductPhotoAppService _productPhotoAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;
    private Randomizer _randomizer = new();

    public ProductPhotoLoader(
        IProductPhotoAppService productPhotoAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _productPhotoAppService = productPhotoAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddProductPhotosAsync()
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

        var productPhotos = GetPhotos();

        foreach (var organizationId in organizationIds)
        {
            foreach (var (Name, Filename) in productPhotos)
            {
                using var stream = ResourceUtil.GetResourceStream($"ProductPhotos.{Filename}");

                var fileType = Filename.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                    ? "image/png"
                    : "image/jpeg";

                var id = await _productPhotoAppService.AddAsync(organizationId, Name, stream, fileType);

                if (_randomizer.Double() < 0.1)
                {
                    await _productPhotoAppService.SetActiveAsync(id, false);
                }
            }
        }

        _monitor.WriteCompletedMessage("Added product photos.");
    }

    private static List<(string Name, string Filename)> GetPhotos()
    {
        return new()
        {
            ("65 Inch TV", "65-inch-tv.jpeg"),
            ("Access Control Keypad", "access-control-keypad.png"),
            ("AV Receiver", "av-receiver.jpeg"),
            ("Data Outlet", "data-outlet.jpeg"),
            ("Equipment Rack", "equipment-rack.jpeg"),
            ("Free Standing Subwoofer", "free-standing-subwoofer.png"),
            ("In-ceiling Speaker", "in-ceiling-speaker.jpeg"),
            ("In-ceiling Speaker Small", "in-ceiling-speaker-small.jpeg"),
            ("In-wall Speaker", "in-wall-speaker.jpeg"),
            ("In-wall Subwoofer", "in-wall-subwoofer.jpeg"),
            ("Landscape Speaker", "landscape-speaker.jpeg"),
            ("Large Motorized Bug Screen", "large-motorized-bug-screen.jpeg"),
            ("Lighting Control Keypad", "lighting-control-keypad.png"),
            ("Lighting Control Panel", "lighting-control-panel.jpeg"),
            ("Lighting Dimmer", "lighting-dimmer.jpeg"),
            ("Lighting Swtich", "lighting-switch.png"),
            ("Medium Motorized Bug Screen", "medium-motorized-bug-screen.jpeg"),
            ("Motorized Drapery Track", "motorized-drapery-track.jpeg"),
            ("Motorized Projector Screen", "motorized-projector-screen.jpeg"),
            ("Outdoor Speakers", "outdoor-speakers.jpeg"),
            ("Outdoor Subwoofer", "outdoor-subwoofer.jpeg"),
            ("Outdoor Wireless Access Point", "outdoor-wireless-access-point.jpeg"),
            ("Pre-wired Video Outlet", "pre-wired-video-outlet.jpeg"),
            ("Shade Power Panel", "shade-power-panel.jpeg"),
            ("Small Single Roller", "small-single-roller.jpeg"),
            ("Soundbar", "soundbar.png"),
            ("Video Projector", "video-projector.jpeg"),
            ("Wireless Access Point", "wireless-access-point.jpeg")
        };
    }
}
