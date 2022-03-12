using System.Text;
using Reports.Shared;

namespace Reports.Signature;

public class SignatureSectionColumn : IHtmlBuilder
{
    private const double Width = 167;
    private const double Height = 209;
    private const double Padding = 8;
    private const double FontSize = 9;
    private const string Background = "#F2F2F2";

    private static readonly int[] RowHeights = { 60, 35, 75, 39 };

    private const double ByMargin = 15;
    private const double ByWidth = Width - 25;

    private const double DateMargin = 23;
    private const double DateWidth = Width - 33;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public string Label { get; }
    public string Signer { get; }

    public SignatureSectionColumn(
        string label,
        string signer
    )
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
        Signer = signer ?? throw new ArgumentNullException(nameof(signer));
    }

    private static string RenderSignatureLine(double width, double marginLeft, string label)
    {
        return $@"
<span>{label}: </span>
<hr style=""width: {width}pt; margin-left: {marginLeft}pt; margin-top: -1pt; border-top: none; border-bottom: 1px solid black;""/>
";
    }

    private static string RenderRow(double height, string child, string extraStyles = "")
    {
        return $@"
<tr>
    <td style=""height: {height}pt; padding: 0; vertical-align: baseline; {extraStyles}"">
        {child}
    </td>
</tr>
";
    }

    public string Render()
    {
        var signatureSectionColumn = new StringBuilder();

        signatureSectionColumn.Append($@"
<div style=""width: {Width}pt; height: {Height}pt; padding: {Padding}pt; background: {Background};"">
    <table style=""width: {Width}pt; height: {Height}pt; font-size: {FontSize}pt; border-collapse: collapse;"">
");

        signatureSectionColumn.Append(RenderRow(RowHeights[0], $"<b>{Label}</b>"));

        signatureSectionColumn.Append(RenderRow(RowHeights[1], RenderSignatureLine(ByWidth, ByMargin, "By")));

        signatureSectionColumn.Append(RenderRow(RowHeights[2], Signer, "text-align: center;"));

        signatureSectionColumn.Append(RenderRow(RowHeights[1], RenderSignatureLine(DateWidth, DateMargin, "Date")));

        signatureSectionColumn.Append("</table></div>");

        return signatureSectionColumn.ToString();
    }
}
