using AppInterfaces;
using AppInterfaces.System;
using AppServices;
using AppServices.System;
using AsposeLicense;
using Autofac;
using Azure.Messaging.ServiceBus;
using DataContext;
using DataInterfaces.Queries;
using DataInterfaces.Repositories;
using DbApplicationImpl;
using EventHandlers;
using FileImport;
using FileStore;
using InfraInterfaces;
using ITI.Baseline.Passwords;
using ITI.DDD.Application;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Application.DomainEvents.Direct;
using ITI.DDD.Logging;
using Logging;
using Permissions;
using Reports;
using ServiceBus;
using ServiceInterfaces;
using Services;
using Settings;

namespace AppConfig;

public class AppModule : Module
{
    private readonly IConfigurationLoader _configurationLoader;
    private readonly bool _isAzure;

    public AppModule(IConfigurationLoader? configurationLoader)
    {
        _configurationLoader = configurationLoader ?? new DefaultConfigurationLoader();

        _isAzure = _configurationLoader.GetSettings<HostSettings>().IsAzure;
    }

    protected override void Load(ContainerBuilder builder)
    {
        AsposeLicenseInitializer.Initialize();

        builder.RegisterModule<ITIDDDModule>();
        builder.RegisterModule<ITIPasswordsModule>();

        builder.RegisterModule<MapperModule>();

        // SETTINGS
        Settings<ConnectionStrings>(builder);
        Settings<IDbLoggerSettings, ConnectionStrings>(builder);

        Settings<AppVersionSettings>(builder);
        Settings<LocalFileStoreSettings>(builder);
        Settings<HostSettings>(builder);
        Settings<ApiAuthenticationSettings>(builder);
        Settings<BugsnagSettings>(builder);
        Settings<ProjectSettings>(builder);
        Settings<PageSettings>(builder);

        // MISC
        RegisterDomainEventPlumbling(builder);
        RegisterDomainEventHandlers(builder);
        RegisterBugsnag(builder);

        builder.RegisterType<AppDataContext>();

        builder.RegisterType<DbLogWriter>();
        builder.RegisterType<BugsnagLogWriter>().As<ILogWriter>();

        builder.RegisterType<DefaultAppPermissions>().As<IAppPermissions>();
        builder.RegisterType<UserAppPermissions>();
        builder.RegisterType<UserRoleService>().As<IUserRoleService>();

        builder.RegisterType<TreeBuilder>();

        if (_isAzure)
        {
            builder.RegisterType<AzureBlobFileStore>().As<IFileStore>();
        }
        else
        {
            builder.RegisterType<LocalFileStore>().As<IFileStore>();
        }

        // DOMAIN SERVICES
        builder.RegisterType<CategoryService>().As<ICategoryService>();

        // APP SERVICES
        builder.RegisterType<UserAppService>().As<IUserAppService>();
        builder.RegisterType<OrganizationAppService>().As<IOrganizationAppService>();
        builder.RegisterType<ProjectAppService>().As<IProjectAppService>();
        builder.RegisterType<SymbolAppService>().As<ISymbolAppService>();
        builder.RegisterType<ProductPhotoAppService>().As<IProductPhotoAppService>();
        builder.RegisterType<CategoryAppService>().As<ICategoryAppService>();
        builder.RegisterType<PageAppService>().As<IPageAppService>();
        builder.RegisterType<ComponentAppService>().As<IComponentAppService>();
        builder.RegisterType<ProductKitAppService>().As<IProductKitAppService>();
        builder.RegisterType<ImportAppService>().As<IImportAppService>();
        builder.RegisterType<ImportSystemAppService>().As<IImportSystemAppService>();
        builder.RegisterType<ProjectPublicationAppService>().As<IProjectPublicationAppService>();
        builder.RegisterType<ReportAppService>().As<IReportAppService>();
        builder.RegisterType<ReportSystemAppService>().As<IReportSystemAppService>();
        builder.RegisterType<DesignerAppService>().As<IDesignerAppService>();
        builder.RegisterType<TermsDocumentAppService>().As<ITermsDocumentAppService>();
        builder.RegisterType<LogoSetAppService>().As<ILogoSetAppService>();
        builder.RegisterType<SheetTypeAppService>().As<ISheetTypeAppService>();
        builder.RegisterType<ProductFamilyAppService>().As<IProductFamilyAppService>();
        builder.RegisterType<ProductKitReferenceAppService>().As<IProductKitReferenceAppService>();
        builder.RegisterType<ComponentTypeAppService>().As<IComponentTypeAppService>();
        builder.RegisterType<OrganizationOptionsAppService>().As<IOrganizationOptionsAppService>();
        builder.RegisterType<ProductRequirementAppService>().As<IProductRequirementAppService>();

        // INFRA SERVICES
        builder.RegisterType<FilePathBuilder>().As<IFilePathBuilder>();
        builder.RegisterType<TempFileService>().As<ITempFileService>();

        builder.RegisterType<DrawingSetReportBuilder>();
        builder.RegisterType<ProposalReportBuilder>();

        builder.RegisterType<ImportProcessor>().As<IImportProcessor>();
        builder.RegisterType<ReportProcessor>().As<IReportProcessor>();

        // REPOSITORIES / QUERIES
        builder.RegisterType<CachedAppPermissionsQueries>().As<IAppPermissionsQueries>();
        builder.RegisterType<EfAppPermissionsQueries>();

        builder.RegisterType<EfUserQueries>().As<IUserQueries>();
        builder.RegisterType<EfUserRepository>().As<IUserRepository>();

        builder.RegisterType<EfOrganizationQueries>().As<IOrganizationQueries>();
        builder.RegisterType<EfOrganizationRepository>().As<IOrganizationRepository>();

        builder.RegisterType<EfProjectQueries>().As<IProjectQueries>();
        builder.RegisterType<EfProjectRepository>().As<IProjectRepository>();

        builder.RegisterType<EfSymbolQueries>().As<ISymbolQueries>();
        builder.RegisterType<EfSymbolRepository>().As<ISymbolRepository>();

        builder.RegisterType<EfProductPhotoQueries>().As<IProductPhotoQueries>();
        builder.RegisterType<EfProductPhotoRepository>().As<IProductPhotoRepository>();

        builder.RegisterType<EfCategoryQueries>().As<ICategoryQueries>();
        builder.RegisterType<EfCategoryRepository>().As<ICategoryRepository>();

        builder.RegisterType<EfImportQueries>().As<IImportQueries>();
        builder.RegisterType<EfImportRepository>().As<IImportRepository>();

        builder.RegisterType<EfProjectPublicationQueries>().As<IProjectPublicationQueries>();
        builder.RegisterType<EfProjectPublicationRepository>().As<IProjectPublicationRepository>();

        builder.RegisterType<EfReportQueries>().As<IReportQueries>();
        builder.RegisterType<EfReportRepository>().As<IReportRepository>();

        builder.RegisterType<EfPageQueries>().As<IPageQueries>();
        builder.RegisterType<EfPageRepository>().As<IPageRepository>();

        builder.RegisterType<EfComponentQueries>().As<IComponentQueries>();
        builder.RegisterType<EfComponentRepository>().As<IComponentRepository>();

        builder.RegisterType<EfProductKitQueries>().As<IProductKitQueries>();
        builder.RegisterType<EfProductKitRepository>().As<IProductKitRepository>();

        builder.RegisterType<EfDesignerQueries>().As<IDesignerQueries>();
        builder.RegisterType<EfDesignerRepository>().As<IDesignerRepository>();

        builder.RegisterType<EfTermsDocumentQueries>().As<ITermsDocumentQueries>();
        builder.RegisterType<EfTermsDocumentRepository>().As<ITermsDocumentRepository>();

        builder.RegisterType<EfLogoSetQueries>().As<ILogoSetQueries>();
        builder.RegisterType<EfLogoSetRepository>().As<ILogoSetRepository>();

        builder.RegisterType<EfSheetTypeQueries>().As<ISheetTypeQueries>();
        builder.RegisterType<EfSheetTypeRepository>().As<ISheetTypeRepository>();

        builder.RegisterType<EfProductFamilyQueries>().As<IProductFamilyQueries>();
        builder.RegisterType<EfProductFamilyRepository>().As<IProductFamilyRepository>();

        builder.RegisterType<EfProductKitReferenceQueries>().As<IProductKitReferenceQueries>();
        builder.RegisterType<EfProductKitReferenceRepository>().As<IProductKitReferenceRepository>();

        builder.RegisterType<EfComponentTypeQueries>().As<IComponentTypeQueries>();
        builder.RegisterType<EfComponentTypeRepository>().As<IComponentTypeRepository>();

        builder.RegisterType<EfOrganizationOptionsQueries>().As<IOrganizationOptionsQueries>();
        builder.RegisterType<EfOrganizationOptionsRepository>().As<IOrganizationOptionsRepository>();

        builder.RegisterType<EfProductRequirementQueries>().As<IProductRequirementQueries>();
        builder.RegisterType<EfProductRequirementRepository>().As<IProductRequirementRepository>();
    }

