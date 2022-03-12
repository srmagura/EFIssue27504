using System.ComponentModel.DataAnnotations;
using Fields;

namespace ValueObjects;

public record OrganizationShortName : DbValueObject
{
    [MaxLength(FieldLengths.Organization.ShortName)]
    public string Value { get; protected init; }

    public override string ToString()
    {
        return Value;
    }
}
