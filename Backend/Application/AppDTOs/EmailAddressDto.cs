using ITI.Baseline.ValueObjects;

namespace AppDTOs
{
    public class EmailAddressDto
    {
        public EmailAddressDto(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public EmailAddress ToValueObject()
        {
            return new EmailAddress(Value);
        }
    }
}
