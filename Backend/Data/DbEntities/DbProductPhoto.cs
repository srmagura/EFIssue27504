using DbEntities.ValueObjects;

namespace DbEntities;

public class DbProductPhoto : DbEntity
{
#nullable disable
    [Obsolete("Entity Framework only.")]
    protected DbProductPhoto() { }
#nullable enable

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    [MaxLength(FieldLengths.ProductPhoto.Name)]
    public string Name { get; set; }

    public DbFileRef Photo { get; set; }

    public bool IsActive { get; set; } = true;

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbProductPhoto>()
            .HasIndex(p => new { p.OrganizationId, p.Name })
            .IsUnique();
    }
}
