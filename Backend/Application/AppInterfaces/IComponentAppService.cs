namespace AppInterfaces;

public interface IComponentAppService
{
    Task<ComponentDto?> GetAsync(ComponentId id);

    Task<ComponentSummaryDto[]> ListAsync(OrganizationId organizationId);

    Task<string> GetNewVersionNameAsync(ComponentId id);

    Task<bool> VendorPartNumberIsAvailableAsync(OrganizationId organizationId, ComponentId? componentId, string vendorPartNumber);

    Task<bool> OrganizationPartNumberIsAvailableAsync(OrganizationId organizationId, ComponentId? componentId, string organizationPartNumber);

    Task<ComponentId> AddAsync(
        OrganizationId organizationId,
        ComponentTypeId componentTypeId,
        MeasurementType measurementType,
        bool isVideoDisplay,
        bool visibleToCustomer,
        string displayName,
        string versionName,
        decimal sellPrice,
        string? url,
        string make,
        string model,
        string vendorPartNumber,
        string? organizationPartNumber,
        string? whereToBuy,
        string? style,
        string? color,
        string? internalNotes
    );

    Task<ComponentVersionId> AddVersionAsync(
        ComponentId id,
        string displayName,
        string versionName,
        decimal sellPrice,
        string? url,
        string make,
        string model,
        string vendorPartNumber,
        string? organizationPartNumber,
        string? whereToBuy,
        string? style,
        string? color,
        string? internalNotes
    );

    Task UpdateVersionAsync(
        ComponentVersionId versionId,
        string displayName,
        string? url,
        string make,
        string model,
        string vendorPartNumber,
        string? organizationPartNumber,
        string? whereToBuy,
        string? style,
        string? color,
        string? internalNotes
    );
    Task SetActiveAsync(ComponentId id, bool active);

    Task SetComponentTypeAsync(ComponentId id, ComponentTypeId componentTypeId);

    Task SetVisibleToCustomerAsync(ComponentId id, bool visibleToCustomer);
}
