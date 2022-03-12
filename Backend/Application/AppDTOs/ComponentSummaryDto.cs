namespace AppDTOs;

public class ComponentSummaryDto
{
    public ComponentSummaryDto(
        ComponentId id,
        MeasurementType measurementType,
        bool isVideoDisplay,
        bool isActive,
        bool visibleToCustomer,
        ComponentVersionId currentVersionId,
        string displayName,
        string currentVersionName,
        decimal sellPrice,
        string componentTypeName,
        string make,
        string model,
        string vendorPartNumber,
        string? organizationPartNumber
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        MeasurementType = measurementType;
        IsVideoDisplay = isVideoDisplay;
        IsActive = isActive;
        VisibleToCustomer = visibleToCustomer;
        CurrentVersionId = currentVersionId ?? throw new ArgumentNullException(nameof(currentVersionId));
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        CurrentVersionName = currentVersionName ?? throw new ArgumentNullException(nameof(currentVersionName));
        SellPrice = sellPrice;
        ComponentTypeName = componentTypeName ?? throw new ArgumentNullException(nameof(componentTypeName));
        Make = make ?? throw new ArgumentNullException(nameof(make));
        Model = model ?? throw new ArgumentNullException(nameof(model));
        VendorPartNumber = vendorPartNumber ?? throw new ArgumentNullException(nameof(vendorPartNumber));
        OrganizationPartNumber = organizationPartNumber;
    }

    public ComponentId Id { get; set; }

    public string ComponentTypeName { get; set; }

    public MeasurementType MeasurementType { get; set; }
    public bool IsVideoDisplay { get; set; }
    public bool IsActive { get; set; }
    public bool VisibleToCustomer { get; set; }

    public ComponentVersionId CurrentVersionId { get; set; }
    public string DisplayName { get; set; }
    public string CurrentVersionName { get; set; }
    public decimal SellPrice { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string VendorPartNumber { get; set; }
    public string? OrganizationPartNumber { get; set; }
}
