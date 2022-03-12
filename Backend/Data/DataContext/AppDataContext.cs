using AutoMapper;
using DbEntities;
using InfraInterfaces;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Logging;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Settings;

namespace DataContext
{
    public class AppDataContext : BaseDataContext, IDataProtectionKeyContext
    {
        private readonly string _connectionString;
        private readonly IAppAuthContext _appAuthContext;
        private readonly IOrganizationContext _organizationContext;

#nullable disable
        [Obsolete("Only for use by Package Manager Console and dotnet-ef commands.", false)]
        public AppDataContext()
        {
            _connectionString = new ConnectionStrings().AppDataContext;
        }

        // For MigrateForDevelopment only
        private AppDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }
#nullable enable

        public AppDataContext(
            ConnectionStrings connectionStrings,
            IMapper mapper,
            IAppAuthContext appAuthContext,
            IOrganizationContext organizationContext
        ) : base(mapper, auditor: null)
        {
            _connectionString = connectionStrings.AppDataContext;
            _appAuthContext = appAuthContext;
            _organizationContext = organizationContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);

            if (_appAuthContext != null && _organizationContext != null)
            {
                optionsBuilder.AddInterceptors(new RowLevelSecurityInterceptor(this, _appAuthContext, _organizationContext));
            }
        }

        public static void MigrateForDevelopment(string connectionString)
        {
            using var db = new AppDataContext(connectionString);
            db.Database.SetCommandTimeout(600);
            db.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            LogEntry.OnModelCreating(modelBuilder);

            DbUser.OnModelCreating(modelBuilder);

            DbOrganization.OnModelCreating(modelBuilder);

            DbProject.OnModelCreating(modelBuilder);

            DbProductPhoto.OnModelCreating(modelBuilder);

            DbCategory.OnModelCreating(modelBuilder);

            DbPageImport.OnModelCreating(modelBuilder);

            DbProjectPublication.OnModelCreating(modelBuilder);

            DbReport.OnModelCreating(modelBuilder);

            DbPage.OnModelCreating(modelBuilder);

            DbSymbol.OnModelCreating(modelBuilder);

            DbComponent.OnModelCreating(modelBuilder);

            DbComponentVersion.OnModelCreating(modelBuilder);

            DbComponentType.OnModelCreating(modelBuilder);

            DbProductKit.OnModelCreating(modelBuilder);

            DbProductKitVersion.OnModelCreating(modelBuilder);

            DbProductKitComponentMap.OnModelCreating(modelBuilder);

            DbProductKitReference.OnModelCreating(modelBuilder);

            DbDesignerData.OnModelCreating(modelBuilder);

            DbTermsDocument.OnModelCreating(modelBuilder);

            DbLogoSet.OnModelCreating(modelBuilder);

            DbSheetType.OnModelCreating(modelBuilder);

            DbProductFamily.OnModelCreating(modelBuilder);

            DbDefaultProjectDescription.OnModelCreating(modelBuilder);

            DbNotForConstructionDisclaimer.OnModelCreating(modelBuilder);

            DbProductRequirement.OnModelCreating(modelBuilder);
        }

        public DbSet<DbOrganization> Organizations => Set<DbOrganization>();
        public DbSet<DbUser> Users => Set<DbUser>();

        public DbSet<DbProject> Projects => Set<DbProject>();
        public DbSet<DbPageImport> PageImports => Set<DbPageImport>();
        public DbSet<DbProjectPublication> ProjectPublications => Set<DbProjectPublication>();
        public DbSet<DbReport> Reports => Set<DbReport>();
        public DbSet<DbPage> Pages => Set<DbPage>();
        public DbSet<DbDesignerData> DesignerData => Set<DbDesignerData>();

        public DbSet<DbCategory> Categories => Set<DbCategory>();

        public DbSet<DbProductPhoto> ProductPhotos => Set<DbProductPhoto>();
        public DbSet<DbSymbol> Symbols => Set<DbSymbol>();

        public DbSet<DbComponent> Components => Set<DbComponent>();
        public DbSet<DbComponentVersion> ComponentVersions => Set<DbComponentVersion>();
        public DbSet<DbComponentType> ComponentTypes => Set<DbComponentType>();

        public DbSet<DbProductKit> ProductKits => Set<DbProductKit>();
        public DbSet<DbProductKitVersion> ProductKitVersions => Set<DbProductKitVersion>();
        public DbSet<DbProductKitComponentMap> ProductKitComponentMaps => Set<DbProductKitComponentMap>();
        public DbSet<DbProductKitReference> ProductKitReferences => Set<DbProductKitReference>();

        public DbSet<DbTermsDocument> TermsDocuments => Set<DbTermsDocument>();

        public DbSet<DbLogoSet> LogoSets => Set<DbLogoSet>();
        public DbSet<DbSheetType> SheetTypes => Set<DbSheetType>();

        public DbSet<DbProductFamily> ProductFamilies => Set<DbProductFamily>();

        public DbSet<DbDefaultProjectDescription> DefaultProjectDescriptions => Set<DbDefaultProjectDescription>();
        public DbSet<DbNotForConstructionDisclaimer> NotForConstructionDisclaimers => Set<DbNotForConstructionDisclaimer>();

        public DbSet<DbProductRequirement> ProductRequirements => Set<DbProductRequirement>();

        public DbSet<LogEntry> LogEntries => Set<LogEntry>();

        public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    }
}
