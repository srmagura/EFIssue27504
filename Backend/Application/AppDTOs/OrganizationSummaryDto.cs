namespace AppDTOs;

public class OrganizationSummaryDto
{
    public OrganizationSummaryDto(OrganizationId id, string name, string shortName)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
    }

    public OrganizationId Id { get; set; }

    public string Name { get; set; }
    public string ShortName { get; set; }

    public bool IsHost { get; set; }
    public bool IsActive { get; set; }
}
