namespace AppDTOs;

public class ProjectDto
{
    public ProjectDto(
        ProjectId id,
        OrganizationId organizationId,
        string name,
        string shortName,
        string description,
        PartialAddressDto address,
        string customerName,
        ProjectBudgetOptionsDto budgetOptions,
        ProjectReportOptionsDto reportOptions
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
        BudgetOptions = budgetOptions ?? throw new ArgumentNullException(nameof(budgetOptions));
        ReportOptions = reportOptions ?? throw new ArgumentNullException(nameof(reportOptions));
    }

    public ProjectId Id { get; set; }

    public OrganizationId OrganizationId { get; set; }

    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }

    public PartialAddressDto Address { get; set; }

    public string CustomerName { get; set; }
    public int EstimatedSquareFeet { get; set; }

    public FileRefDto? Photo { get; set; }

    public ProjectBudgetOptionsDto BudgetOptions { get; set; }
    public ProjectReportOptionsDto ReportOptions { get; set; }

    public bool IsActive { get; set; }

    public UserReferenceDto? DesignerLockedBy { get; set; }
    public DateTimeOffset? DesignerLockedUtc { get; set; }
}
