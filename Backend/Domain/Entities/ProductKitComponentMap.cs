namespace Entities;

public class ProductKitComponentMap : Member<ProductKit>
{
    [Obsolete("AutoMapper only.")]
    protected ProductKitComponentMap(
        OrganizationId organizationId,
        ComponentVersionId componentVersionId,
        int count
    )
    {
        OrganizationId = organizationId;
        ComponentVersionId = componentVersionId;
        Count = count;
    }

    public ProductKitComponentMap(
        OrganizationId organizationId,
        ComponentVersion componentVersion,
        int count
    )
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        Require.NotNull(componentVersion, "Component version is required.");
        Require.IsTrue(componentVersion.OrganizationId == organizationId, "Organization ID mismatch.");
        ComponentVersionId = componentVersion.Id;

        Require.IsTrue(count > 0, "Count must be greater than zero.");
        Count = count;
    }

    public ProductKitComponentMapId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public ComponentVersionId ComponentVersionId { get; protected set; }

    public int Count { get; protected set; }
}
