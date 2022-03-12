using AppDTOs.Report;
using Reports.Shared;

namespace Reports.ProductLegend;

public class ProductLegendPage : IHtmlBuilder
{
    private const double TitleBlockMargin = 37.4;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public bool IsDraft { get; }
    public List<ProductKitReportDto[]> ProductKitRows { get; }
    public string DirectoryPath { get; }

    public ProductLegendPage(
        bool isDraft,
        List<ProductKitReportDto[]> productKitRows,
        string directoryPath
    )
    {
        IsDraft = isDraft;
        ProductKitRows = productKitRows ?? throw new ArgumentNullException(nameof(productKitRows));
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
    }

    public string Render()
    {
        List<IHtmlBuilder> children = new List<IHtmlBuilder>();

        // Title Block

        children.Add(new TitleBlock(style: $"margin-left: {TitleBlockMargin}pt;"));

        // Product Kit List

        children.Add(new ProductKitList(ProductKitRows, DirectoryPath));

        //

        var page = new BasePageWithTitleBlock(children: children, isDraft: IsDraft);
        return page.Render();
    }
}
