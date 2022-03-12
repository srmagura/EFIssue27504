namespace Reports.Shared;

public class TitleBlock : IHtmlBuilder
{
    private const double TitleBlockWidth = 102.8;
    private const double TitleBlockHeaderHeight = 48.8;
    private const double TitleBlockBodyHeight = ReportConstants.ContentHeight - TitleBlockHeaderHeight;

    private const string TitleBlockHeaderColor = "#4d4a4b";
    private const string TitleBlockBodyColor = "#f3f3f3";

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public TitleBlock(string? style = null)
    {
        Style = style;
    }

    public string Render()
    {
        return $@"
<div style=""width: {TitleBlockWidth}pt; {Style ?? ""}"">
	<div style=""height: {TitleBlockHeaderHeight}pt; background: {TitleBlockHeaderColor}""></div>
	<div style=""height: {TitleBlockBodyHeight}pt; background: {TitleBlockBodyColor}""></div>
</div>
";
    }
}
