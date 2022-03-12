using System.ComponentModel.DataAnnotations;
using Fields;

namespace ValueObjects
{
    public record PartialAddress : DbValueObject
    {
        public PartialAddress() { }

        public PartialAddress(string? line1, string? line2, string? city, string? state, PostalCode? postalCode)
        {
            Line1 = line1;
            Line2 = line2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        // So EF can differentiate between a null PartialAddress and a PartialAddress
        // with all properties equal to null
        public bool? HasValue { get; protected init; } = true;

        [MaxLength(FieldLengths.Address.Line1)]
        public string? Line1 { get; protected init; }

        [MaxLength(FieldLengths.Address.Line2)]
        public string? Line2 { get; protected init; }

        [MaxLength(FieldLengths.Address.City)]
        public string? City { get; protected init; }

        [MaxLength(FieldLengths.Address.State)]
        public string? State { get; protected init; }

        public PostalCode? PostalCode { get; protected init; }
    }
}
