using System.ComponentModel.DataAnnotations;

namespace ValueObjects
{
    public record CompanyContactInfo : DbValueObject
    {
        [MaxLength(Fields.FieldLengths.Project.CompanyContactInfoName)]
        public string Name { get; protected init; }

        public Url Url { get; protected init; }

        public EmailAddress Email { get; protected init; }

        public PhoneNumber Phone { get; protected init; }
    }
}
