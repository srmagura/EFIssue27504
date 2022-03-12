namespace AppDTOs;

public class ProductKitComponentMapDto
{
    public ProductKitComponentMapDto(
        ProductKitComponentMapId id,
        ComponentId componentId,
        ComponentVersionId componentVersionId,
        string displayName,
        string versionName,
        string make,
        string model,
        string vendorPartNumber
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        ComponentId = componentId ?? throw new ArgumentNullException(nameof(componentId));
        ComponentVersionId = componentVersionId ?? throw new ArgumentNullException(nameof(componentVersionId));
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        VersionName = versionName ?? throw new ArgumentNullException(nameof(versionName));
        Make = make ?? throw new ArgumentNullException(nameof(make));
        Model = model ?? throw new ArgumentNullException(nameof(model));
        VendorPartNumber = vendorPartNumber ?? throw new ArgumentNullException(nameof(vendorPartNumber));
    }

    public ProductKitComponentMapId Id { get; set; }

    public ComponentId ComponentId { get; set; }
    public ComponentVersionId ComponentVersionId { get; set; }

    public int Count { get; set; }

    public MeasurementType MeasurementType { get; set; }
    public bool IsVideoDisplay { get; set; }
    public bool VisibleToCustomer { get; set; }

    public string DisplayName { get; set; }
    public string VersionName { get; set; }

    public decimal SellPrice { get; set; }
    public string? Url { get; set; }

    public string Make { get; set; }
    public string Model { get; set; }
    public string VendorPartNumber { get; set; }
    public string? OrganizationPartNumber { get; set; }
}
