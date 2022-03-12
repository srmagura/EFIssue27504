using System.Reflection;
using AppConfig;
using AppDTOs;
using AppDTOs.Enumerations;
using AppInterfaces;
using Autofac;
using AutoMapper;
using DataContext;
using DataInterfaces.Repositories;
using DbEntities;
using Entities;
using Enumerations;
using FileStore;
using Identities;
using InfraInterfaces;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Application.DomainEvents.Direct;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Infrastructure.DataMapping;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Settings;
using ValueObjects;

namespace TestUtilities
{
    public abstract class IntegrationTest
    {
#nullable disable // so we don't have to null check it in every test
        protected IContainer Container { get; set; }
#nullable enable

#nullable disable // so we don't have to null check it in every test
        protected OrganizationId HostOrganizationId { get; private set; }
        protected UserDto AutomationUser { get; private set; }
#nullable enable

        protected static void AddRegistrations(ContainerBuilder builder)
        {
            builder.RegisterModule(new AppModule(null));

            var connectionStrings = GetConnectionStrings();
            builder.RegisterInstance(connectionStrings);
            builder.RegisterInstance<IDbLoggerSettings>(connectionStrings);

            builder.RegisterType<TestAuthContext>().As<IAppAuthContext>().As<IAuthContext>();
            builder.RegisterType<TestScopedOrganizationContext>().As<IOrganizationContext>();

            builder.RegisterType<ConsoleLogWriter>().As<ILogWriter>();
            builder.RegisterType<InMemoryFileStore>().As<IFileStore>();
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            InMemoryFileStore.Clear();
            ConsoleLogWriter.ClearErrors();
            DirectDomainEventPublisher.ShouldWaitForHandlersToComplete(true);

            var builder = new ContainerBuilder();
            AddRegistrations(builder);
            Container = builder.Build();

            var connectionString = GetConnectionStrings().AppDataContext;
            await DeleteFromTablesUtil.DeleteFromTablesAsync(connectionString);

            //
            // SET UP AS A HOST USER (INITIALLY)
            //
            var (hostOrganizationId, hostUserId) = await AddHostDataAsync();
            HostOrganizationId = hostOrganizationId;

            using var db = Container.Resolve<AppDataContext>();
            var mapper = Container.Resolve<IMapper>();

            var automationUser = await db.Users
                .Where(u => u.Id == hostUserId.Guid)
                .ProjectToDtoAsync<DbUser, UserDto>(mapper);

            if (automationUser == null)
                throw new Exception("Automation user is null.");

            AutomationUser = automationUser;
            TestAuthContext.SetUser(automationUser);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(ConsoleLogWriter.HasErrors);
            Container.Dispose();
        }

        private async Task<(OrganizationId HostOrganizationId, UserId AutomationUserId)> AddHostDataAsync()
        {
            var db = Container.Resolve<AppDataContext>();

            var hostOrganization = new DbOrganization("System 7", new OrganizationShortName("system7"))
            {
                IsHost = true,
            };
            db.Organizations.Add(hostOrganization);

            var automationUser = new DbUser(
                new EmailAddress("automationUser@iticentral.com"),
                new PersonName("Automation", "User")
            )
            {
                OrganizationId = hostOrganization.Id,
                Role = UserRole.SystemAdmin
            };

            db.Users.Add(automationUser);
            await db.SaveChangesAsync();

            return (new OrganizationId(hostOrganization.Id), new UserId(automationUser.Id));
        }

        public static ConnectionStrings GetConnectionStrings()
        {
            var connectionString = Environment.GetEnvironmentVariable("CIDATABASECONNECTIONSTRING");

            if (connectionString == null)
            {
                connectionString = new ConnectionStrings().AppDataContext.Replace("Slateplan", "Slateplan_e2e_test");
            }

            return new ConnectionStrings
            {
                AppDataContext = connectionString
            };
        }

        protected async Task<OrganizationId> AddOrganizationAsync(int i = 0)
        {
            var svc = Container.Resolve<IOrganizationAppService>();

            var organizationId = await svc.AddAsync(
                $"Test Organization {i}",
                $"TestOrg{i}",
                ownerEmail: new EmailAddressDto($"owner{i}@example2.com"),
                ownerName: new PersonNameDto("Owner", "Todd"),
                "Password1234!"
            );

            return organizationId;
        }

