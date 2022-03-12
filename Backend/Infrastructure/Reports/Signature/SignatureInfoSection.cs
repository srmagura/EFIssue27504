using Reports.Shared;

namespace Reports.Signature;

public class SignatureInfoSection : IHtmlBuilder
{
    private const double ContainerWidth = 440;
    private const double ContainerHeight = 60;
    private const double ContainerMarginTop = 13;
    private const double ContainerPaddingX = 40;

    private const double DisclaimerPaddingX = 45;
    private const double DisclaimerWidth = ContainerWidth - (DisclaimerPaddingX * 2);
    private const double DisclaimerHeight = 35;

    private const double BudgetPaddingX = 20;
    private const double BudgetHeight = ContainerHeight - DisclaimerHeight;

    private const double FontBig = 10;
    private const double FontSmall = 9;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public double Budget { get; }
    public double DepositPercentage { get; }

    public SignatureInfoSection(
        double budget,
        double depositPercentage
    )
    {
        Budget = budget;
        DepositPercentage = depositPercentage;
    }

    public string Render()
    {
        var deposit = Math.Floor(Budget * DepositPercentage);
        var percentage = (int)(DepositPercentage * 100);

        return $@"
<div style=""width: {ContainerWidth}pt; height: {ContainerHeight}pt; margin-top: {ContainerMarginTop}pt; padding: 0 {ContainerPaddingX}pt;"">
    <div style=""width: {DisclaimerWidth}pt; height: {DisclaimerHeight}pt; padding: 0 {DisclaimerPaddingX}pt; font-size: {FontSmall}pt;"">
        A design deposit will be invoiced to proceed to the next phase of the project. Phases and services are detailed above. The design deposit will be {percentage}% of the overall budget.
    </div>
    <table style=""width: {ContainerWidth}pt; height: {BudgetHeight}pt; border-bottom: 1px solid black; border-collapse: collapse; font-size: {FontSmall}pt;"">
        <tr>
            <td style=""padding: 0 0 0 {BudgetPaddingX}pt; vertical-align: baseline;"">
                <b style=""font-size: {FontBig}pt;"">BUDGET: </b>
                ${string.Format("{0:n}", Budget)}
            </td>
            <td style=""padding: 0 {BudgetPaddingX}pt 0 0; vertical-align: baseline; text-align: end;"">
                <b style=""font-size: {FontBig}pt;"">DEPOSIT: </b>
                ${string.Format("{0:n}", deposit)}
            </td>
        </tr>
    </table>
</div>
";
    }
}
