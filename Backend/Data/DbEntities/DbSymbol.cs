namespace DbEntities;

public class DbSymbol : DbEntity
{
    public DbSymbol(Guid organizationId, string name, string svgText)
    {
        OrganizationId = organizationId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        SvgText = svgText ?? throw new ArgumentNullException(nameof(svgText));
    }

    public bool IsActive { get; set; } = true;

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    [MaxLength(FieldLengths.Symbol.Name)]
    public string Name { get; set; }

    public string SvgText { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbSymbol>()
            .HasIndex(p => new { p.OrganizationId, p.Name })
            .IsUnique();
    }
}
