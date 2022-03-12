using ITI.DDD.Core;
using ValueObjects;

namespace AppDTOs
{
    public class PartialAddressDto
    {
        public PartialAddressDto() { }

        public PartialAddressDto(string? line1, string? line2, string? city, string? state, string? postalCode)
        {
            Line1 = line1;
            Line2 = line2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        public PartialAddress ToValueObject()
        {
            return new PartialAddress(
                Line1,
                Line2,
                City,
                State,
                PostalCode.HasValue() ? new PostalCode(PostalCode) : null
            );
        }
    }
}
