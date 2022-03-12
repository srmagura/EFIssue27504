using ITI.Baseline.Util;
using ITI.DDD.Domain;
using Settings;

namespace ValueObjects;

public record ProjectBudgetOptions : DbValueObject
{
    [Obsolete("Serialization only", true)]
    protected ProjectBudgetOptions() { }

    public ProjectBudgetOptions(
        Percentage costAdjustment,
        Percentage depositPercentage,
        bool showPricingInBudgetBreakdown,
        bool showPricePerSquareFoot
    )
    {
        Require.NotNull(costAdjustment, "Cost adjustment must be provided.");
        Require.NotNull(depositPercentage, "Deposit percentage must be provided.");
        Require.IsTrue(depositPercentage >= 0m && depositPercentage <= 100m, "Deposit percentage must be between 0% and 100%.");

        CostAdjustment = costAdjustment;
        DepositPercentage = depositPercentage;
        ShowPricingInBudgetBreakdown = showPricingInBudgetBreakdown;
        ShowPricePerSquareFoot = showPricePerSquareFoot;
    }

    public Percentage CostAdjustment { get; protected init; }
    public Percentage DepositPercentage { get; protected init; }

    public bool ShowPricingInBudgetBreakdown { get; protected init; }
    public bool ShowPricePerSquareFoot { get; protected init; }

    public static ProjectBudgetOptions GetDefault(ProjectSettings projectSettings)
        => new(
            costAdjustment: new Percentage(0m),
            depositPercentage: new Percentage(projectSettings.DefaultDepositPercentage),
            showPricingInBudgetBreakdown: true,
            showPricePerSquareFoot: true
        );
}
