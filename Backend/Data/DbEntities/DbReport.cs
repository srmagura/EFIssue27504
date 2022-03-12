using DbEntities.ValueObjects;
using ValueObjects;

namespace DbEntities;

public class DbReport : DbEntity
{
    public Guid OrganizationId { get; protected set; }
    public DbOrganization? Organization { get; protected set; }

    public Guid ProjectId { get; protected set; }
    public DbProject? Project { get; protected set; }

    public Guid? ProjectPublicationId { get; protected set; }
    public DbProjectPublication? ProjectPublication { get; protected set; }

    public ReportType Type { get; protected set; }

    public DbFileRef? File { get; protected set; }
    public string? Filename { get; protected set; }

    public DateTimeOffset? ProcessingStartUtc { get; protected set; }
    public DateTimeOffset? ProcessingEndUtc { get; protected set; }

    public Percentage PercentComplete { get; protected set; } = new Percentage(0);

    public ReportStatus Status { get; protected set; }
    public string? ErrorMessage { get; protected set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        // For convention
    }
}
