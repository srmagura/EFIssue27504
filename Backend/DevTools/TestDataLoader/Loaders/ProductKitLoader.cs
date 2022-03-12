using AppDTOs;

namespace TestDataLoader.Loaders;

internal class ProductKitLoader
{
    private readonly IProductKitAppService _productKitAppService;
    private readonly IComponentAppService _componentAppService;
    private readonly ICategoryAppService _categoryAppService;
    private readonly ISymbolAppService _symbolAppService;
    private readonly IProductPhotoAppService _productPhotoAppService;
    private readonly IProductFamilyAppService _productFamilyAppService;
    private readonly TestDataMonitor _testDataMonitor;
    private readonly Func<AppDataContext> _dbFactory;

    public ProductKitLoader(
        IProductKitAppService productKitAppService,
        IComponentAppService componentAppService,
        ICategoryAppService categoryAppService,
        ISymbolAppService symbolAppService,
        IProductPhotoAppService productPhotoAppService,
        IProductFamilyAppService productFamilyAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _productKitAppService = productKitAppService;
        _componentAppService = componentAppService;
        _categoryAppService = categoryAppService;
        _symbolAppService = symbolAppService;
        _productPhotoAppService = productPhotoAppService;
        _productFamilyAppService = productFamilyAppService;
        _testDataMonitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddProductKitsAsync()
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

        var loremSet = new Bogus.DataSets.Lorem();

        foreach (var organizationId in organizationIds)
        {
            var components = await _componentAppService.ListAsync(organizationId);
            var categories = await _categoryAppService.GetCategoryTreeAsync(organizationId, ActiveFilter.ActiveOnly);
            var symbols = await _symbolAppService.ListAsync(organizationId, 0, 10000, ActiveFilter.All, search: null);
            var productPhotos = await _productPhotoAppService.ListAsync(organizationId, 0, 1000, ActiveFilter.All, search: null);
            var productFamilies = await _productFamilyAppService.ListAsync(organizationId, 0, 1000, ActiveFilter.All, search: null);

            var avCategoryId = categories.Children.First(p => p.Category!.Name.Contains("Audio & Video")).Category!.Id;
            var dataCategoryId = categories.Children.First(p => p.Category!.Name.Contains("Voice, Data & Infastructure")).Category!.Id;
            var lightingCategoryId = categories.Children.First(p => p.Category!.Name.Contains("Lighting Control")).Category!.Id;
            var securityCategoryId = categories.Children.First(p => p.Category!.Name.Contains("Safety & Security")).Category!.Id;
            var motorizedShadesCategoryId = categories.Children.First(p => p.Category!.Name.Contains("Motorized Shades")).Category!.Id;

            // AUDIO & VIDEO

            var goodTvComponent = components.First(p => p.DisplayName.Contains("Good TV"));
            var betterTvComponent = components.First(p => p.DisplayName.Contains("Better TV"));
            var bestTvComponent = components.First(p => p.DisplayName.Contains("Best TV"));
            var wallMountComponent = components.First(p => p.DisplayName.Contains("Wall Mount"));
            var hdmiCableComponent = components.First(p => p.DisplayName.Contains("HDMI Cable"));

            var avOutletComponent = components.First(p => p.DisplayName.Contains("Wall Plate"));
            var projectorScreenComponent = components.First(p => p.DisplayName.Contains("Projector Screen"));
            var projectorComponent = components.First(p => p.DisplayName.Contains("Home Cinema Projector"));
            var inwallSubwooferComponent = components.First(p => p.DisplayName.Contains("In - Wall Subwoofer"));
            var soundbarComponent = components.First(p => p.DisplayName.Contains("Smart Soundbar"));
            var outdoorSpeakerComponent = components.First(p => p.DisplayName.Contains("Outdoor Speakers(Pair)"));
            var landscapeSpeakerComponent = components.First(p => p.DisplayName.Contains("2 - Way Outdoor Speaker"));
            var outdoorSubwooferComponent = components.First(p => p.DisplayName.Contains("Hardscape Subwoofer"));

            var receiverComponent = components.First(p => p.DisplayName.Contains("Receiver"));
            var bookshelfSpeakerComponent = components.First(p => p.DisplayName.Contains("Bookshelf Speaker"));
            var towerSpeakerComponent = components.First(p => p.DisplayName.Contains("Tower Speaker"));
            var centerSpeaker = components.First(p => p.DisplayName.Contains("Center Channel Speaker"));
            var subwooferComponent = components.First(p => p.DisplayName.Contains("Subwoofer"));

            var bestTvComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, bestTvComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, wallMountComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, hdmiCableComponent.CurrentVersionId, 2)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Best HD TV",
                loremSet.Sentences(7), // testing long description
                versionName,
                symbols.Items.First(p => p.Name.Contains("65 Inch Television")).Id,
                productPhotos.Items.First(p => p.Name.Contains("65 Inch TV")).Id,
                bestTvComponent.CurrentVersionId,
                bestTvComponentInputDtos,
                productFamilies.Items.First(p => p.Name == "HD TVs").Id
            );

