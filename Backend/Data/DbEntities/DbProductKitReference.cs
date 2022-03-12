namespace DbEntities;

public class DbProductKitReference : DbEntity
{
    public DbProductKitReference(Guid organizationId, Guid projectId, Guid productKitVersionId)
    {
        OrganizationId = organizationId;
        ProjectId = projectId;
        ProductKitVersionId = productKitVersionId;
    }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public Guid ProjectId { get; set; }
    public DbProject? Project { get; set; }

    public Guid ProductKitVersionId { get; set; }
    public DbProductKitVersion? ProductKitVersion { get; set; }

    [MaxLength(FieldLengths.ProductKitReference.Tag)]
    public string? Tag { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        // For convention
    }
}
