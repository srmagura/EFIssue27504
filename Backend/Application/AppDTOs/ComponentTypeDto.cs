namespace AppDTOs;

public class ComponentTypeDto
{
    public ComponentTypeDto(ComponentTypeId id, OrganizationId organizationId, string name)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public ComponentTypeId Id { get; set; }

    public OrganizationId OrganizationId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }
}
