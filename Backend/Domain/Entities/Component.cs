using Enumerations;

namespace Entities;

public class Component : AggregateRoot
{
    public Component(
        OrganizationId organizationId,
        ComponentTypeId componentTypeId,
        MeasurementType measurementType,
        bool isVideoDisplay,
        bool visibleToCustomer
    )
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        Require.NotNull(componentTypeId, "Component type ID is required.");
        ComponentTypeId = componentTypeId;

        MeasurementType = measurementType;
        IsVideoDisplay = isVideoDisplay;

        VisibleToCustomer = visibleToCustomer;
    }

    public ComponentId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public ComponentTypeId ComponentTypeId { get; protected set; }

    public MeasurementType MeasurementType { get; protected set; }
    public bool IsVideoDisplay { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    public bool VisibleToCustomer { get; protected set; }

    public void SetActive(bool active)
    {
        IsActive = active;
    }

    public void SetComponentType(ComponentType componentType)
    {
        Require.IsTrue(
            OrganizationId == componentType.OrganizationId,
            "Component type must be from the same organization."
        );
        ComponentTypeId = componentType.Id;
    }

    public void SetVisibleToCustomer(bool visibleToCustomer)
    {
        VisibleToCustomer = visibleToCustomer;
    }
}
