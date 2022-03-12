using System.ComponentModel.DataAnnotations;

namespace ValueObjects;

public record PhoneNumber : DbValueObject
{
    [MaxLength(16)]
    public string Value { get; protected set; }
}
