namespace ValueObjects;

public record TypicalBudgetRange
{
    public TypicalBudgetRange(Money low, Money median, Money high)
    {
        Low = low ?? throw new ArgumentNullException(nameof(low));
        Median = median ?? throw new ArgumentNullException(nameof(median));
        High = high ?? throw new ArgumentNullException(nameof(high));
    }

    public Money Low { get; protected init; }
    public Money Median { get; protected init; }
    public Money High { get; protected init; }
}
