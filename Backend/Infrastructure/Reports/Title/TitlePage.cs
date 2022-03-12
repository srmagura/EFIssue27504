using AppDTOs;
using Reports.Shared;

namespace Reports.Title;

public class TitlePage : IHtmlBuilder
{
    private const double TitleBlockMargin = 51;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public bool IsDraft { get; }
    public ProjectDto Project { get; }
    public List<(string? Number, string? Name)> SheetIndex { get; }
    public string DirectoryPath { get; }

    public TitlePage(
        bool isDraft,
        ProjectDto project,
        List<(string? Number, string? Name)> sheetIndex,
        string directoryPath
    )
    {
        IsDraft = isDraft;
        Project = project ?? throw new ArgumentNullException(nameof(project));
        SheetIndex = sheetIndex ?? throw new ArgumentNullException(nameof(sheetIndex));
        DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
    }

    public string Render()
    {
        List<IHtmlBuilder> children = new List<IHtmlBuilder>();

        // Title Block

        children.Add(new TitleBlock(style: $"margin-left: {TitleBlockMargin}pt;"));

        // Project Info Section

        children.Add(new ProjectInfoSection(Project, SheetIndex));

        // Project Photo Section

        if (Project.Photo != null)
        {
            var projectPhotoPath = Path.Combine(DirectoryPath, Project.Id.ToString());
            children.Add(new ProjectPhotoSection(projectPhotoPath));
        }

        //

        var page = new BasePageWithTitleBlock(children: children, isDraft: IsDraft);
        return page.Render();
    }
}