    private void RegisterBugsnag(ContainerBuilder builder)
    {
        var bugsnagSettings = _configurationLoader.GetSettings<BugsnagSettings>();
        var appVersionSettings = _configurationLoader.GetSettings<AppVersionSettings>();

        var config = BugsnagConfigurationFactory.Create(bugsnagSettings, appVersionSettings);
        var client = new Bugsnag.Client(config);
        builder.RegisterInstance<Bugsnag.IClient>(client);

        builder.RegisterType<BugsnagLogWriter>().As<ILogWriter>();
    }

    private void RegisterDomainEventPlumbling(ContainerBuilder builder)
    {
        if (_isAzure)
        {
            var connectionString = _configurationLoader.GetSettings<ConnectionStrings>().AzureWebJobsServiceBus;
            var serviceBusClient = new ServiceBusClient(connectionString);
            builder.RegisterInstance(serviceBusClient);

            builder.RegisterType<ServiceBusDomainEventPublisher>()
                .As<IDomainEventPublisher>();
        }
        else
        {
            builder.RegisterType<DirectDomainEventPublisherLifetimeScopeProvider>()
                .As<IDirectDomainEventPublisherLifetimeScopeProvider>()
                .SingleInstance();

            builder.RegisterBuildCallback(DirectDomainEventPublisherLifetimeScopeProvider.OnContainerBuilt);

            builder.RegisterType<DirectDomainEventPublisher>()
                .AsSelf()
                .As<IDomainEventPublisher>();
        }
    }

    private static void RegisterDomainEventHandlers(ContainerBuilder builder)
    {
        var registryBuilder = new DomainEventHandlerRegistryBuilder(builder);

        // If adding a new event registration, you also need to add a Service Bus
        // topic & subscription in the AzureResources project and an Azure Functions trigger to
        // listen on the subscription
        ImportEventHandler.Register(registryBuilder);
        ProjectEventHandler.Register(registryBuilder);
        ReportEventHandler.Register(registryBuilder);

        builder.RegisterInstance(registryBuilder.Build());
    }

    private void Settings<T>(ContainerBuilder builder)
        where T : class, new()
    {
        var instance = _configurationLoader.GetSettings<T>();
        builder.RegisterInstance(instance);
    }

    private void Settings<TInt, T>(ContainerBuilder builder)
        where T : class, TInt, new()
        where TInt : class
    {
        var instance = _configurationLoader.GetSettings<T>();
        builder.RegisterInstance<TInt>(instance);
    }
}
