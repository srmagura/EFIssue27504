using DbEntities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataContext
{
    public class AppDataContext : DbContext, IDataProtectionKeyContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Windows authentication. If you are using Linux, need to tweak this
            var connectionString = "Server=localhost;Database=Slateplan;TrustServerCertificate=True;MultipleActiveResultSets=True;Trusted_Connection=True;";

            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbUser.OnModelCreating(modelBuilder);

            DbOrganization.OnModelCreating(modelBuilder);

            DbProject.OnModelCreating(modelBuilder);

            DbProductPhoto.OnModelCreating(modelBuilder);

            DbCategory.OnModelCreating(modelBuilder);

            DbImport.OnModelCreating(modelBuilder);

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
        public DbSet<DbImport> Imports => Set<DbImport>();
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

        public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    }
}
