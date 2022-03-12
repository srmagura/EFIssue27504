using Enumerations;
using ITI.Baseline.Passwords;
using ITI.Baseline.ValueObjects;
using ValueObjects;

namespace Entities
{
    public class User : AggregateRoot
    {
        [Obsolete("Serialization Only", true)]
        public User() { }

        public User(OrganizationId organizationId, EmailAddress email, PersonName name, UserRole role, EncodedPassword encodedPassword)
        {
            Require.NotNull(organizationId, "Organization");
            OrganizationId = organizationId;

            Require.NotNull(email, "Email");
            Email = email;

            SetName(name);

            IsActive = true;

            SetPassword(encodedPassword);

            SetRole(role);
        }

        public UserId Id { get; protected set; } = new UserId();
        public bool IsActive { get; protected set; }

        public OrganizationId OrganizationId { get; protected set; }

        public EmailAddress Email { get; protected set; }
        public PersonName Name { get; protected set; }
        public UserRole Role { get; protected set; }

        public EncodedPassword? EncodedPassword { get; protected set; }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetName(PersonName name)
        {
            Require.NotNull(name, "Name");
            Name = name;
        }

        public void SetPassword(EncodedPassword encodedPassword)
        {
            EncodedPassword = encodedPassword;
        }

        public void SetRole(UserRole role)
        {
            Role = role;
        }
    }
}
