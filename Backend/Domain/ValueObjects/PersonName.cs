using System.ComponentModel.DataAnnotations;
using Fields;
using ITI.Baseline.Util;
using ITI.DDD.Domain;

namespace ValueObjects;

public record PersonName : DbValueObject
{
    public PersonName(string first, string last)
    {
        Require.HasValue(first, "First name is required.");
        First = first;

        Require.HasValue(first, "Last name is required.");
        Last = last;
    }

    [MaxLength(FieldLengths.PersonName.First)]
    public string First { get; protected init; }

    [MaxLength(FieldLengths.PersonName.Last)]
    public string Last { get; protected init; }

    public override string ToString()
    {
        return $"{First} {Last}";
    }
}
