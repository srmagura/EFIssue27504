namespace DbEntities;

public class DbProductFamily : DbEntity
{
    public DbProductFamily(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    [MaxLength(FieldLengths.ProductFamily.Name)]
    public string Name { get; set; }

    public bool IsActive { get; set; } = true;

    public List<DbProductKit> ProductKits { get; set; } = new();

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbProductFamily>()
            .HasIndex(p => new { p.OrganizationId, p.Name })
            .IsUnique();
    }
}
