using DbEntities.ValueObjects;

namespace DbEntities;

public class DbPage : DbEntity
{
    public DbFileRef Pdf { get; set; }
    public DbFileRef Thumbnail { get; set; }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public Guid ProjectId { get; set; }
    public DbProject? Project { get; set; }

    public int Index { get; set; }

    [MaxLength(8)]
    public string? SheetNumberSuffix { get; set; }

    [MaxLength(32)]
    public string? SheetNameSuffix { get; set; }

    public bool IsActive { get; set; } = true;

    public static void OnModelCreating(ModelBuilder mb)
    {
    }
}
