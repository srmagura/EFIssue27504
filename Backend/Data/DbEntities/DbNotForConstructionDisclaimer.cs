namespace DbEntities;

public class DbNotForConstructionDisclaimer : DbEntity
{
    public DbNotForConstructionDisclaimer(string text)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public string Text { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbNotForConstructionDisclaimer>()
            .HasIndex(p => p.OrganizationId)
            .IsUnique();
    }
}
