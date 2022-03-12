using AppDTOs.Designer;
using AppDTOs.Report;
using Reports.Shared;

namespace Reports.Floorplan;

public class PlacedProductKitDisplay : IHtmlBuilder
{
    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public PlacedProductKitDto DesignerData { get; }
    public ProductKitReportDto ProductKit { get; }
    public double SymbolSize { get; }
    public string DirectoryPath { get; }

    public PlacedProductKitDisplay(
        PlacedProductKitDto designerData,
        ProductKitReportDto productKit,
        double symbolSize,
        string directoryPath
    )
    {
        DesignerData = designerData ?? throw new ArgumentNullException(nameof(designerData));
        ProductKit = productKit ?? throw new ArgumentNullException(nameof(productKit));
        SymbolSize = symbolSize;
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
    }

    public string Render()
    {
        var left = (DesignerData.Position[0] * 72) - (SymbolSize / 2);
        var top = (DesignerData.Position[1] * 72) - (SymbolSize / 2);

        return $@"
<div style=""background: {ProductKit.Color!}; width: {SymbolSize}pt; height: {SymbolSize}pt; border-radius: 50%; position: absolute; left: {left}pt; top: {top}pt; transform: rotate({DesignerData.Rotation}deg); z-index: {ReportConstants.PlacedProductKitZIndex};"">
	<img style=""width: {SymbolSize}pt; height: {SymbolSize}pt;"" src=""{Path.Combine(DirectoryPath, $"{ProductKit.SymbolId}.png")}"" />
</div>";
    }
}
