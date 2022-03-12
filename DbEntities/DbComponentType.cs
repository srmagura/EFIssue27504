namespace DbEntities;

public class DbComponentType : DbEntity
{
    public DbComponentType(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    [MaxLength(FieldLengths.ComponentType.Name)]
    public string Name { get; set; }

    public bool IsActive { get; set; } = true;

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbComponentType>()
            .HasIndex(p => new { p.OrganizationId, p.Name })
            .IsUnique();
    }
}
