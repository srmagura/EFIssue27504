using System.Text;
using Reports.Shared;

namespace Reports.Title;

public class SheetIndexSection : IHtmlBuilder
{
    private const double FontSize = 5;
    private const double ColumnPaddingRight = 17;
    private const double RowPaddingBottom = 6;
    private const double TableSpacerHeight = 10;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    private readonly List<(string? Number, string? Name)> _sheetIndex;

    public SheetIndexSection(List<(string? Number, string? Name)> sheetIndex)
    {
        _sheetIndex = sheetIndex ?? throw new ArgumentNullException(nameof(sheetIndex));
    }

    public string Render()
    {
        var sheetIndexSection = new StringBuilder();

        sheetIndexSection.Append($@"
<table style=""border-collapse: collapse;"">
	<tr style=""font-size: {FontSize}pt; font-weight: bold;"">
		<td style=""padding-right: {ColumnPaddingRight}pt;"">SHEET NO.</td>
		<td>SHEET NAME</td>
	</tr>
	<tr style=""height: {TableSpacerHeight}pt;"" />
");

        // Index Rows

        _sheetIndex.ForEach(s =>
        {
            if (s.Number == null || s.Name == null)
            {
                sheetIndexSection.Append(MissingTypeRow());
            }
            else
            {
                sheetIndexSection.Append(Row(s.Number.ToUpperInvariant(), s.Name.ToUpperInvariant()));
            }
        });

        //

        sheetIndexSection.Append("</table>");

        return sheetIndexSection.ToString();
    }

    private static string MissingTypeRow()
    {
        return $@"
    <tr style=""font-size: {FontSize}pt; font-style: italic; color: red; padding-bottom: {RowPaddingBottom}pt;"">
	    <td>XX.XXX</td>
	    <td>Please select a sheet type for this page</td>
    </tr>
";
    }

    private static string Row(string sheetNumber, string sheetName)
    {
        return $@"
    <tr style=""font-size: {FontSize}pt; color: {ReportConstants.TextColorLight}; padding-bottom: {RowPaddingBottom}pt;"">
	    <td>{sheetNumber}</td>
	    <td>{sheetName}</td>
    </tr>
";
    }
}
