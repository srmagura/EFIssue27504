namespace DbEntities
{
    public class DbProductKit : DbEntity
    {
        public DbProductKit(Guid organizationId, Guid categoryId, bool isActive)
        {
            OrganizationId = organizationId;
            CategoryId = categoryId;
            IsActive = isActive;
        }

        public Guid OrganizationId { get; set; }
        public DbOrganization? Organization { get; set; }

        public Guid CategoryId { get; set; }
        public DbCategory? Category { get; set; }

        // This is denormalized, since the ProductKit measurement type is determined
        // from its components, but there is no harm since the MeasurementType
        // cannot change after the ProductKit is created
        public MeasurementType MeasurementType { get; set; }

        public bool IsActive { get; set; }

        public Guid? ProductFamilyId { get; set; }
        public DbProductFamily? ProductFamily { get; set; }

        public List<DbProductKitVersion> Versions { get; set; } = new List<DbProductKitVersion>();

        public static void OnModelCreating(ModelBuilder mb)
        {
            // For convention
        }
    }
}
