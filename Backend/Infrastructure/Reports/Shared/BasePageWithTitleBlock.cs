using System.Text;

namespace Reports.Shared;

public class BasePageWithTitleBlock : IHtmlBuilder
{
    private const double DraftTextLeft = 10;
    private const double DraftTextTop = 10;
    private const double DraftTextFontSize = 50;
    private const double DraftTextOpacity = 0.2;
    private const string DraftTextColor = "blue";

    public List<IHtmlBuilder> Children { get; }
    public string? Style { get; }

    public bool IsDraft { get; }
    public List<IHtmlBuilder> AbsolutePositionedChildren { get; }

    public BasePageWithTitleBlock(List<IHtmlBuilder> children, bool isDraft, List<IHtmlBuilder>? absolutePositionedChildren = null, string? style = null)
    {
        Children = children;
        IsDraft = isDraft;
        AbsolutePositionedChildren = absolutePositionedChildren ?? new List<IHtmlBuilder>();
        Style = style;
    }

    public string Render()
    {
        var children = new StringBuilder();
        Children.ForEach(c => children.Append(c.Render()));

        var absolutePositionedChildren = new StringBuilder();

        if (IsDraft)
        {
            absolutePositionedChildren.Append($@"
<div style=""position: absolute; left: {DraftTextLeft}pt; top: {DraftTextTop}pt; font-size: {DraftTextFontSize}pt; opacity: {DraftTextOpacity}; color: {DraftTextColor}; z-index: {ReportConstants.DraftTextZIndex}"">
    DRAFT
</div>
");
        }

        AbsolutePositionedChildren.ForEach(c => absolutePositionedChildren.Append(c.Render()));

        return $@"
<body style=""height: {ReportConstants.ContentHeight}pt; padding: {ReportConstants.PagePadding}pt; margin: 0; font-family: arial; {Style ?? ""}"">
    {absolutePositionedChildren}
	<div style=""height: {ReportConstants.ContentHeight}pt; display: flex; flex-direction: row-reverse; justify-content: flex-start;"">
        {children}
    </div>
</body>
";
    }
}
