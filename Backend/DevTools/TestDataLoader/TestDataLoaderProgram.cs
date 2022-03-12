using System.Diagnostics;
using System.Reflection;
using AppConfig;
using Autofac;
using FileStore;
using InfraInterfaces;
using ITI.DDD.Application.DomainEvents.Direct;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using Settings;
using TestDataLoader.Loaders;
using TestUtilities;

namespace TestDataLoader;

internal class TestDataLoaderProgram
{
    private readonly TestDataMonitor _monitor;
    private readonly ConnectionStrings _connectionStrings;
    private readonly LocalFileStore _localFileStore;

    private readonly HostOrganizationLoader _hostOrganizationLoader;
    private readonly OrganizationLoader _organizationLoader;
    private readonly ProjectLoader _projectLoader;
    private readonly UserLoader _userLoader;
    private readonly SymbolLoader _symbolLoader;
    private readonly ProductPhotoLoader _productPhotoLoader;
    private readonly PageLoader _pageLoader;
    private readonly CategoryLoader _categoryLoader;
    private readonly ComponentLoader _componentLoader;
    private readonly ProductKitLoader _productKitLoader;
    private readonly DesignerLoader _designerLoader;
    private readonly TermsDocumentLoader _termsDocumentLoader;
    private readonly LogoSetLoader _logoSetLoader;
    private readonly SheetTypeLoader _sheetTypeLoader;
    private readonly ProjectPublicationLoader _projectPublicationLoader;
    private readonly ProductFamilyLoader _productFamilyLoader;
    private readonly ComponentTypeLoader _componentTypeLoader;
    private readonly OrganizationOptionsLoader _organizationOptionsLoader;
    private readonly ProductRequirementLoader _productRequirementLoader;

    public TestDataLoaderProgram(
        TestDataMonitor monitor,
        ConnectionStrings connectionStrings,
        LocalFileStore localFileStore,
        HostOrganizationLoader hostOrganizationLoader,
        OrganizationLoader organizationLoader,
        ProjectLoader projectLoader,
        UserLoader userLoader,
        CategoryLoader categoryLoader,
        SymbolLoader symbolLoader,
        PageLoader pageLoader,
        ProductPhotoLoader productPhotoLoader,
        ComponentLoader componentLoader,
        ProductKitLoader productKitLoader,
        DesignerLoader designerLoader,
        TermsDocumentLoader termsDocumentLoader,
        LogoSetLoader logoSetLoader,
        SheetTypeLoader sheetTypeLoader,
        ProjectPublicationLoader projectPublicationLoader,
        ProductFamilyLoader productFamilyLoader,
        ComponentTypeLoader componentTypeLoader,
        OrganizationOptionsLoader organizationOptionsLoader,
        ProductRequirementLoader productRequirementLoader
    )
    {
        _monitor = monitor;
        _connectionStrings = connectionStrings;

        _localFileStore = localFileStore;
        _hostOrganizationLoader = hostOrganizationLoader;
        _organizationLoader = organizationLoader;
        _projectLoader = projectLoader;
        _userLoader = userLoader;
        _symbolLoader = symbolLoader;
        _productPhotoLoader = productPhotoLoader;
        _pageLoader = pageLoader;
        _categoryLoader = categoryLoader;
        _componentLoader = componentLoader;
        _productKitLoader = productKitLoader;
        _designerLoader = designerLoader;
        _termsDocumentLoader = termsDocumentLoader;
        _logoSetLoader = logoSetLoader;
        _sheetTypeLoader = sheetTypeLoader;
        _projectPublicationLoader = projectPublicationLoader;
        _productFamilyLoader = productFamilyLoader;
        _componentTypeLoader = componentTypeLoader;
        _organizationOptionsLoader = organizationOptionsLoader;
        _productRequirementLoader = productRequirementLoader;
    }

    private void BailIfAzureSql()
    {
        if (StoredProcedureLoader.IsAzureSql(_connectionStrings.AppDataContext))
        {
            Console.WriteLine("[error] Cowardly refusing to run against an Azure SQL database.");
            Environment.Exit(1);
        }
    }

    public async Task ResetDbAsync()
    {
        BailIfAzureSql();

        AppDataContext.MigrateForDevelopment(_connectionStrings.AppDataContext);
        _monitor.WriteCompletedMessage("Applied migrations.");

        _localFileStore.DeleteAllFiles();
        _monitor.WriteCompletedMessage("Cleared local file storage.");

        await DeleteFromTablesUtil.DeleteFromTablesAsync(_connectionStrings.AppDataContext);
        _monitor.WriteCompletedMessage("Deleted data from all tables.");

        await _hostOrganizationLoader.AddHostOrganizationAsync();
        await _organizationLoader.AddOrganizationsAsync();
        await _logoSetLoader.AddLogoSetsAsync();
        await _termsDocumentLoader.AddTermsDocumentsAsync();
        await _projectLoader.AddProjectsAsync();
        await _userLoader.AddUsersAsync();
        await _symbolLoader.AddSymbolsAsync();
        await _productPhotoLoader.AddProductPhotosAsync();
        await _sheetTypeLoader.AddSheetTypesAsync();
        await _pageLoader.AddPagesAsync();
        await _categoryLoader.AddCategoriesAsync();
        await _componentTypeLoader.AddComponentTypesAsync();
        await _componentLoader.AddComponentsAsync();
        await _productFamilyLoader.AddProductFamiliesAsync();
        await _productKitLoader.AddProductKitsAsync();
        await _designerLoader.AddDesignerDataAsync();
        await _projectPublicationLoader.AddProjectPublicationsAsync();
        await _organizationOptionsLoader.AddOrganizationOptionsAsync();
        await _productRequirementLoader.AddProductRequirementsAsync();
    }

    public static async Task Main()
    {
        var stopwatch = Stopwatch.StartNew();

        DirectDomainEventPublisher.ShouldWaitForHandlersToComplete(true);

        var builder = new ContainerBuilder();
        builder.RegisterModule(new AppModule(null));

        var connectionStrings = new ConnectionStrings();
        builder.RegisterInstance(connectionStrings);
        builder.RegisterInstance<IDbLoggerSettings>(connectionStrings);

        builder.RegisterType<TestDataLoaderAuthContext>()
            .As<IAuthContext>()
            .As<IAppAuthContext>();
        builder.RegisterType<SystemOrganizationContext>().As<IOrganizationContext>();
        builder.RegisterType<ConsoleLogWriter>().As<ILogWriter>();

        builder.RegisterType<LocalFileStoreSettings>();
        builder.RegisterType<LocalFileStore>();

        builder.RegisterType<TestDataLoaderProgram>();
        builder.RegisterInstance(new TestDataMonitor());

        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .InNamespaceOf<CategoryLoader>();

        var container = builder.Build();
        await container.Resolve<TestDataLoaderProgram>().ResetDbAsync();

        Console.WriteLine($"Operation complete. It took {stopwatch.ElapsedMilliseconds} ms.");
    }
}
