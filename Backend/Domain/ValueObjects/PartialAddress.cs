using System.ComponentModel.DataAnnotations;
using Fields;
using ITI.DDD.Core;
using ITI.DDD.Domain;

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

        public override string ToString()
        {
            var s = "";

            AddPart(ref s, Line1);
            AddPart(ref s, Line2);
            AddPart(ref s, City);
            AddPart(ref s, $"{State} {PostalCode?.Value}");

            return s.Replace("  ", " ").Trim();
        }

        public string? GetCityStateString()
        {
            if (!City.HasValue() || !State.HasValue()) return null;
            return $"{City}, {State}";
        }

        private static void AddPart(ref string s, string? value)
        {
            if (!value.HasValue())
                return;

            if (s.HasValue())
                s += ", ";

            s += value;
        }
    }
}
