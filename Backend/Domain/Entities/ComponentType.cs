namespace Entities;

public class ComponentType : AggregateRoot
{
    public ComponentType(OrganizationId organizationId, string name)
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        SetName(name);
    }

    public ComponentTypeId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Name { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        Require.HasValue(name, "Name is required.");
        Name = name;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }
}
