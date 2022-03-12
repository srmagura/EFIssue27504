namespace ValueObjects;

public record Url : DbValueObject
{
    public string Value { get; protected init; }

    public override string ToString()
    {
        return Value;
    }
}
