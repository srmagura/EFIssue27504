using System.Text;
using AppDTOs;
using Reports.Shared;

namespace Reports.Signature;

public class SignatureSection : IHtmlBuilder
{
    private const double Width = 366;
    private const double Height = 225;
    private const double MarginTop = 60;
    private const double PaddingX = 77;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public ProjectReportOptionsDto ReportOptions { get; }

    public SignatureSection(
        ProjectReportOptionsDto reportOptions
    )
    {
        ReportOptions = reportOptions ?? throw new ArgumentNullException(nameof(reportOptions));
    }

    public string Render()
    {
        var signatureSection = new StringBuilder();

        signatureSection.Append($"<div style=\"width: {Width}pt; height: {Height}pt; margin-top: {MarginTop}pt; padding: 0 {PaddingX}pt; display: flex;\">");

        // Client

        signatureSection.Append(new SignatureSectionColumn("CLIENT", ReportOptions.SigneeName).Render());

        // Consultant

        signatureSection.Append(new SignatureSectionColumn("CONSULTANT", ReportOptions.PreparerName).Render());

        //

        signatureSection.Append("</div>");

        return signatureSection.ToString();
    }
}