        protected async Task<ProjectId> AddProjectAsync(
            OrganizationId organizationId,
            LogoSetId? logoSetId = null,
            TermsDocumentId? termsDocumentId = null
        )
        {
            var projectSvc = Container.Resolve<IProjectAppService>();

            if (logoSetId == null)
                logoSetId = await AddLogoSetAsync(organizationId);

            if (termsDocumentId == null)
                termsDocumentId = await AddTermsDocumentAsync(organizationId);

            return await projectSvc.AddAsync(
                organizationId,
                name: "myProject",
                shortName: "myProj",
                description: "description",
                address: new PartialAddressDto("Line 1", null, null, null, null),
                customerName: "customerName",
                signeeName: "signeeName",
                logoSetId: logoSetId,
                termsDocumentId: termsDocumentId,
                estimatedSquareFeet: 1000
            );
        }

        protected async Task AcquireDesignerLockAsync(ProjectId projectId)
        {
            var projectSvc = Container.Resolve<IProjectAppService>();

            await projectSvc.AcquireDesignerLockAsync(projectId);
        }

        protected async Task<ProjectPublicationId> AddProjectPublicationAsync(ProjectId projectId)
        {
            var svc = Container.Resolve<IProjectPublicationAppService>();

            await svc.PublishAsync(projectId);
            return (await svc.ListAsync(projectId)).First().Id;
        }

        protected const string DefaultPassword = "Password1234!";

        protected Task<UserId> AddUserAsync(OrganizationId organizationId, UserRole role)
        {
            var userSvc = Container.Resolve<IUserAppService>();

            return userSvc.AddAsync(
                organizationId,
                new EmailAddressDto($"tester{Guid.NewGuid()}@iticentral.com"),
                new PersonNameDto("Test", "Tester"),
                role,
                DefaultPassword
            );
        }

        protected async Task<UserDto> GetUserAsync(UserId userId)
        {
            var userSvc = Container.Resolve<IUserAppService>();

            var user = await userSvc.GetAsync(userId);
            Assert.IsNotNull(user);

            return user;
        }

        protected async Task<PageId> AddPageAsync(OrganizationId organizationId, ProjectId projectId, int index)
        {
            var uowp = Container.Resolve<IUnitOfWorkProvider>();
            var pageRepo = Container.Resolve<IPageRepository>();

            using var uow = uowp.Begin();

            var pdf = new FileRef(new FileId(), "application/pdf");
            var thumbnail = new FileRef(new FileId(), "image/jpeg");
            var page = new Page(organizationId, projectId, pdf, thumbnail, index);

            pageRepo.Add(page);

            await uow.CommitAsync();

            return page.Id;
        }

        protected async Task ImportAsync(ProjectId projectId, string filename)
        {
            var importSvc = Container.Resolve<IImportAppService>();

            using var stream = GetResourceStream(filename);
            var importId = await importSvc.ImportAsync(projectId, filename, stream);

            var import = (await importSvc.ListAsync(projectId, skip: 0, take: 1)).Single();
            Assert.AreEqual(importId, import.Id);
            Assert.AreEqual(ImportStatus.Completed, import.Status);
            Assert.AreEqual(1m, import.PercentComplete);
            Assert.AreEqual(0, InMemoryFileStore.ListFiles(FilePathBuilder.ImportsPath).Count);
        }

        protected async Task<ProductPhotoId> AddProductPhotoAsync(OrganizationId organizationId)
        {
            var productPhotoSvc = Container.Resolve<IProductPhotoAppService>();

            ProductPhotoId productPhotoId;
            using (var stream = GetResourceStream("frog.jpg"))
            {
                productPhotoId = await productPhotoSvc.AddAsync(
                    organizationId,
                    name: $"productPhoto{Guid.NewGuid()}",
                    stream,
                    "image/jpeg"
                );
            }

            return productPhotoId;
        }

        protected async Task<SymbolId> AddSymbolAsync(OrganizationId organizationId)
        {
            var symbolSvc = Container.Resolve<ISymbolAppService>();

            return await symbolSvc.AddAsync(organizationId, $"symbol{Guid.NewGuid()}", "<svg><circle cx=\"50\" cy=\"50\" r=\"40\" /></svg>");
        }

