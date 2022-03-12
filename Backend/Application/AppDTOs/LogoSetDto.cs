namespace AppDTOs;

public class LogoSetDto
{
    public LogoSetDto(
        LogoSetId id,
        OrganizationId organizationId,
        string name,
        FileRefDto darkLogo,
        FileRefDto lightLogo
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        DarkLogo = darkLogo ?? throw new ArgumentNullException(nameof(darkLogo));
        LightLogo = lightLogo ?? throw new ArgumentNullException(nameof(lightLogo));
    }

    public LogoSetId Id { get; set; }
    public OrganizationId OrganizationId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public FileRefDto DarkLogo { get; set; }

    public FileRefDto LightLogo { get; set; }
}
