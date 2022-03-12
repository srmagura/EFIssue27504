using System.Text;

namespace Reports.Shared;

public class BasePage : IHtmlBuilder
{
    private const double DraftTextLeft = 10;
    private const double DraftTextTop = 10;
    private const double DraftTextFontSize = 50;
    private const double DraftTextOpacity = 0.2;
    private const string DraftTextColor = "blue";

    public List<IHtmlBuilder> Children { get; }
    public string? Style { get; }

    public bool IsDraft { get; }

    public BasePage(List<IHtmlBuilder> children, bool isDraft, string? style = null)
    {
        Children = children;
        IsDraft = isDraft;
        Style = style;
    }

    public string Render()
    {
        var children = new StringBuilder();

        if (IsDraft)
        {
            children.Append($@"
<div style=""position: absolute; left: {DraftTextLeft}pt; top: {DraftTextTop}pt; font-size: {DraftTextFontSize}pt; opacity: {DraftTextOpacity}; color: {DraftTextColor}; z-index: {ReportConstants.DraftTextZIndex}"">
    DRAFT
</div>
");
        }

        Children.ForEach(c => children.Append(c.Render()));

        return $@"
<body style=""height: {ReportConstants.ContentHeight}pt; padding: {ReportConstants.PagePadding}pt; margin: 0; font-family: arial; {Style ?? ""}"">
    {children}
</body>
";
    }
}
