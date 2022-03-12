using System.Text;
using AppDTOs;
using Reports.Shared;

namespace Reports.Signature;

public class SignatureOuterContainer : IHtmlBuilder
{
    private const double Width = 520;
    private const double Height = 713;
    private const double MarginLeft = 289.5;
    private const double PaddingX = 46;
    private const double PaddingY = 23;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public double Budget { get; }
    public double DepositPercentage { get; }
    public ProjectReportOptionsDto ReportOptions { get; }

    public SignatureOuterContainer(
        double budget,
        double depositPercentage,
        ProjectReportOptionsDto reportOptions
    )
    {
        Budget = budget;
        DepositPercentage = depositPercentage;
        ReportOptions = reportOptions ?? throw new ArgumentNullException(nameof(reportOptions));
    }

    public string Render()
    {
        var signatureOuterContainer = new StringBuilder();

        signatureOuterContainer.Append($@"
<div style=""width: {Width}pt; height: {Height}pt; margin-left: {MarginLeft}pt; padding: {PaddingY}pt {PaddingX}pt;"">
    <span style=""font-weight: bold;"">DEPOSIT</span>
");

        // Info Section

        signatureOuterContainer.Append(new SignatureInfoSection(Budget, DepositPercentage).Render());

        // Signature Section

        signatureOuterContainer.Append(new SignatureSection(ReportOptions).Render());

        //

        signatureOuterContainer.Append("</div>");

        return signatureOuterContainer.ToString();
    }
}
