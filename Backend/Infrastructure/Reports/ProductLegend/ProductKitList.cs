using System.Text;
using AppDTOs.Report;
using Reports.Shared;

namespace Reports.ProductLegend;

public class ProductKitList : IHtmlBuilder
{
    private const double ListWidth = 1036.3;
    private const double ListPaddingTop = 24.2;
    private const double ListHeight = ReportConstants.ContentHeight - ListPaddingTop;

    private string CurrentCategory { get; set; } = ""; // State variable

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public List<ProductKitReportDto[]> ProductKitRows { get; }
    public string DirectoryPath { get; }

    public ProductKitList(
        List<ProductKitReportDto[]> productKitRows,
        string directoryPath
    )
    {
        ProductKitRows = productKitRows ?? throw new ArgumentNullException(nameof(productKitRows));
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));

        if (productKitRows.Count > ReportConstants.ProductLegendTotalRowCount || productKitRows.Count == 0) throw new ArgumentOutOfRangeException(nameof(productKitRows));
    }

    private void RenderSection(StringBuilder sb, List<ProductKitReportDto[]> rows)
    {
        sb.Append($"<div style=\"height: {ListHeight}pt;\">");

        for (var i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            if (row.Length == 0) throw new Exception("Product Kit row cannot be empty.");

            bool showHeader = row[0].CategoryName != CurrentCategory;
            CurrentCategory = row[0].CategoryName;

            bool isLast = i == rows.Count - 1;

            sb.Append(new ProductKitRow(row, DirectoryPath, showHeader, isLast).Render());
        }

        sb.Append("</div>");
    }

    public string Render()
    {
        var productKitList = new StringBuilder();

        productKitList.Append($@"
<div style=""width: {ListWidth}pt; padding-top: {ListPaddingTop}pt;"">
	<div style=""width: {ListWidth}pt; height: {ListHeight}pt;"">
		<div style=""display: flex; justify-content: space-between;"">
");

        // Product Legend Sections

        var leftSideRows = ProductKitRows.Skip(0).Take(ReportConstants.ProductLegendSectionRowCount).ToList();
        var rightSideRows = ProductKitRows.Skip(ReportConstants.ProductLegendSectionRowCount).Take(ReportConstants.ProductLegendSectionRowCount).ToList();

        if (leftSideRows.Count == 0) throw new Exception("Product Kit list cannot be empty.");
        RenderSection(productKitList, leftSideRows);

        if (rightSideRows.Count > 0) RenderSection(productKitList, rightSideRows);

        //

        productKitList.Append("</div></div></div>");

        return productKitList.ToString();
    }
}
