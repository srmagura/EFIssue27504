using AppDTOs.Designer;
using Reports.Shared;

namespace Reports.Floorplan;

public class NoteIcon : IHtmlBuilder
{
    private const string TextColor = "#F8F9FA";
    private const double TextVerticalOffsetMultiplier = -0.85;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public NoteDto DesignerData { get; }
    public int Index { get; }
    public double SymbolSize { get; }
    public string DirectoryPath { get; }

    public NoteIcon(
        NoteDto designerData,
        int index,
        double symbolSize,
        string directoryPath
    )
    {
        DesignerData = designerData ?? throw new ArgumentNullException(nameof(designerData));
        Index = index;
        SymbolSize = symbolSize;
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
    }

    public string Render()
    {
        var imageSrc = Path.Combine(DirectoryPath, ReportConstants.NoteIconFileName);

        var height = SymbolSize;
        var width = height * (3d / 4d);

        var fontSize = ReportConstants.NoteFontSizeMultiplier * height;

        return $@"
<div style=""width: {width}pt; height: {height}pt;"">
	<img style=""width: {width}pt; height: {height}pt;"" src=""{imageSrc}"" />
	<table style=""width: {width}pt; position: relative; top: {height * TextVerticalOffsetMultiplier}pt; color: {TextColor}; font-size: {fontSize}pt; font-weight: bold; border-collapse: collapse; text-align: center;"">
        <tr>
            <td style=""padding: 0;"">
                {Index}
            </td>
        </tr>
    </table>
</div>";
    }
}
