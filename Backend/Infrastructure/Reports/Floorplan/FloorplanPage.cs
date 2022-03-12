using AppDTOs.Report;
using Reports.Shared;

namespace Reports.Floorplan;

public class FloorplanPage : IHtmlBuilder
{
    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public bool IsDraft { get; }
    public DesignerDataReportDto DesignerData { get; }
    public ProductKitReportDto[] ProductKits { get; }
    public string DirectoryPath { get; }

    public FloorplanPage(
        bool isDraft,
        DesignerDataReportDto designerData,
        ProductKitReportDto[] productKits,
        string directoryPath
    )
    {
        IsDraft = isDraft;
        DesignerData = designerData ?? throw new ArgumentNullException(nameof(designerData));
        ProductKits = productKits ?? throw new ArgumentNullException(nameof(productKits));
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
    }

    public string Render()
    {
        List<IHtmlBuilder> children = new List<IHtmlBuilder>();
        List<IHtmlBuilder> absolutePositionedChildren = new List<IHtmlBuilder>();

        var symbolScale = DesignerData.PageOptions?.SymbolScale ?? ReportConstants.DefaultSymbolScale;
        var symbolSize = (symbolScale / 1000) * ReportConstants.PageWidth;

        // Title Block

        children.Add(new TitleBlock());

        // Placed Product Kits

        foreach (var designerData in DesignerData.PlacedProductKits)
        {
            var productKit = ProductKits.First(p => p.Id == designerData.ProductKitId);
            absolutePositionedChildren.Add(new PlacedProductKitDisplay(designerData, productKit, symbolSize, DirectoryPath));
        }

        // Notes

        for (var i = 0; i < DesignerData.Notes.Length; i++)
        {
            // TODO:AQ Product Kit Notes in Milestone 4
            if (DesignerData.Notes[i].Position != null)
            {
                absolutePositionedChildren.Add(new NoteDisplay(DesignerData.Notes[i], i + 1, symbolSize, DirectoryPath));
            }
        }

        // Note Block

        if (DesignerData.NoteBlock != null)
        {
            absolutePositionedChildren.Add(new NoteBlockDisplay(DesignerData.NoteBlock, DesignerData.Notes, DirectoryPath));
        }

        //

        var page = new BasePageWithTitleBlock(children: children, absolutePositionedChildren: absolutePositionedChildren, isDraft: IsDraft);
        return page.Render();
    }
}
