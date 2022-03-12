using Entities;

namespace AppDTOs;

public class ProductKitVersionDto
{
    public ProductKitVersionDto(
        ProductKitVersionId id,
        OrganizationId organizationId,
        ProductKitId productKitId,
        string name,
        string description,
        string versionName,
        SymbolDto symbol,
        ComponentVersionDto mainComponentVersion
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        ProductKitId = productKitId ?? throw new ArgumentNullException(nameof(productKitId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        VersionName = versionName ?? throw new ArgumentNullException(nameof(versionName));
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        MainComponentVersion = mainComponentVersion ?? throw new ArgumentNullException(nameof(mainComponentVersion));
    }

    public ProductKitVersionId Id { get; set; }
    public OrganizationId OrganizationId { get; set; }
    public ProductKitId ProductKitId { get; set; }
    public DateTimeOffset DateCreatedUtc { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string VersionName { get; set; }
    public decimal SellPrice { get; set; }

    public SymbolDto Symbol { get; set; }
    public ProductPhotoDto? ProductPhoto { get; set; }

    public ComponentVersionDto MainComponentVersion { get; set; }
    public List<ProductKitComponentMapDto> ComponentMaps { get; set; } = new List<ProductKitComponentMapDto>();

    public bool IsVideoDisplay => ProductKit.IsVideoDisplay(
        ComponentMaps.Select(m => m.IsVideoDisplay).ToArray()
    );
}
