using AppDTOs.Report;
using Reports.Shared;

namespace Reports.ProductLegend;

public class ProductKitDisplay : IHtmlBuilder
{
    private const double ContainerWidth = 76.3;
    private const double ContainerHeight = 129.8;
    private const double ContainerMarginRight = 25.7;

    private const double HeaderPaddingSide = 4.1;
    private const double HeaderPaddingTop = 3;
    private const double HeaderWidth = ContainerWidth - (HeaderPaddingSide * 2);
    private const double HeaderHeight = 12.7 - HeaderPaddingTop;
    private const double HeaderFontSize = 5.25;

    private const double BodyMargin = 4.6;
    private const double BodyWidth = ContainerWidth - (BodyMargin * 2);
    private const double BodyHeight = ContainerHeight - HeaderHeight - (BodyMargin * 2);
    private const string BodyColor = "#f3f3f3";

    private const double ImageContainerMargin = BodyMargin;
    private const double ImageContainerHeight = 60;

    private const double InfoContainerHeight = BodyHeight - ImageContainerHeight - ImageContainerMargin;
    private const double InfoSymbolSize = 16;
    private const double InfoNameMargin = 4.1;
    private const double InfoNameHeight = InfoContainerHeight - InfoNameMargin;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public ProductKitReportDto ProductKit { get; }
    public string DirectoryPath { get; }
    public bool IsLast { get; }

    public ProductKitDisplay(
        ProductKitReportDto productKit,
        string directoryPath,
        bool isLast
    )
    {
        ProductKit = productKit ?? throw new ArgumentNullException(nameof(productKit));
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
        IsLast = isLast;
    }

    public string Render()
    {
        var productPhotoHtml = ProductKit.ProductPhotoFileId != null ?
            $"<img style=\"max-width: {BodyWidth}pt; max-height: {ImageContainerHeight}pt;\" src=\"{Path.Combine(DirectoryPath, ProductKit.Id.ToString())}\" />" :
            $"<img style=\"width: {BodyWidth}pt; height: {ImageContainerHeight}pt;\" src=\"{Path.Combine(DirectoryPath, ReportConstants.ImageUnavailableFileName)}\" />";

        var symbolHtml = $"<img style=\"width: {InfoSymbolSize}pt; height: {InfoSymbolSize}pt;\" src=\"{Path.Combine(DirectoryPath, $"{ProductKit.SymbolId}.png")}\" />";

        return $@"
<a href=""{ProductKit.MainComponentUrl ?? "#"}"" style=""width: {ContainerWidth}pt; height: {ContainerHeight}pt; margin-right: {(IsLast ? 0 : ContainerMarginRight)}pt; text-decoration: none; color: {ReportConstants.TextColor};"">
	<div style=""background: {UtilityFunctions.ColorBlendFromHex(ProductKit.Color!, "#FFFFFF", 0.5)}; width: {HeaderWidth}pt; height: {HeaderHeight}pt; padding: {HeaderPaddingTop}pt {HeaderPaddingSide}pt 0;"">
        <table style=""width: {HeaderWidth}pt; height: {HeaderHeight}pt; border-collapse: collapse; font-size: {HeaderFontSize}pt; white-space: nowrap; table-layout: fixed; overflow: hidden;"">
            <tr>
                <td style=""padding: 0; vertical-align: baseline;"">{ProductKit.MainComponentName}</td>
            </tr>
        </table>
	</div>
	<div style=""background: {BodyColor}; width: {BodyWidth}pt; height: {BodyHeight}pt; padding: {BodyMargin}pt;"">
		<table style=""width: {BodyWidth}pt; height: {ImageContainerHeight}pt; margin-bottom: {ImageContainerMargin}pt; border-collapse: collapse;"">
			<tr>
				<td style=""text-align: center; vertical-align: middle; padding: 0;"">
					{productPhotoHtml}
				</td>
			</tr>
		</table>
		<div style=""width: {BodyWidth}pt; height: {InfoContainerHeight}pt; display: flex;"">
			<div style=""background: {ProductKit.Color!}; width: {InfoSymbolSize}pt; height: {InfoSymbolSize}pt; border-radius: 50%;"">
				{symbolHtml}
			</div>
			<div style=""height: {InfoNameHeight}pt; margin-left: {InfoNameMargin}pt; margin-top: {InfoNameMargin}pt; font-size: {HeaderFontSize}pt; overflow: hidden;"">
                {ProductKit.Name}
            </div>
        </div>
    </div>
</a>
";
    }
}
