namespace AppDTOs.Designer;

public class ProductKitDesignerDto
{
    public ProductKitDesignerDto(
        ProductKitId id,
        CategoryId categoryId,
        MeasurementType measurementType,
        bool isActive,
        ProductKitVersionId versionId,
        string versionName,
        string name,
        decimal sellPrice,
        ProductPhotoId? productPhotoId,
        string symbolSvgText,
        string mainComponentName,
        string? tag
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        CategoryId = categoryId ?? throw new ArgumentNullException(nameof(categoryId));
        MeasurementType = measurementType;
        IsActive = isActive;
        VersionId = versionId ?? throw new ArgumentNullException(nameof(versionId));
        VersionName = versionName ?? throw new ArgumentNullException(nameof(versionName));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        SellPrice = sellPrice;
        ProductPhotoId = productPhotoId;
        SymbolSvgText = symbolSvgText ?? throw new ArgumentNullException(nameof(symbolSvgText));
        MainComponentName = mainComponentName ?? throw new ArgumentNullException(nameof(mainComponentName));
        Tag = tag;
    }

    public ProductKitId Id { get; set; }
    public CategoryId CategoryId { get; set; }
    public MeasurementType MeasurementType { get; set; }
    public bool IsActive { get; set; }

    public ProductKitVersionId VersionId { get; set; }
    public string VersionName { get; set; }

    public string Name { get; set; }
    public decimal SellPrice { get; set; }
    public ProductPhotoId? ProductPhotoId { get; set; }
    public string SymbolSvgText { get; set; }
    public string MainComponentName { get; set; }

    public string? Tag { get; set; }
}
