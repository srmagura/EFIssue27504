namespace AppDTOs;

public class ProductKitSummaryDto
{
    public ProductKitSummaryDto(
        ProductKitId id,
        MeasurementType measurementType,
        bool isActive,
        string currentVersionName,
        string name,
        decimal sellPrice,
        string symbolSvgText,
        string mainComponentName
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        MeasurementType = measurementType;
        IsActive = isActive;
        CurrentVersionName = currentVersionName ?? throw new ArgumentNullException(nameof(currentVersionName));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        SellPrice = sellPrice;
        SymbolSvgText = symbolSvgText ?? throw new ArgumentNullException(nameof(symbolSvgText));
        MainComponentName = mainComponentName ?? throw new ArgumentNullException(nameof(mainComponentName));
    }

    public ProductKitId Id { get; set; }

    public MeasurementType MeasurementType { get; set; }
    public bool IsActive { get; set; }
    public string CurrentVersionName { get; set; }

    public string Name { get; set; }
    public decimal SellPrice { get; set; }
    public string SymbolSvgText { get; set; }
    public string MainComponentName { get; set; }
}
