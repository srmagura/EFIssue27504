using DbEntities.ValueObjects;
using ValueObjects;

namespace DbEntities;

public class DbProject : DbEntity
{
#nullable disable
    [Obsolete("Entity Framework only.")]
    protected DbProject() { }
#nullable enable

    public DbProject(
        string name,
        string shortName,
        string description,
        PartialAddress address,
        string customerName,
        ProjectBudgetOptions budgetOptions,
        DbProjectReportOptions reportOptions
    )
    {
        Name = name;
        ShortName = shortName;
        Description = description;
        Address = address;
        CustomerName = customerName;
        BudgetOptions = budgetOptions;
        ReportOptions = reportOptions;
    }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    [MaxLength(FieldLengths.Project.Name)]
    public string Name { get; set; }

    [MaxLength(FieldLengths.Project.ShortName)]
    public string ShortName { get; set; }

    public string Description { get; set; }

    public PartialAddress Address { get; set; }

    [MaxLength(FieldLengths.Project.CustomerName)]
    public string CustomerName { get; set; }

    public int EstimatedSquareFeet { get; set; }

    public DbFileRef? Photo { get; set; }

    public ProjectBudgetOptions BudgetOptions { get; set; }
    public DbProjectReportOptions ReportOptions { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid? DesignerLockedById { get; set; }
    public DbUser? DesignerLockedBy { get; set; }

    public DateTimeOffset? DesignerLockedUtc { get; set; }

    public List<DbPageImport> Imports { get; set; } = new();
    public List<DbProjectPublication> ProjectPublications { get; set; } = new();
    public List<DbReport> Reports { get; set; } = new();
    public List<DbProductKitReference> ProductKitReferences { get; set; } = new();

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbProject>()
            .HasIndex(p => new { p.OrganizationId, p.Name })
            .IsUnique();

        mb.Entity<DbProject>()
            .HasMany(p => p.Imports)
            .WithOne(i => i.Project)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<DbProject>()
            .HasMany(p => p.ProjectPublications)
            .WithOne(i => i.Project)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<DbProject>()
            .HasMany(p => p.Reports)
            .WithOne(i => i.Project)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<DbProject>()
            .HasMany(p => p.ProductKitReferences)
            .WithOne(r => r.Project)
            .OnDelete(DeleteBehavior.Restrict);
    }
}