        protected async Task<ComponentId> AddComponentAsync(OrganizationId organizationId, MeasurementType measurementType = MeasurementType.Normal)
        {
            var componentSvc = Container.Resolve<IComponentAppService>();

            var componentTypeId = await AddComponentTypeAsync(organizationId);

            return await componentSvc.AddAsync(
                organizationId: organizationId,
                componentTypeId: componentTypeId,
                measurementType: measurementType,
                isVideoDisplay: false,
                visibleToCustomer: true,
                displayName: $"component{Guid.NewGuid()}",
                versionName: "1",
                sellPrice: 10m,
                url: null,
                make: "make",
                model: Guid.NewGuid().ToString(),
                vendorPartNumber: Guid.NewGuid().ToString(),
                organizationPartNumber: null,
                whereToBuy: null,
                style: null,
                color: null,
                internalNotes: null
            );
        }

        protected async Task<CategoryId> AddCategoryAsync(OrganizationId organizationId, SymbolId symbolId)
        {
            var categorySvc = Container.Resolve<ICategoryAppService>();

            var tree = new TreeInputDto();
            tree.Children.Add(new TreeInputDto());

            tree.Children[0].Category = new CategoryInputDto(new CategoryId(), "HD TVs")
            {
                SymbolId = symbolId,
                Color = "#000000",
                IsActive = true
            };

            await categorySvc.SetCategoryTreeAsync(organizationId, tree);
            var actualTree = await categorySvc.GetCategoryTreeAsync(organizationId, ActiveFilter.ActiveOnly);

            return actualTree.Children[0].Category!.Id;
        }

        protected async Task<ProductKitId> AddProductKitAsync(OrganizationId organizationId, CategoryId categoryId, SymbolId symbolId, ComponentVersionId componentVersionId)
        {
            var productKitSvc = Container.Resolve<IProductKitAppService>();

            var componentMaps = new List<ProductKitComponentMapInputDto>()
            {
                new ProductKitComponentMapInputDto(null, componentVersionId, 1)
            };

            return await productKitSvc.AddAsync(
                organizationId: organizationId,
                categoryId: categoryId,
                name: $"productKit{Guid.NewGuid()}",
                description: "description",
                versionName: "0",
                symbolId: symbolId,
                productPhotoId: null,
                mainComponentVersionId: componentVersionId,
                componentMaps,
                productFamilyId: null
            );
        }

        protected async Task<LogoSetId> AddLogoSetAsync(OrganizationId organizationId)
        {
            var logoSetSvc = Container.Resolve<ILogoSetAppService>();

            using var darkStream = GetResourceStream("system7-dark.svg");
            using var lightStream = GetResourceStream("system7-light.svg");

            return await logoSetSvc.AddAsync(
                organizationId,
                name: $"logoSet{Guid.NewGuid()}",
                darkStream,
                "image/svg",
                lightStream,
                "image/svg"
            );
        }

        protected async Task<SheetTypeId> AddSheetTypeAsync(OrganizationId organizationId)
        {
            var sheetTypeSvc = Container.Resolve<ISheetTypeAppService>();

            return await sheetTypeSvc.AddAsync(organizationId, "T1.0", "Technology Design — AV");
        }

        protected async Task<TermsDocumentId> AddTermsDocumentAsync(OrganizationId organizationId)
        {
            var termsDocSvc = Container.Resolve<ITermsDocumentAppService>();

            using var stream = GetResourceStream("billing-rates.pdf");

            return await termsDocSvc.AddAsync(organizationId, stream);
        }

        protected async Task<ComponentTypeId> AddComponentTypeAsync(OrganizationId organizationId)
        {
            var componentTypeSvc = Container.Resolve<IComponentTypeAppService>();

            var componentTypes = await componentTypeSvc.ListAsync(organizationId, ActiveFilter.ActiveOnly);

            if (componentTypes.Any())
                return componentTypes.First().Id;

            return await componentTypeSvc.AddAsync(organizationId, "Component type 1");
        }

        public static Stream GetResourceStream(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = $"{assembly.GetName().Name}.Resources.{filename}";

            return assembly.GetManifestResourceStream(resourcePath)
                ?? throw new Exception($"Could not load resource: {filename}.");
        }
    }
}
