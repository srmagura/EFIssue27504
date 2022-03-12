using ITI.Baseline.ValueObjects;

namespace AppDTOs
{
    public class PhoneNumberDto
    {
        public PhoneNumberDto(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public PhoneNumber ToValueObject()
        {
            return new PhoneNumber(Value);
        }
    }
}
