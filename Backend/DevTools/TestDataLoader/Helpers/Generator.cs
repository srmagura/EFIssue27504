using System.Text.RegularExpressions;
using AppDTOs;

namespace TestDataLoader.Helpers
{
    public class EmailAndName
    {
        public EmailAndName(EmailAddressDto email, PersonNameDto name)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public EmailAddressDto Email { get; set; }
        public PersonNameDto Name { get; set; }
    }

    public static class Generator
    {
        private const int DefaultLength = 1000;

        public static List<string> CompanyNames(int seed, int count = DefaultLength)
        {
            var list = new List<string>();
            Randomizer.Seed = new Random(seed);

            var companySet = new Bogus.DataSets.Company();

            for (var i = 0; i < count; i++)
            {
                list.Add(companySet.CompanyName());
            }

            return list;
        }

        public static List<EmailAndName> EmailsAndNames(int seed, int count = DefaultLength)
        {
            var list = new List<EmailAndName>();
            Randomizer.Seed = new Random(seed);

            var nameSet = new Bogus.DataSets.Name();

            for (var i = 0; i < count; i++)
            {
                var firstName = nameSet.FirstName();
                var lastName = nameSet.LastName();

                var emailFriendlyLastName = Regex.Replace(lastName, "[^a-zA-Z]", "");
                var email = $"{firstName[0]}{emailFriendlyLastName}@example2.com";

                list.Add(new EmailAndName(new EmailAddressDto(email), new PersonNameDto(firstName, lastName)));
            }

            return list;
        }

        public static List<PartialAddressDto> Addresses(int seed, int count = DefaultLength, bool completeAddresses = false)
        {
            var list = new List<PartialAddressDto>();
            Randomizer.Seed = new Random(seed);

            var addressSet = new Bogus.DataSets.Address();
            var randomizer = new Randomizer(seed + 1);

            for (var i = 0; i < count; i++)
            {
                string? line1 = (completeAddresses || randomizer.Bool()) ? addressSet.StreetAddress() : null;
                string? city = (completeAddresses || randomizer.Bool()) ? addressSet.City() : null;
                string? state = (completeAddresses || randomizer.Bool()) ? addressSet.StateAbbr() : null;
                string? postalCode = (completeAddresses || randomizer.Bool()) ? addressSet.ZipCode() : null;

                list.Add(new PartialAddressDto
                {
                    Line1 = line1,
                    Line2 = null,
                    City = city,
                    State = state,
                    PostalCode = postalCode
                });
            }

            return list;
        }
    }
}
