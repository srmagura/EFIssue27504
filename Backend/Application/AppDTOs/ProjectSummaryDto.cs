namespace AppDTOs;

public class ProjectSummaryDto
{
    public ProjectSummaryDto(
        ProjectId id,
        string name,
        string shortName,
        PartialAddressDto address,
        string customerName
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
    }

    public ProjectId Id { get; set; }

    public string Name { get; set; }
    public string ShortName { get; set; }

    public PartialAddressDto Address { get; set; }

    public string CustomerName { get; set; }

    public bool IsActive { get; set; }
}
