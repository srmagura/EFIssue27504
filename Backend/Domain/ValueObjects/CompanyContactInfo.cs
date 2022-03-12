using System.ComponentModel.DataAnnotations;
using ITI.Baseline.Util;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Domain;

namespace ValueObjects
{
    public record CompanyContactInfo : DbValueObject
    {
#nullable disable
        [Obsolete("Entity Framework only.")]
        protected CompanyContactInfo() { }
#nullable enable

        public CompanyContactInfo(string name, Url url, EmailAddress email, PhoneNumber phone)
        {
            Require.HasValue(name, "Name is required.");
            Require.NotNull(url, "URL is required.");
            Require.NotNull(email, "Email is required.");
            Require.NotNull(phone, "Phone is required.");

            Name = name;
            Url = url;
            Email = email;
            Phone = phone;
        }

        [MaxLength(Fields.FieldLengths.Project.CompanyContactInfoName)]
        public string Name { get; protected init; }

        public Url Url { get; protected init; }

        public EmailAddress Email { get; protected init; }

        public PhoneNumber Phone { get; protected init; }
    }
}
