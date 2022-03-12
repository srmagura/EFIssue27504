using DbEntities.ValueObjects;

namespace DbEntities;

public class DbImport : DbEntity
{
    public Guid OrganizationId { get; protected set; }
    public DbOrganization? Organization { get; protected set; }

    public Guid ProjectId { get; protected set; }
    public DbProject? Project { get; protected set; }

    public string Filename { get; protected set; }
    public DbFileRef File { get; protected set; }

    public DateTimeOffset? ProcessingStartUtc { get; protected set; }
    public DateTimeOffset? ProcessingEndUtc { get; protected set; }

    public string? ErrorMessage { get; protected set; }
}
