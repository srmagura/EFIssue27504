namespace DbEntities;

public class DbProjectPublication : DbEntity
{
    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public Guid ProjectId { get; set; }
    public DbProject? Project { get; set; }

    public Guid PublishedById { get; set; }
    public DbUser? PublishedBy { get; set; }

    public int RevisionNumber { get; set; }

    public bool ReportsSentToCustomer { get; set; }

    public List<DbReport> Reports { get; set; } = new();

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbProjectPublication>()
            .HasIndex(p => new { p.ProjectId, p.RevisionNumber })
            .IsUnique();
    }
}
