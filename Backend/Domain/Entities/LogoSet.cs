using ValueObjects;

namespace Entities;

public class LogoSet : AggregateRoot
{
    public LogoSet(
        OrganizationId organizationId,
        string name,
        FileRef darkLogo,
        FileRef lightLogo
    )
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        SetName(name);
        SetDarkLogo(darkLogo);
        SetLightLogo(lightLogo);
    }

    public LogoSetId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Name { get; protected set; }

    public FileRef DarkLogo { get; protected set; }
    public FileRef LightLogo { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        Require.HasValue(name, "Name is required.");
        Name = name;
    }

    [MemberNotNull(nameof(DarkLogo))]
    public void SetDarkLogo(FileRef darkLogo)
    {
        Require.NotNull(darkLogo, "Dark logo is required.");
        DarkLogo = darkLogo;
    }

    [MemberNotNull(nameof(LightLogo))]
    public void SetLightLogo(FileRef lightLogo)
    {
        Require.NotNull(lightLogo, "Light logo is required.");
        LightLogo = lightLogo;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }
}
