namespace Entities;

public class ProductKitReference : AggregateRoot
{
    [Obsolete("AutoMapper only.")]
    public ProductKitReference(
        OrganizationId organizationId,
        ProjectId projectId,
        ProductKitVersionId productKitVersionId
    )
    {
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        ProductKitVersionId = productKitVersionId ?? throw new ArgumentNullException(nameof(productKitVersionId));
    }

    // ProductKitReferences are created via the DB entity for performance which is
    // why there is no non-obsolete constructor

    public ProductKitReferenceId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }
    public ProjectId ProjectId { get; protected set; }

    public ProductKitVersionId ProductKitVersionId { get; protected set; }

    public string? Tag { get; protected set; }

    public void SetProductKitVersion(ProductKitVersion productKitVersion)
    {
        Require.NotNull(productKitVersion, "Product kit version is required.");
        Require.IsTrue(OrganizationId == productKitVersion.OrganizationId, "Organization ID mismatch.");

        ProductKitVersionId = productKitVersion.Id;
    }

    // This method doesn't check for organization ID mismatch and exists to support
    // more efficient/convenient bulk updates
    public void SetProductKitVersionId(ProductKitVersionId productKitVersionId)
    {
        Require.NotNull(productKitVersionId, "Product kit version ID is required.");

        ProductKitVersionId = productKitVersionId;
    }

    public void SetTag(string? tag)
    {
        Tag = tag;
    }
}
