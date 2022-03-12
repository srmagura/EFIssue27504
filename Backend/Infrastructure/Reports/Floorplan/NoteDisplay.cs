using AppDTOs.Designer;
using Reports.Shared;

namespace Reports.Floorplan;

public class NoteDisplay : IHtmlBuilder
{
    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public NoteDto DesignerData { get; }
    public int Index { get; }
    public double SymbolSize { get; }
    public string DirectoryPath { get; }

    public NoteDisplay(
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
        var height = SymbolSize;
        var width = height * (3d / 4d);

        var left = (DesignerData.Position![0] * 72) - (width / 2);
        var top = (DesignerData.Position![1] * 72) - height;

        var noteIcon = new NoteIcon(DesignerData, Index, SymbolSize, DirectoryPath);

        return $@"
<div style=""width: {width}pt; height: {height}pt; position: absolute; left: {left}pt; top: {top}pt; z-index: {ReportConstants.NoteZIndex};"">
    {noteIcon.Render()}
</div>";
    }
}
