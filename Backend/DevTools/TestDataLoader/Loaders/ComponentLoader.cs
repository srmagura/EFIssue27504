namespace TestDataLoader.Loaders;

internal class ComponentLoader
{
    private readonly IComponentAppService _componentAppService;
    private readonly IComponentTypeAppService _componentTypeAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    private readonly Bogus.DataSets.Lorem _lorem = new();
    private readonly Randomizer _randomizer = new();
    private int _increment = 0;

    public ComponentLoader(
        IComponentAppService componentAppService,
        IComponentTypeAppService componentComponentTypeAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _componentAppService = componentAppService;
        _componentTypeAppService = componentComponentTypeAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddComponentsAsync()
    {
        List<OrganizationId> organizationIds;
        using (var db = _dbFactory())
        {
            organizationIds = db.Organizations
                .Where(p => p.IsActive)
                .Select(organization => new OrganizationId(organization.Id))
                .ToList();
        }

        var versionName = DateTime.UtcNow.ToString("MMMyy");
        var versionName2 = $"{versionName} (2)";

        foreach (var organizationId in organizationIds)
        {
            _increment = 0;

            var componentTypes = await _componentTypeAppService.ListAsync(organizationId, ActiveFilter.All);

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                true,
                true,
                "Good TV",
                versionName,
                499.99m,
                "https://www.bestbuy.com/site/samsung-65-class-7-series-led-4k-uhd-smart-tizen-tv/6401722.p?skuId=6401722",
                make: "Samsung",
                model: "65inch Class 7 Series LED 4K UHD",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                true,
                true,
                "Better TV",
                versionName,
                1099.99m,
                "https://www.bestbuy.com/site/samsung-65-class-q70a-series-qled-4k-uhd-smart-tizen-tv/6451476.p?skuId=6451476",
                make: "Samsung",
                model: "65inch Class Q70A Series QLED 4K UHD",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                true,
                true,
                "Best TV",
                versionName,
                1899.99m,
                "https://www.bestbuy.com/site/samsung-65-class-qn90a-neo-qled-4k-uhd-smart-tizen-tv/6452320.p?skuId=6452320",
                make: "Samsung",
                model: "65inch Class QN90A Neo QLED 4K UHD",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Advanced Tilt 4D TV Wall Mount",
                versionName,
                169.99m,
                "https://www.bestbuy.com/site/sanus-advanced-tilt-4d-tv-wall-mount-for-tvs-42-90-black/6450295.p?skuId=6450295",
                make: "Sanus",
                model: "42inch - 90inch",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "HDMI Cable",
                versionName,
                5.99m,
                "https://www.bestbuy.com/site/best-buy-essentials-3-4k-ultra-hd-hdmi-cable-black/6472357.p?skuId=6472357",
                make: "Best Buy",
                model: "3' 4K Ultra HD",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "5.2 channel Receiver",
                versionName,
                349.99m,
                "https://www.bestbuy.com/site/denon-avr-s540bt-receiver-5-2-channel-4k-ultra-hd-audio-and-video-home-theater-system-built-in-bluetooth-and-usb-black/6210100.p?skuId=6210100",
                make: "Denon",
                model: "AVR-S540BT",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Bookshelf Speaker Pair",
                versionName,
                149.99m,
                "https://www.bestbuy.com/site/polk-audio-monitor-xt15-bookshelf-speaker-pair-midnight-black/6477913.p?skuId=6477913",
                make: "Polk Audio",
                model: "Monitor XT15",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Tower Speaker",
                versionName,
                168.99m,
                "https://www.bestbuy.com/site/polk-audio-monitor-xt60-tower-speaker-midnight-black/6477931.p?skuId=6477931",
                make: "Polk Audio",
                model: "Monitor XT60",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Center Channel Speaker",
                versionName,
                149.99m,
                "https://www.bestbuy.com/site/polk-audio-monitor-xt30-center-channel-speaker-midnight-black/6477932.p?skuId=6477932",
                make: "Polk Audio",
                model: "Monitor XT30 ",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Powered Subwoofer",
                versionName,
                249.99m,
                "https://www.bestbuy.com/site/polk-audio-psw108-10-powered-subwoofer-100w-peak-power-explosive-performance-for-movies-music-black-black/9154073.p?skuId=9154073",
                make: "Polk Audio",
                model: "PSW108 10inch",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Doorbell",
                versionName,
                169.99m,
                "https://www.bestbuy.com/site/ring-video-doorbell-4-smart-wi-fi-video-doorbell-wired-battery-operated-satin-nickel/6459035.p?skuId=6459035",
                make: "Ring",
                model: "Video Doorbell 4 - Smart Wi-Fi",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Spotlight Cam",
                versionName,
                199.99m,
                "https://www.bestbuy.com/site/ring-spotlight-cam-wire-free-white/5936903.p?skuId=5936903",
                make: "Ring",
                model: "Wire-free",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Floodlight Cam",
                versionName,
                249.99m,
                "https://www.bestbuy.com/site/ring-floodlight-cam-wired-pro-outdoor-wireless-1080p-surveillance-camera-white/6456121.p?skuId=6456121",
                make: "Ring",
                model: "Wired Pro Outdoor Wireless 1080p",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Smart Display with Alexa",
                versionName,
                44.99m,
                "https://www.bestbuy.com/site/amazon-echo-show-5-2nd-gen-smart-display-with-alexa-charcoal/6461319.p?skuId=6461319",
                make: "Amazon",
                model: "Echo Show 5 (2nd Gen)",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "Base Station",
                versionName,
                249.99m,
                "https://www.bestbuy.com/site/ring-alarm-pro-base-station-white/6481922.p?skuId=6481922",
                make: "Ring",
                model: "Alarm Pro",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "Rack",
                versionName,
                579.00m,
                "https://www.discount-low-voltage.com/Data-Enclosures-Racks-Shelves/4-Post-Relay-Racks/KH-1940-3-001-41",
                make: "Kendall Howard",
                model: "41U Knockdown Open Frame",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "Access Point",
                versionName,
                69.99m,
                "https://www.netgear.com/business/wifi/access-points/wax202/",
                make: "Netgear",
                model: "WiFi 6 AX1800 Dual Band",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "Faceplate",
                versionName,
                8.99m,
                "https://www.amazon.com/Port-Ethernet-Wall-Plate-Faceplate/dp/B07YX1LC23/ref=asc_df_B07YX1LC23/?tag=&linkCode=df0&hvadid=385180287745&hvpos=&hvnetw=g&hvrand=4168373040234070436&hvpone=&hvptwo=&hvqmt=&hvdev=c&hvdvcmdl=&hvlocint=&hvlocphy=9009736&hvtargid=pla-865757054154&ref=&adgrpid=78829231856&th=1",
                make: "Amazon",
                model: "2 Port Ethernet Cat6 RJ45 Network Female to Female",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "1-pack Light",
                versionName,
                22.99m,
                "https://www.philips-hue.com/en-us/p/hue-white-ambiance-1-pack-e26/046677548490#overview",
                make: "Hue",
                model: "E26",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Smart button",
                versionName,
                24.99m,
                "https://www.philips-hue.com/en-us/p/hue-smart-button/046677553715",
                make: "Hue",
                model: "v2",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Motion sensor",
                versionName,
                39.99m,
                "https://www.philips-hue.com/en-us/p/hue-motion-sensor/046677473389",
                make: "Hue",
                model: "v3",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "Bridge",
                versionName,
                59.99m,
                "https://www.philips-hue.com/en-us/p/hue-bridge/046677458478#overview",
                make: "Hue",
                model: "v1",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "Wall Plate",
                versionName,
                15.33m,
                "https://www.walmart.com/ip/RiteAV-Power-Outlet-3-HDMI-White-Coax-Wall-Plate-White/177386937?wmlspartner=wlpa&selectedSellerId=18854",
                make: "RiteAV",
                model: "Power Outlet 3 HDMI White Coax",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                true,
                true,
                "Projector Screen",
                versionName,
                3618.00m,
                "https://www.bestbuy.com/site/elite-screens-saker-tab-tension-135-motorized-projector-screen-black/5203042.p?skuId=5203042",
                make: "Elite Screens",
                model: "Saker Tab-Tension 135inch Motorized",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                true,
                true,
                "Home Cinema Projector",
                versionName,
                1699.99m,
                "https://www.bestbuy.com/site/epson-home-cinema-3800-4k-3lcd-projector-with-high-dynamic-range-white/6366530.p?skuId=6366530",
                make: "Epson",
                model: "3800 4K 3LCD",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "In - Wall Subwoofer",
                versionName,
                1799.98m,
                "https://www.bestbuy.com/site/sonance-visual-performance-10-passive-in-wall-subwoofer-paintable-white/6253955.p?skuId=6253955",
                make: "Sonance",
                model: "Visual Performance 10inch Passive",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Outdoor Speakers(Pair)",
                versionName,
                119.99m,
                "https://www.bestbuy.com/site/yamaha-natural-sound-5-2-way-all-weather-outdoor-speakers-pair-black/8837672.p?skuId=8837672",
                make: "Yamaha",
                model: "Natural Sound 5inch 2 - Way All - Weather",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "2 - Way Outdoor Speaker",
                versionName,
                659.98m,
                "https://www.bestbuy.com/site/sonance-landscape-series-6-1-2-2-way-outdoor-speaker-each-dark-brown/6253967.p?skuId=6253967",
                make: "Sonance",
                model: "Landscape Series 6-1/2inch",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Hardscape Subwoofer",
                versionName,
                2419.98m,
                "https://www.bestbuy.com/site/sonance-landscape-series-12-passive-hardscape-subwoofer-dark-brown/6253968.p?skuId=6253968",
                make: "Sonance",
                model: "Landscape Series 12inch Passive",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Smart Soundbar",
                versionName,
                899.99m,
                "https://www.bestbuy.com/site/bose-smart-soundbar-900-with-dolby-atmos-and-voice-assistant-black/6470267.p?skuId=6470267",
                make: "Bose",
                model: "900 With Dolby Atmos",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "NanoStation Outdoor",
                versionName,
                89.99m,
                "https://www.newegg.com/ubiquiti-nsm2-us/p/0ED-0005-00005?Description=outdoor%20access%20point&cm_re=outdoor_access%20point-_-0ED-0005-00005-_-Product",
                make: "Ubiquiti",
                model: "2.4GHz 11dB 150Mbps CPE (NSM2-US) US Version",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "In-Wall Touchscreen Control",
                versionName,
                549.99m,
                "https://www.bestbuy.com/site/brilliant-smart-home-control-4-switch-panel-in-wall-touchscreen-control-for-lights-music-more-white/6476332.p?skuId=6476332",
                make: "Brilliant",
                model: "Smart Home Control - 4-Switch Panel",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Remote Control Dimmer Keypad",
                versionName,
                79.99m,
                "https://www.smarthome.com/products/insteon-2334-232-keypad-dimmer-switch-dual-band-6-button-white",
                make: "Insteon",
                model: "6-Button",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Remote Control Dimmer Switch",
                versionName,
                49.99m,
                "https://www.smarthome.com/products/switchlinc-dimmer-insteon-2477d-remote-control-dimmer-dual-band-white",
                make: "Insteon",
                model: "v2",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Smart Home Panel",
                versionName,
                1499.00m,
                "https://www.bluettipower.com/products/ep500-ups-boxsub-panel?_pos=9&_sid=07edcc717&_ss=r&variant=42105178292443",
                make: "Bluetti",
                model: "v1",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "Shade Power Panel",
                versionName,
                1999.99m,
                "https://www.lutron.com/en-US/pages/default.aspx",
                make: "Lutron",
                model: "v002",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Roller",
                versionName,
                599.99m,
                "https://www.lutron.com/en-US/pages/default.aspx",
                make: "Lutron",
                model: "v6",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Motorized Drapery Track",
                versionName,
                3499.99m,
                "https://www.lutron.com/en-US/pages/default.aspx",
                make: "Lutron",
                model: "v1.2",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Medium Motorized Bug Screen",
                versionName,
                2499.99m,
                "https://www.lutron.com/en-US/pages/default.aspx",
                make: "Lutron",
                model: "0062",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                true,
                "Large Motorized Bug Screen",
                versionName,
                2999.99m,
                "https://www.lutron.com/en-US/pages/default.aspx",
                make: "Lutron",
                model: "v6.7",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.AddAsync(
                organizationId: organizationId,
                componentTypeId: _randomizer.ArrayElement(componentTypes).Id,
                measurementType: MeasurementType.Linear,
                isVideoDisplay: false,
                visibleToCustomer: true,
                displayName: "Linear Light Fixture",
                versionName: versionName,
                sellPrice: 300m,
                url: "https://example.com",
                make: "Cartwright",
                model: "C123",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: null,
                internalNotes: GetInternalNotes()
            );

            var inactiveComponentId = await _componentAppService.AddAsync(
                organizationId,
                _randomizer.ArrayElement(componentTypes).Id,
                MeasurementType.Normal,
                false,
                false,
                "Inactive Component",
                versionName,
                1.00m,
                null,
                make: "OOB",
                model: "outdated model",
                vendorPartNumber: GetPartNumber(true)!,
                organizationPartNumber: GetPartNumber(false),
                whereToBuy: null,
                style: null,
                color: GetColor(),
                internalNotes: GetInternalNotes()
            );

            await _componentAppService.SetActiveAsync(inactiveComponentId, false);

            await AddVersionsAsync(organizationId, versionName2);
        }

        _monitor.WriteCompletedMessage("Added components.");
    }

    private async Task AddVersionsAsync(OrganizationId organizationId, string versionName)
    {
        var componentSummaries = await _componentAppService.ListAsync(organizationId);

        var activeOnly = componentSummaries.Where(p => p.IsActive);

        foreach (var componentSummary in activeOnly)
        {
            var component = await _componentAppService.GetAsync(componentSummary.Id);
            if (component == null)
                throw new Exception("Component is unexpectedly null.");

            var version1 = component.Versions.Single();

            await _componentAppService.AddVersionAsync(
                id: component.Id,
                displayName: version1.DisplayName,
                versionName: versionName,
                sellPrice: Math.Round(version1.SellPrice * 1.1m) - 0.01m,
                url: version1.Url,
                make: version1.Make,
                model: version1.Model,
                vendorPartNumber: version1.VendorPartNumber,
                organizationPartNumber: version1.OrganizationPartNumber,
                whereToBuy: version1.WhereToBuy,
                style: version1.Style,
                color: version1.Color,
                internalNotes: version1.InternalNotes
            );
        }
    }

    private string GetInternalNotes()
    {
        return _lorem.Sentences(sentenceCount: _randomizer.Number(1, 4), separator: " ");
    }

    private string? GetColor()
    {
        return _randomizer.Double() < 0.5 ? "black" : null;
    }

    private string? GetPartNumber(bool isVendorPartNumber)
    {
        if (isVendorPartNumber || _randomizer.Double() < 0.5)
        {
            _increment++;
            return $"{_randomizer.AlphaNumeric(8).ToUpper()}{_increment}";
        }

        return null;
    }
}
