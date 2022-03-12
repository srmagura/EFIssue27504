namespace AppDTOs;

public class ComponentDto
{
    public ComponentDto(
        ComponentId id,
        OrganizationId organizationId,
        MeasurementType measurementType,
        bool isVideoDisplay,
        bool isActive,
        bool visibleToCustomer,
        ComponentTypeDto componentType
    )
    {
        Id = id;
        OrganizationId = organizationId;
        MeasurementType = measurementType;
        IsVideoDisplay = isVideoDisplay;
        IsActive = isActive;
        VisibleToCustomer = visibleToCustomer;
        ComponentType = componentType ?? throw new ArgumentNullException(nameof(componentType));
    }

    public ComponentId Id { get; set; }

    public OrganizationId OrganizationId { get; set; }

    public ComponentTypeDto ComponentType { get; set; }

    public MeasurementType MeasurementType { get; set; }
    public bool IsVideoDisplay { get; set; }
    public bool IsActive { get; set; }
    public bool VisibleToCustomer { get; set; }

    public List<ComponentVersionDto> Versions { get; set; } = new();
}
