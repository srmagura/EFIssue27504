namespace ValueObjects;

public record ProjectBudgetOptions : DbValueObject
{
    public Percentage CostAdjustment { get; protected init; }
    public Percentage DepositPercentage { get; protected init; }

    public bool ShowPricingInBudgetBreakdown { get; protected init; }
    public bool ShowPricePerSquareFoot { get; protected init; }
}
