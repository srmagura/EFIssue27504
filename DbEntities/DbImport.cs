using DbEntities.ValueObjects;
using ValueObjects;

namespace DbEntities;

public class DbImport : DbEntity
{
#nullable disable
    [Obsolete("For use by Entity Framework only.")]
    protected DbImport() { }
#nullable enable

    public Guid OrganizationId { get; protected set; }
    public DbOrganization? Organization { get; protected set; }

    public Guid ProjectId { get; protected set; }
    public DbProject? Project { get; protected set; }

    public string Filename { get; protected set; }
    public DbFileRef File { get; protected set; }

    public ImportStatus Status { get; protected set; }

    public DateTimeOffset? ProcessingStartUtc { get; protected set; }
    public DateTimeOffset? ProcessingEndUtc { get; protected set; }

    public Percentage PercentComplete { get; protected set; } = new Percentage(0);

    public string? ErrorMessage { get; protected set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        // For convention
    }
}
