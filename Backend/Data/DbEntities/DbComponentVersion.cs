using ValueObjects;

namespace DbEntities
{
    public class DbComponentVersion : DbEntity
    {
#nullable disable
        [Obsolete("EF cannot map navigation properties in constructor", true)]
        protected DbComponentVersion()
        {
        }
#nullable enable

        public Guid ComponentId { get; set; }
        public DbComponent? Component { get; set; }

        public Guid OrganizationId { get; set; }
        public DbOrganization? Organization { get; set; }

        [MaxLength(FieldLengths.ComponentVersion.DisplayName)]
        public string DisplayName { get; set; }
        [MaxLength(FieldLengths.ComponentVersion.VersionName)]
        public string VersionName { get; set; }

        public Money SellPrice { get; set; }
        public Url? Url { get; set; }

        [MaxLength(FieldLengths.ComponentVersion.Make)]
        public string Make { get; set; }
        [MaxLength(FieldLengths.ComponentVersion.Model)]
        public string Model { get; set; }
        [MaxLength(FieldLengths.ComponentVersion.VendorPartNumber)]
        public string VendorPartNumber { get; set; }
        [MaxLength(FieldLengths.ComponentVersion.OrganizationPartNumber)]
        public string? OrganizationPartNumber { get; set; }
        [MaxLength(FieldLengths.ComponentVersion.WhereToBuy)]
        public string? WhereToBuy { get; set; }
        [MaxLength(FieldLengths.ComponentVersion.Style)]
        public string? Style { get; set; }
        [MaxLength(FieldLengths.ComponentVersion.Color)]
        public string? Color { get; set; }
        public string? InternalNotes { get; set; }

        public static void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<DbComponentVersion>()
                .HasIndex(p => new { p.ComponentId, p.VersionName })
                .IsUnique();
        }
    }
}
