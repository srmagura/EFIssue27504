using System.ComponentModel.DataAnnotations;
using Fields;

namespace ValueObjects;

public record PersonName : DbValueObject
{
    [MaxLength(FieldLengths.PersonName.First)]
    public string First { get; protected init; }

    [MaxLength(FieldLengths.PersonName.Last)]
    public string Last { get; protected init; }

    public override string ToString()
    {
        return $"{First} {Last}";
    }
}
