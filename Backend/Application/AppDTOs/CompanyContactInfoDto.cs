using ValueObjects;

namespace AppDTOs;

public class CompanyContactInfoDto
{
    public CompanyContactInfoDto(string name, string url, EmailAddressDto email, PhoneNumberDto phone)
    {
        Name = name;
        Url = url;
        Email = email;
        Phone = phone;
    }

    public string Name { get; set; }
    public string Url { get; set; }
    public EmailAddressDto Email { get; set; }
    public PhoneNumberDto Phone { get; set; }

    public CompanyContactInfo ToValueObject()
    {
        return new CompanyContactInfo(
            Name,
            new Url(Url),
            Email.ToValueObject(),
            Phone.ToValueObject()
        );
    }
}
