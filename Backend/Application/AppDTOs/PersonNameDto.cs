using ValueObjects;

namespace AppDTOs
{
    public class PersonNameDto
    {
        public PersonNameDto(string first, string last)
        {
            First = first;
            Last = last;
        }

        public string First { get; set; }
        public string Last { get; set; }

        public override string ToString()
        {
            return $"{First} {Last}";
        }

        public PersonName ToValueObject()
        {
            return new PersonName(First, Last);
        }
    }
}
