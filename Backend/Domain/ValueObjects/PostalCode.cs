using System.ComponentModel.DataAnnotations;
using ITI.Baseline.Util;
using ITI.DDD.Core;
using ITI.DDD.Domain;

namespace ValueObjects;

public record PostalCode : DbValueObject
{
    public PostalCode(string value)
    {
        Require.HasValue(value, "Postal code is required.");

        Value = LettersAndNumbersOnly(value.Trim());

        if (IsAllDigits(Value))
            Value = Value.MaxLength(5); // strip "+4" formatted US zips

        Require.IsTrue(IsValidUsZip(Value) || IsValidCanadianPostalCode(Value), "Invalid postal code format.");
    }

    private static bool IsAllDigits(string value)
    {
        return value.All(char.IsDigit);
    }

    private static string LettersAndNumbersOnly(string value)
    {
        if (value == null)
            return "";

        var arr = value.Where(p => char.IsLetter(p) || char.IsNumber(p)).ToArray();
        return new string(arr);
    }

    //

    [MaxLength(16)]
    public string Value { get; protected init; }

    //

    private static bool IsValidCanadianPostalCode(string value)
    {
        if (value == null)
            return false;

        if (value.Length != 6)
            return false;

        if (char.IsLetter(value[0]) && char.IsNumber(value[1])
            && char.IsLetter(value[2]) && char.IsNumber(value[3])
            && char.IsLetter(value[4]) && char.IsNumber(value[5])
        )
            return true;

        return false;
    }

    private static bool IsValidUsZip(string value)
    {
        if (value == null)
            return false;

        if (value.Length != 5 && value.Length != 9)
            return false;

        if (value.All(char.IsNumber))
            return true;

        return false;
    }

    public override string ToString()
    {
        return Value;
    }
}
