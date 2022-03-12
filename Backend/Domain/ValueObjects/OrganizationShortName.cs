using System.ComponentModel.DataAnnotations;
using Fields;
using ITI.Baseline.Util;
using ITI.DDD.Domain;

namespace ValueObjects;

public record OrganizationShortName : DbValueObject
{
    public OrganizationShortName(string value)
    {
        Require.HasValue(value, "Short name is required.");
        Require.IsTrue(IsValid(value), "Short name must contain only letters, numbers, period, and hyphen.");

        Value = value;
    }

    // To prevent routing issues on frontend
    private static readonly List<string> IllegalValues = new()
    {
        "home",
        "api"
    };

    public static bool IsValid(string value)
    {
        if (IllegalValues.Any(v => v.Equals(value, StringComparison.OrdinalIgnoreCase)))
            return false;

        return value.All(IsValidChar);
    }

    private static bool IsValidChar(char ch)
    {
        return char.IsLetterOrDigit(ch) || ch == '-' || ch == '.';
    }

    [MaxLength(FieldLengths.Organization.ShortName)]
    public string Value { get; protected init; }

    public override string ToString()
    {
        return Value;
    }
}
