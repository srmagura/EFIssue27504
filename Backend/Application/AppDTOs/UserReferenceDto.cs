namespace AppDTOs;

public class UserReferenceDto
{
    public UserReferenceDto(UserId id, EmailAddressDto email, PersonNameDto name)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public UserId Id { get; set; }

    public EmailAddressDto Email { get; set; }
    public PersonNameDto Name { get; set; }
    public UserRole Role { get; set; }
}
