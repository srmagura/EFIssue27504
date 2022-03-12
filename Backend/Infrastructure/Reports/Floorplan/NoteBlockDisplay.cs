using System.Text;
using AppDTOs.Designer;
using Reports.Shared;

namespace Reports.Floorplan;

public class NoteBlockDisplay : IHtmlBuilder
{
    private const string Color = "#F8F9FA";
    private const string BorderColor = "#ADB5BD";
    private const double Padding = 6;

    private const double NoteTableMargin = 6;
    private const double NoteRowBottomMargin = 3;
    private const double NoteLeftMargin = 2;
    private const double NoteFontSize = 6;
    private const double NoteLineHeight = 9;
    private const double NoteIconHeight = 9.52;
    private const double NoteIconWidth = NoteIconHeight * (3d / 4d);

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public NoteBlockDto DesignerData { get; }
    public NoteDto[] Notes { get; }
    public string DirectoryPath { get; }

    public NoteBlockDisplay(
        NoteBlockDto designerData,
        NoteDto[] notes,
        string directoryPath
    )
    {
        DesignerData = designerData ?? throw new ArgumentNullException(nameof(designerData));
        Notes = notes ?? throw new ArgumentNullException(nameof(notes));
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
    }

    private string RenderNote(NoteDto designerData, int index, double tableWidth)
    {
        var note = new StringBuilder();

        note.Append($"<tr style=\"vertical-align: top; border-bottom: {NoteRowBottomMargin}pt solid transparent;\">");

        note.Append($"<td>{new NoteIcon(designerData, index, NoteIconHeight, DirectoryPath).Render()}</td>");

        note.Append($"<td style=\"width: {tableWidth - NoteIconWidth - NoteLeftMargin}pt; border-left: {NoteLeftMargin}pt solid transparent; font-size: {NoteFontSize}pt; line-height: {NoteLineHeight}pt;\">{designerData.Text}</td>");

        note.Append("</tr>");

        return note.ToString();
    }

    private string RenderNotes(double width)
    {
        var notes = new StringBuilder();

        notes.Append($"<div style=\"width: {width}pt; display: flex; justify-content: space-between;\">");

        var breakPointIndex = Math.Ceiling((double)Notes.Length / DesignerData.Columns);

        var tableWidth = DesignerData.Columns == 2 ? (width - NoteTableMargin) / 2 : width;

        notes.Append($"<table style=\"width: {tableWidth}pt; border-collapse: collapse; break-inside: avoid;\">");

        var i = 0;
        for (; i < breakPointIndex; i++)
        {
            notes.Append(RenderNote(Notes[i], i + 1, tableWidth));
        }

        notes.Append("</table>");

        if (i < Notes.Length)
        {
            notes.Append($"<table style=\"width: {tableWidth}pt; border-collapse: collapse; break-inside: avoid;\">");

            for (; i < Notes.Length; i++)
            {
                notes.Append(RenderNote(Notes[i], i + 1, tableWidth));
            }

            notes.Append("</table>");
        }

        notes.Append("</div>");

        return notes.ToString();
    }

    public string Render()
    {
        var left = DesignerData.Position[0] * 72;
        var top = DesignerData.Position[1] * 72;

        // In the UI, the box has padding, this accounts for that.
        var width = (DesignerData.Dimensions[0] * 72) - (Padding * 2);
        var height = (DesignerData.Dimensions[1] * 72) - (Padding * 2);

        var noteBlock = new StringBuilder();

        noteBlock.Append($@"<div style=""background: {Color}; padding: {Padding}pt; border-color: {BorderColor}; border-style: dashed; z-index: {ReportConstants.NoteBlockZIndex}; width: {width}pt; height: {height}pt; position: absolute; left: {left}pt; top: {top}pt;"">");

        // Notes

        noteBlock.Append(RenderNotes(width));

        //

        noteBlock.Append("</div>");

        return noteBlock.ToString();
    }
}
