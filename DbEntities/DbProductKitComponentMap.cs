namespace DbEntities
{
    public class DbProductKitComponentMap : DbEntity
    {
        public Guid OrganizationId { get; set; }
        public DbOrganization? Organization { get; set; }

        public Guid ComponentVersionId { get; set; }
        public DbComponentVersion? ComponentVersion { get; set; }

        public Guid ProductKitVersionId { get; set; }
        public DbProductKitVersion? ProductKitVersion { get; set; }

        public int Count { get; set; }

        public static void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<DbProductKitComponentMap>()
                .HasOne(p => p.ProductKitVersion)
                .WithMany(p => p.ComponentMaps);
        }
    }
}
