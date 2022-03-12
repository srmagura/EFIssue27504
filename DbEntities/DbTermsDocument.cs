using DbEntities.ValueObjects;

namespace DbEntities;

public class DbTermsDocument : DbEntity
{
#nullable disable
    [Obsolete("Entity Framework only.")]
    protected DbTermsDocument() { }
#nullable enable

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public int Number { get; set; }

    public DbFileRef File { get; set; }

    public bool IsActive { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbTermsDocument>()
            .HasIndex(p => new { p.OrganizationId, p.Number })
            .IsUnique();
    }
}
