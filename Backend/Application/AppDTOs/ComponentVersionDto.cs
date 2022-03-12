namespace AppDTOs;

public class ComponentVersionDto
{
    public ComponentVersionDto(
        ComponentVersionId id,
        ComponentId componentId,
        string displayName,
        string versionName,
        decimal sellPrice,
        string? url,
        string make,
        string model,
        string vendorPartNumber
    )
    {
        Id = id;
        ComponentId = componentId;
        DisplayName = displayName;
        VersionName = versionName;
        SellPrice = sellPrice;
        Url = url;
        Make = make;
        Model = model;
        VendorPartNumber = vendorPartNumber;
    }

    public ComponentVersionId Id { get; set; }
    public ComponentId ComponentId { get; set; }
    public DateTimeOffset DateCreatedUtc { get; set; }

    public string DisplayName { get; set; }
    public string VersionName { get; set; }

    public decimal SellPrice { get; set; }
    public string? Url { get; set; }

    public string Make { get; set; }
    public string Model { get; set; }
    public string VendorPartNumber { get; set; }
    public string? OrganizationPartNumber { get; set; }
    public string? WhereToBuy { get; set; }
    public string? Style { get; set; }
    public string? Color { get; set; }
    public string? InternalNotes { get; set; }
}
