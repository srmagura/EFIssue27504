using System.Text;
using AppDTOs.Report;
using Reports.Shared;

namespace Reports.ProductLegend;

public class ProductKitRow : IHtmlBuilder
{
    private const double ContainerWidth = 484.3;
    private const double ContainerHeight = 167;
    private const double ContainerMarginBottom = 22.3;

    private const double HeaderHeight = 20.4;
    private const double HeaderMarginBottom = 16.8;
    private const double HeaderFontSize = 10;
    private const double HeaderPadding = 13.2;

    private const double RowHeight = 129.8;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public ProductKitReportDto[] ProductKits { get; }
    public string DirectoryPath { get; }
    public bool ShowHeader { get; }
    public bool IsLast { get; }

    public ProductKitRow(
        ProductKitReportDto[] productKits,
        string directoryPath,
        bool showHeader,
        bool isLast
    )
    {
        ProductKits = productKits ?? throw new ArgumentNullException(nameof(productKits));
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
        ShowHeader = showHeader;
        IsLast = isLast;

        if (productKits.Length > ReportConstants.ProductLegendItemCount || productKits.Length == 0) throw new ArgumentOutOfRangeException(nameof(productKits));
    }

    public string Render()
    {
        var productKitRow = new StringBuilder();

        var header = ShowHeader ? ProductKits[0].CategoryName.ToUpperInvariant() : "";
        var headerColor = ShowHeader ? ProductKits[0].Color! : "white";

        productKitRow.Append($@"
<div style=""width: {ContainerWidth}pt; height: {ContainerHeight}pt; margin-bottom: {(IsLast ? 0 : ContainerMarginBottom)}pt"">
	<table style=""height: {HeaderHeight}pt; margin-bottom: {HeaderMarginBottom}pt; border-collapse: collapse;"">
		<tr style=""background: {headerColor};"">
			<td style=""font-size: {HeaderFontSize}pt; color: white; padding: 0 {HeaderPadding}pt;"">
				{header}
			</td>
		</tr>
	</table>
	<div style=""height: {RowHeight}pt; display: flex;"">
");

        // Product Kit Items

        for (var i = 0; i < ProductKits.Length; i++)
        {
            bool isLast = i == ProductKits.Length - 1;
            productKitRow.Append(new ProductKitDisplay(ProductKits[i], DirectoryPath, isLast).Render());
        }

        //

        productKitRow.Append("</div></div>");

        return productKitRow.ToString();
    }
}
