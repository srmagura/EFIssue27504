using ITI.Baseline.Util;
using ITI.DDD.Domain;

namespace ValueObjects;

public record Url : DbValueObject
{
    public string Value { get; protected init; }

    public Url(string value)
    {
        Require.HasValue(value, "URL cannot be empty.");
        Require.IsTrue(
            value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            value.StartsWith("https://", StringComparison.OrdinalIgnoreCase),
            "URL must start with http:// or https://."
        );
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}
