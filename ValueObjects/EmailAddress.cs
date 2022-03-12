using System.ComponentModel.DataAnnotations;

namespace ValueObjects;

public record EmailAddress : DbValueObject
{
    [MaxLength(128)]
    public string Value { get; protected init; }

    public override string ToString()
    {
        return Value;
    }
}
