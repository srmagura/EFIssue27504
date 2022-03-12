using AppDTOs;
using Reports.Shared;

namespace Reports.Signature;

public class SignaturePage : IHtmlBuilder
{
    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public bool IsDraft { get; }
    public ProjectDto Project { get; }
    public double Budget { get; }

    public SignaturePage(
        bool isDraft,
        ProjectDto project,
        double budget
    )
    {
        IsDraft = isDraft;
        Project = project ?? throw new ArgumentNullException(nameof(project));
        Budget = budget;
    }

    public string Render()
    {
        List<IHtmlBuilder> children = new List<IHtmlBuilder>();

        children.Add(new SignatureOuterContainer(Budget, (double)Project.BudgetOptions.DepositPercentage, Project.ReportOptions));

        var page = new BasePage(children: children, isDraft: IsDraft);
        return page.Render();
    }
}