            var betterTvComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, betterTvComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, wallMountComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, hdmiCableComponent.CurrentVersionId, 2)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Better HD TV",
                loremSet.Sentences(7), // testing long description
                versionName,
                symbols.Items.First(p => p.Name.Contains("65 Inch Television")).Id,
                productPhotos.Items.First(p => p.Name.Contains("65 Inch TV")).Id,
                betterTvComponent.CurrentVersionId,
                betterTvComponentInputDtos,
                productFamilies.Items.First(p => p.Name == "HD TVs").Id
            );

            var goodTvComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, goodTvComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, wallMountComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, hdmiCableComponent.CurrentVersionId, 2)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Good HD TV",
                loremSet.Sentences(7), // testing long description
                versionName,
                symbols.Items.First(p => p.Name.Contains("65 Inch Television")).Id,
                productPhotos.Items.First(p => p.Name.Contains("65 Inch TV")).Id,
                goodTvComponent.CurrentVersionId,
                goodTvComponentInputDtos,
                productFamilies.Items.First(p => p.Name == "HD TVs").Id
            );

            var surroundSoundComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, receiverComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, bookshelfSpeakerComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, towerSpeakerComponent.CurrentVersionId, 2),
                new ProductKitComponentMapInputDto(id: null, centerSpeaker.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, subwooferComponent.CurrentVersionId, 2),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Surround Sound",
                "5.2 surround sound setup.",
                versionName,
                symbols.Items.First(p => p.Name.Contains("AV Receiver")).Id,
                productPhotoId: null,
                receiverComponent.CurrentVersionId,
                surroundSoundComponentInputDtos,
                productFamilyId: null
            );

            var homeTheaterComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, bestTvComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, receiverComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, bookshelfSpeakerComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, towerSpeakerComponent.CurrentVersionId, 2),
                new ProductKitComponentMapInputDto(id: null, centerSpeaker.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, subwooferComponent.CurrentVersionId, 2),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Home Theater",
                "Home theater with 5.2 surround sound.",
                versionName,
                symbols.Items.First(p => p.Name.Contains("65 Inch Television")).Id,
                productPhotos.Items.First(p => p.Name.Contains("AV Receiver")).Id,
                bestTvComponent.CurrentVersionId,
                homeTheaterComponentInputDtos,
                productFamilyId: null
            );

            var projectorComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, projectorComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, projectorScreenComponent.CurrentVersionId, 1),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Projector setup",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Video Projection")).Id,
                productPhotoId: null,
                projectorComponent.CurrentVersionId,
                projectorComponentInputDtos,
                productFamilyId: null
            );

            var theaterWithProjectorComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, projectorComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, projectorScreenComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, receiverComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, bookshelfSpeakerComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, towerSpeakerComponent.CurrentVersionId, 2),
                new ProductKitComponentMapInputDto(id: null, centerSpeaker.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, subwooferComponent.CurrentVersionId, 2),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Home theater with projector",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Video Projection")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Video Projector")).Id,
                projectorComponent.CurrentVersionId,
                theaterWithProjectorComponentInputDtos,
                productFamilyId: null
            );

            var outdoorTvComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, bestTvComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, outdoorSpeakerComponent.CurrentVersionId, 2),
                new ProductKitComponentMapInputDto(id: null, outdoorSubwooferComponent.CurrentVersionId, 1),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Outdoor tv",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Television")).Id,
                productPhotoId: null,
                bestTvComponent.CurrentVersionId,
                outdoorTvComponentInputDtos,
                productFamilyId: null
            );

            var tvWithSoundbarComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, bestTvComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, soundbarComponent.CurrentVersionId, 1),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "TV with sound bar",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Soundbar")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Soundbar")).Id,
                bestTvComponent.CurrentVersionId,
                tvWithSoundbarComponentInputDtos,
                productFamilyId: null
            );

            var outdoorSoundComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, receiverComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, outdoorSpeakerComponent.CurrentVersionId, 10),
                new ProductKitComponentMapInputDto(id: null, outdoorSubwooferComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, landscapeSpeakerComponent.CurrentVersionId, 10),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Outdoor sound system",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Outdoor Speakers")).Id,
                productPhotoId: null,
                outdoorSpeakerComponent.CurrentVersionId,
                outdoorSoundComponentInputDtos,
                productFamilyId: null
            );

            var outdoorTheaterComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, bestTvComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, receiverComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, outdoorSpeakerComponent.CurrentVersionId, 5),
                new ProductKitComponentMapInputDto(id: null, outdoorSubwooferComponent.CurrentVersionId, 2),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Outdoor theater",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Television")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Landscape Speaker")).Id,
                bestTvComponent.CurrentVersionId,
                outdoorTheaterComponentInputDtos,
                productFamilyId: null
            );

            var wholeHosueSoundComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, receiverComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, bookshelfSpeakerComponent.CurrentVersionId, 20),
                new ProductKitComponentMapInputDto(id: null, inwallSubwooferComponent.CurrentVersionId, 5),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Whole house sound system",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("LCR Speakers")).Id,
                productPhotoId: null,
                receiverComponent.CurrentVersionId,
                wholeHosueSoundComponentInputDtos,
                productFamilyId: null
            );

            var gameRoomComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, bestTvComponent.CurrentVersionId, 5),
                new ProductKitComponentMapInputDto(id: null, avOutletComponent.CurrentVersionId, 5),
                new ProductKitComponentMapInputDto(id: null, hdmiCableComponent.CurrentVersionId, 5),
                new ProductKitComponentMapInputDto(id: null, wallMountComponent.CurrentVersionId, 5),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Game room",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Television")).Id,
                productPhotos.Items.First(p => p.Name.Contains("65 Inch TV")).Id,
                bestTvComponent.CurrentVersionId,
                gameRoomComponentInputDtos,
                productFamilyId: null
            );

            var barComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, bestTvComponent.CurrentVersionId, 3),
                new ProductKitComponentMapInputDto(id: null, wallMountComponent.CurrentVersionId, 3),
                new ProductKitComponentMapInputDto(id: null, avOutletComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, soundbarComponent.CurrentVersionId, 3),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Bar setup",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Television")).Id,
                productPhotoId: null,
                bestTvComponent.CurrentVersionId,
                barComponentInputDtos,
                productFamilyId: null
            );

            var outdoorBarComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, bestTvComponent.CurrentVersionId, 3),
                new ProductKitComponentMapInputDto(id: null, wallMountComponent.CurrentVersionId, 3),
                new ProductKitComponentMapInputDto(id: null, avOutletComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, outdoorSpeakerComponent.CurrentVersionId, 6)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Outdoor bar setup",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Television")).Id,
                productPhotos.Items.First(p => p.Name.Contains("65 Inch TV")).Id,
                bestTvComponent.CurrentVersionId,
                outdoorBarComponentInputDtos,
                productFamilyId: null
            );

            var kitchenSoundComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, receiverComponent.CurrentVersionId, 1),
                new ProductKitComponentMapInputDto(id: null, bookshelfSpeakerComponent.CurrentVersionId, 4)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                avCategoryId,
                "Kitchen sound system",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Sonos Sub")).Id,
                productPhotoId: null,
                receiverComponent.CurrentVersionId,
                kitchenSoundComponentInputDtos,
                productFamilyId: null
            );

            // SAFETY & SECURITY

            var doorbellComponentId = components.First(p => p.DisplayName.Contains("Doorbell")).Id;
            var doorbellComponent = await _componentAppService.GetAsync(doorbellComponentId);
            var spotlightCamComponentId = components.First(p => p.DisplayName.Contains("Spotlight Cam")).Id;
            var spotlightCamComponent = await _componentAppService.GetAsync(spotlightCamComponentId);
            var floodlightCamComponentId = components.First(p => p.DisplayName.Contains("Floodlight Cam")).Id;
            var floodlightCamComponent = await _componentAppService.GetAsync(floodlightCamComponentId);
            var echoComponentId = components.First(p => p.DisplayName.Contains("Smart Display")).Id;
            var echoComponent = await _componentAppService.GetAsync(echoComponentId);
            var alarmBaseStationComponentId = components.First(p => p.DisplayName.Contains("Base Station")).Id;
            var alarmBaseStationComponent = await _componentAppService.GetAsync(alarmBaseStationComponentId);

            var ringAlarmComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, doorbellComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, spotlightCamComponent!.Versions.First(p => p.VersionName == versionName).Id, 2),
                new ProductKitComponentMapInputDto(id: null, floodlightCamComponent!.Versions.First(p => p.VersionName == versionName).Id, 2),
                new ProductKitComponentMapInputDto(id: null, echoComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, alarmBaseStationComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                securityCategoryId,
                "Ring Alarm System",
                "Ring alarm system with doorbell and cameras.",
                versionName,
                symbols.Items.First(p => p.Name.Contains("Access Control Door Station")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Access Control Keypad")).Id,
                alarmBaseStationComponent.Versions.First(p => p.VersionName == versionName).Id,
                ringAlarmComponentInputDtos,
                productFamilyId: null
            );

            // VOICE, DATA & INFRASTRUCTURE

            var rackComponentId = components.First(p => p.DisplayName.Contains("Rack")).Id;
            var rackComponent = await _componentAppService.GetAsync(rackComponentId);
            var wapComponentId = components.First(p => p.DisplayName.Contains("Access Point")).Id;
            var wapComponent = await _componentAppService.GetAsync(wapComponentId);
            var dataPortComponentId = components.First(p => p.DisplayName.Contains("Faceplate")).Id;
            var dataPortComponent = await _componentAppService.GetAsync(dataPortComponentId);
            var outdoorWapComponentId = components.First(p => p.DisplayName.Contains("NanoStation Outdoor")).Id;
            var outdoorWapComponent = await _componentAppService.GetAsync(outdoorWapComponentId);

            var dataClosetComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, rackComponent!.Versions.First(p => p.VersionName == versionName).Id, 2),
                new ProductKitComponentMapInputDto(id: null, dataPortComponent!.Versions.First(p => p.VersionName == versionName).Id, 5)
            };

            await _productKitAppService.AddAsync(
               organizationId,
               dataCategoryId,
               "Data closet",
               loremSet.Sentence(),
               versionName,
               symbols.Items.First(p => p.Name.Contains("Data Outlet")).Id,
               productPhotoId: null,
               rackComponent!.Versions.First(p => p.VersionName == versionName).Id,
               dataClosetComponentInputDtos,
               productFamilyId: null
           );

            var dataNetworkComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, wapComponent!.Versions.First(p => p.VersionName == versionName).Id, 10),
                new ProductKitComponentMapInputDto(id: null, dataPortComponent!.Versions.First(p => p.VersionName == versionName).Id, 20)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                dataCategoryId,
                "Data network",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Satellite Speakers")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Wireless Access Point")).Id,
                dataPortComponent!.Versions.First(p => p.VersionName == versionName).Id,
                dataNetworkComponentInputDtos,
                productFamilyId: null
            );

            var wifiComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, wapComponent!.Versions.First(p => p.VersionName == versionName).Id, 10),
                new ProductKitComponentMapInputDto(id: null, rackComponent!.Versions.First(p => p.VersionName == versionName).Id, 3)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                dataCategoryId,
                "Wireless network",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Indoor Wireless")).Id,
                productPhotoId: null,
                wapComponent!.Versions.First(p => p.VersionName == versionName).Id,
                wifiComponentInputDtos,
                productFamilyId: null
            );

            var outdoorWifiComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, outdoorWapComponent!.Versions.First(p => p.VersionName == versionName).Id, 5),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                dataCategoryId,
                "Outdoor wireless network",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Outdoor Wireless")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Wireless Access Point")).Id,
                outdoorWapComponent!.Versions.First(p => p.VersionName == versionName).Id,
                outdoorWifiComponentInputDtos,
                productFamilyId: null
            );

            // LIGHTING CONTROL

            var e26ComponentId = components.First(p => p.DisplayName.Contains("1-pack Light")).Id;
            var e26Component = await _componentAppService.GetAsync(e26ComponentId);
            var smartButtonComponentId = components.First(p => p.DisplayName.Contains("Smart button")).Id;
            var smartButtonComponent = await _componentAppService.GetAsync(smartButtonComponentId);
            var motionSensorComponentId = components.First(p => p.DisplayName.Contains("Motion sensor")).Id;
            var motionSensorComponent = await _componentAppService.GetAsync(motionSensorComponentId);
            var bridgeComponentId = components.First(p => p.DisplayName.Contains("Bridge")).Id;
            var bridgeComponent = await _componentAppService.GetAsync(bridgeComponentId);

            var lightingPanelComponentId = components.First(p => p.DisplayName.Contains("Smart Home Panel")).Id;
            var lightingPanelComponent = await _componentAppService.GetAsync(lightingPanelComponentId);
            var lightingKeypadComponentId = components.First(p => p.DisplayName.Contains("Dimmer Keypad")).Id;
            var lightingKeypadComponent = await _componentAppService.GetAsync(lightingKeypadComponentId);
            var lightingDimmerComponentId = components.First(p => p.DisplayName.Contains("Dimmer Switch")).Id;
            var lightingDimmerComponent = await _componentAppService.GetAsync(lightingDimmerComponentId);
            var lightingSwitchComponentId = components.First(p => p.DisplayName.Contains("Touchscreen Control")).Id;
            var lightingSwitchComponent = await _componentAppService.GetAsync(lightingSwitchComponentId);

            var linearLightFixtureComponentId = components.First(p => p.DisplayName.Contains("Linear Light Fixture")).Id;
            var linearLightFixtureComponent = await _componentAppService.GetAsync(linearLightFixtureComponentId);

            var smartLightingComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, e26Component!.Versions.First(p => p.VersionName == versionName).Id, 20),
                new ProductKitComponentMapInputDto(id: null, smartButtonComponent!.Versions.First(p => p.VersionName == versionName).Id, 3),
                new ProductKitComponentMapInputDto(id: null, motionSensorComponent!.Versions.First(p => p.VersionName == versionName).Id, 10),
                new ProductKitComponentMapInputDto(id: null, bridgeComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                lightingCategoryId,
                "Smart Lighting",
                "Smart lighting with controls and motion sensors.",
                versionName,
                symbols.Items.First(p => p.Name.Contains("Lighting Control Headend Equipment")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Lighting Control Keypad")).Id,
                bridgeComponent!.Versions.First(p => p.VersionName == versionName).Id,
                smartLightingComponentInputDtos,
                productFamilyId: null
            );

            var bedroomLightingComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, e26Component!.Versions.First(p => p.VersionName == versionName).Id, 4),
                new ProductKitComponentMapInputDto(id: null, lightingSwitchComponent!.Versions.First(p => p.VersionName == versionName).Id, 2),
                new ProductKitComponentMapInputDto(id: null, lightingDimmerComponent!.Versions.First(p => p.VersionName == versionName).Id, 1)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                lightingCategoryId,
                "Bedroom lighting",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Lighting Control Headend Equipment")).Id,
                productPhotoId: null,
                lightingSwitchComponent!.Versions.First(p => p.VersionName == versionName).Id,
                bedroomLightingComponentInputDtos,
                productFamilyId: null
            );

            var lightingControlPanelComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, lightingPanelComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, lightingSwitchComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, lightingDimmerComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, lightingKeypadComponent!.Versions.First(p => p.VersionName == versionName).Id, 1)
            };

            await _productKitAppService.AddAsync(
                organizationId,
                lightingCategoryId,
                "Lighting control panel",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("SeeTouch Keypad")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Lighting Control Panel")).Id,
                lightingPanelComponent!.Versions.First(p => p.VersionName == versionName).Id,
                lightingControlPanelComponentInputDtos,
                productFamilyId: null
            );

            var patioLightingComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, lightingKeypadComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, e26Component!.Versions.First(p => p.VersionName == versionName).Id, 6),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                lightingCategoryId,
                "Patio lighting",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Lighting Control Headend Equipment")).Id,
                productPhotoId: null,
                lightingKeypadComponent!.Versions.First(p => p.VersionName == versionName).Id,
                patioLightingComponentInputDtos,
                productFamilyId: null
            );

            var linearLightFixtureComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, linearLightFixtureComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, lightingSwitchComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
            };

            var linearLightFixtureProductFamilyId = productFamilies.Items.First(p => p.Name == "Linear Light Fixtures").Id;

            await _productKitAppService.AddAsync(
                organizationId,
                lightingCategoryId,
                "Linear Light Fixture",
                "A light that comes in various lengths.",
                versionName,
                symbols.Items.First(p => p.Name.Contains("Lighting Control Headend Equipment")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Lighting Control Keypad")).Id,
                linearLightFixtureComponent!.Versions.First(p => p.VersionName == versionName).Id,
                linearLightFixtureComponentInputDtos,
                productFamilyId: linearLightFixtureProductFamilyId
            );

            await _productKitAppService.AddAsync(
                organizationId,
                lightingCategoryId,
                "Better Linear Light Fixture",
                "A light that comes in various lengths.",
                versionName,
                symbols.Items.First(p => p.Name.Contains("Lighting Control Headend Equipment")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Lighting Control Keypad")).Id,
                linearLightFixtureComponent!.Versions.First(p => p.VersionName == versionName).Id,
                linearLightFixtureComponentInputDtos,
                productFamilyId: linearLightFixtureProductFamilyId
            );

            // MOTORIZED SHADES

            var shadePowerPanelComponentId = components.First(p => p.DisplayName.Contains("Shade Power Panel")).Id;
            var shadePowerPanelComponent = await _componentAppService.GetAsync(shadePowerPanelComponentId);
            var rollerComponentId = components.First(p => p.DisplayName.Contains("Roller")).Id;
            var rollerComponent = await _componentAppService.GetAsync(rollerComponentId);
            var draperyComponentId = components.First(p => p.DisplayName.Contains("Motorized Drapery Track")).Id;
            var draperyComponent = await _componentAppService.GetAsync(draperyComponentId);
            var mediumBugScreenComponentId = components.First(p => p.DisplayName.Contains("Medium Motorized Bug Screen")).Id;
            var mediumBugScreenComponent = await _componentAppService.GetAsync(mediumBugScreenComponentId);
            var largeBugScreenComponentId = components.First(p => p.DisplayName.Contains("Large Motorized Bug Screen")).Id;
            var largeBugScreenComponent = await _componentAppService.GetAsync(largeBugScreenComponentId);

            var livingRoomShadesComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, shadePowerPanelComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, rollerComponent!.Versions.First(p => p.VersionName == versionName).Id, 4),
                new ProductKitComponentMapInputDto(id: null, draperyComponent!.Versions.First(p => p.VersionName == versionName).Id, 2),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                motorizedShadesCategoryId,
                "Living room shades",
                "Motoroized shades, drapery and bug screens.",
                versionName,
                symbols.Items.First(p => p.Name.Contains("Drape Track")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Drapery Track")).Id,
                shadePowerPanelComponent!.Versions.First(p => p.VersionName == versionName).Id,
                livingRoomShadesComponentInputDtos,
                productFamilyId: null
            );

            var outdoorShadesComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, shadePowerPanelComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, mediumBugScreenComponent!.Versions.First(p => p.VersionName == versionName).Id, 2),
                new ProductKitComponentMapInputDto(id: null, largeBugScreenComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                motorizedShadesCategoryId,
                "Outdoor shades",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Medium Motorized Bug Screen")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Bug Screen")).Id,
                shadePowerPanelComponent!.Versions.First(p => p.VersionName == versionName).Id,
                outdoorShadesComponentInputDtos,
                productFamilyId: null
            );

            var bedroomShadesComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, rollerComponent!.Versions.First(p => p.VersionName == versionName).Id, 4),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                motorizedShadesCategoryId,
                "Bedroom shades",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Roller Shade")).Id,
                productPhotoId: null,
                rollerComponent!.Versions.First(p => p.VersionName == versionName).Id,
                bedroomShadesComponentInputDtos,
                productFamilyId: null
            );

            var masterBedroomShadesComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, rollerComponent!.Versions.First(p => p.VersionName == versionName).Id, 6),
                new ProductKitComponentMapInputDto(id: null, draperyComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                motorizedShadesCategoryId,
                "Master bedroom shades",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Drape Track")).Id,
                productPhotos.Items.First(p => p.Name.Contains("Drapery Track")).Id,
                draperyComponent!.Versions.First(p => p.VersionName == versionName).Id,
                masterBedroomShadesComponentInputDtos,
                productFamilyId: null
            );

            var multiWindowShadesComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, shadePowerPanelComponent!.Versions.First(p => p.VersionName == versionName).Id, 1),
                new ProductKitComponentMapInputDto(id: null, rollerComponent!.Versions.First(p => p.VersionName == versionName).Id, 10),
                new ProductKitComponentMapInputDto(id: null, draperyComponent!.Versions.First(p => p.VersionName == versionName).Id, 4),
            };

            await _productKitAppService.AddAsync(
                organizationId,
                motorizedShadesCategoryId,
                "Multi-window shade setup",
                loremSet.Sentence(),
                versionName,
                symbols.Items.First(p => p.Name.Contains("Drape Track")).Id,
                productPhotoId: null,
                shadePowerPanelComponent!.Versions.First(p => p.VersionName == versionName).Id,
                multiWindowShadesComponentInputDtos,
                productFamilyId: null
            );

            // INACTIVE

            var inactiveComponent = components.First(p => !p.IsActive);

            var inactiveComponentInputDtos = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(id: null, inactiveComponent.CurrentVersionId, 1),
            };

            var inactiveProductKitId = await _productKitAppService.AddAsync(
                organizationId,
                dataCategoryId,
                "Inactive Product Kit",
                "An inactive product kit",
                versionName,
                symbols.Items.First(p => p.Name.Contains("2 Port Data Outlet")).Id,
                productPhotoId: null,
                inactiveComponent.CurrentVersionId,
                inactiveComponentInputDtos,
                productFamilyId: null
           );

            await _productKitAppService.SetActiveAsync(inactiveProductKitId, false);

            await AddVersionsAsync(organizationId, versionName2);
        }

        _testDataMonitor.WriteCompletedMessage("Added product kits.");
    }

    private async Task AddVersionsAsync(OrganizationId organizationId, string versionName)
    {
        var productKitSummaries = await _productKitAppService.ListAsync(organizationId);
        var componentSummaries = await _componentAppService.ListAsync(organizationId);

        var activeProductKitSummaries = productKitSummaries.Where(p => p.IsActive);

        ComponentVersionId GetCurrentVersion(ComponentId componentId)
        {
            return componentSummaries.First(c => c.Id == componentId).CurrentVersionId;
        }

        foreach (var productKitSummary in activeProductKitSummaries)
        {
            var productKit = await _productKitAppService.GetAsync(productKitSummary.Id);
            if (productKit == null)
                throw new Exception("Product kit is unexpectedly null.");

            var version1 = await _productKitAppService.GetVersionAsync(productKit.Versions.Single().Id);
            if (version1 == null)
                throw new Exception("Product kit version is unexpectedly null.");

            var componentMaps = version1.ComponentMaps
                .Select(map => new ProductKitComponentMapInputDto(
                    id: null,
                    componentVersionId: GetCurrentVersion(map.ComponentId),
                    count: map.Count
                ))
                .ToList();

            await _productKitAppService.AddVersionAsync(
                organizationId: organizationId,
                productKitId: productKit.Id,
                name: version1.Name,
                description: version1.Description,
                versionName: versionName,
                mainComponentVersionId: GetCurrentVersion(version1.MainComponentVersion.ComponentId),
                componentMaps: componentMaps
            );
        }
    }
}
