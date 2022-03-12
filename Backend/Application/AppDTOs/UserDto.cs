namespace AppDTOs
{
    public class UserDto
    {
        public UserDto(
            UserId id,
            OrganizationReferenceDto organization,
            EmailAddressDto email,
            PersonNameDto name,
            UserRole role,
            bool isActive
        )
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Role = role;
            IsActive = isActive;
        }

        public UserId Id { get; set; }

        public OrganizationReferenceDto Organization { get; set; }

        public EmailAddressDto Email { get; set; }
        public PersonNameDto Name { get; set; }
        public UserRole Role { get; set; }

        public bool IsActive { get; set; }
    }
}
