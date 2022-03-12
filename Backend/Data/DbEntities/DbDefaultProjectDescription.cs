namespace DbEntities;

public class DbDefaultProjectDescription : DbEntity
{
    public DbDefaultProjectDescription(string text)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public string Text { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbDefaultProjectDescription>()
            .HasIndex(p => p.OrganizationId)
            .IsUnique();
    }
}
