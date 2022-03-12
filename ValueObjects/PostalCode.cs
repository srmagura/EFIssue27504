using System.ComponentModel.DataAnnotations;

namespace ValueObjects;

public record PostalCode : DbValueObject
{
    [MaxLength(16)]
    public string Value { get; protected init; }

    public override string ToString()
    {
        return Value;
    }
}
