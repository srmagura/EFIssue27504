namespace AppDTOs.Report;

public class ProductKitReportDto
{
    public ProductKitReportDto(
        ProductKitId id,
        string categoryName,
        string? color,
        string name,
        FileId? productPhotoFileId,
        SymbolId symbolId,
        string symbolSvgText,
        string mainComponentName,
        string? mainComponentUrl
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        Color = color;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ProductPhotoFileId = productPhotoFileId;
        SymbolId = symbolId ?? throw new ArgumentNullException(nameof(symbolId));
        SymbolSvgText = symbolSvgText ?? throw new ArgumentNullException(nameof(symbolSvgText));
        MainComponentName = mainComponentName ?? throw new ArgumentNullException(nameof(mainComponentName));
        MainComponentUrl = mainComponentUrl;
    }

    public ProductKitId Id { get; set; }

    public string CategoryName { get; set; }
    public string? Color { get; set; }

    public string Name { get; set; }
    public FileId? ProductPhotoFileId { get; set; }
    public SymbolId SymbolId { get; set; }
    public string SymbolSvgText { get; set; }
    public string MainComponentName { get; set; }
    public string? MainComponentUrl { get; set; }
}
