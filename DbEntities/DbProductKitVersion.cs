using ValueObjects;

namespace DbEntities
{
    public class DbProductKitVersion : DbEntity
    {
#nullable disable
        [Obsolete("EF cannot map navigation properties in constructor", true)]
        protected DbProductKitVersion()
        {
        }
#nullable enable

        public Guid OrganizationId { get; set; }
        public DbOrganization? Organization { get; set; }

        public Guid ProductKitId { get; set; }
        public DbProductKit? ProductKit { get; set; }

        [MaxLength(FieldLengths.ProductKitVersion.Name)]
        public string Name { get; set; }

        public string Description { get; set; }

        [MaxLength(FieldLengths.ProductKitVersion.VersionName)]
        public string VersionName { get; set; }

        public Money SellPrice { get; set; }

        public Guid SymbolId { get; set; }
        public DbSymbol? Symbol { get; set; }

        public Guid? ProductPhotoId { get; set; }
        public DbProductPhoto? ProductPhoto { get; set; }

        public Guid MainComponentVersionId { get; set; }
        public DbComponentVersion? MainComponentVersion { get; set; }

        public List<DbProductKitComponentMap> ComponentMaps { get; set; } = new List<DbProductKitComponentMap>();
        public List<DbProductKitReference> References { get; set; } = new List<DbProductKitReference>();

        public static void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<DbProductKitVersion>()
                .HasIndex(v => new { v.ProductKitId, v.VersionName })
                .IsUnique();

            mb.Entity<DbProductKitVersion>()
                .HasMany(v => v.References)
                .WithOne(r => r.ProductKitVersion)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
