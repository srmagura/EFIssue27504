using ValueObjects;

namespace Entities;

public class Organization : AggregateRoot
{
    [Obsolete("Serialization Only", true)]
    public Organization() { }

    public Organization(string name, OrganizationShortName shortName)
    {
        SetName(name);
        SetShortName(shortName);

        IsActive = true;
    }

    public OrganizationId Id { get; protected set; } = new OrganizationId();

    public bool IsActive { get; protected set; }

    public string Name { get; protected set; }
    public OrganizationShortName ShortName { get; protected set; }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    public void SetName(string name)
    {
        Require.HasValue(name, "Name is required.");
        Name = name;
    }

    public void SetShortName(OrganizationShortName shortName)
    {
        Require.NotNull(shortName, "Short Name");
        ShortName = shortName;
    }
}
