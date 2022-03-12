using DbEntities.ValueObjects;

namespace DbEntities;

public class DbLogoSet : DbEntity
{
#nullable disable
    [Obsolete("Entity Framework only.")]
    protected DbLogoSet() { }
#nullable enable

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    [MaxLength(FieldLengths.LogoSet.Name)]
    public string Name { get; set; }

    public DbFileRef DarkLogo { get; set; }
    public DbFileRef LightLogo { get; set; }

    public bool IsActive { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbLogoSet>()
            .HasIndex(p => new { p.OrganizationId, p.Name })
            .IsUnique();
    }
}
