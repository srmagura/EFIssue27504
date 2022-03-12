using Settings;
using ValueObjects;

namespace Entities;

public class Project : AggregateRoot
{
    [Obsolete("AutoMapper only.")]
    protected Project(
        OrganizationId organizationId,
        string name,
        string shortName,
        string description,
        PartialAddress address,
        string customerName,
        ProjectBudgetOptions budgetOptions,
        ProjectReportOptions reportOptions
    )
    {
        OrganizationId = organizationId;
        Name = name;
        ShortName = shortName;
        Description = description;
        Address = address;
        CustomerName = customerName;
        BudgetOptions = budgetOptions;
        ReportOptions = reportOptions;
    }

    public Project(
        OrganizationId organizationId,
        string name,
        string shortName,
        string description,
        PartialAddress address,
        string customerName,
        string signeeName,
        LogoSetId logoSetId,
        TermsDocumentId termsDocumentId,
        string preparerName,
        int estimatedSquareFeet,
        ProjectSettings projectSettings
    )
    {
        OrganizationId = organizationId;

        SetName(name);
        SetShortName(shortName);
        SetDescription(description);

        SetCustomerName(customerName);
        SetEstimatedSquareFeet(estimatedSquareFeet);

        SetAddress(address);

        SetBudgetOptions(ProjectBudgetOptions.GetDefault(projectSettings));
        SetReportOptions(new ProjectReportOptions(signeeName, preparerName, logoSetId, termsDocumentId));
    }

    public ProjectId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Name { get; protected set; }
    public string ShortName { get; protected set; }
    public string Description { get; protected set; }

    public PartialAddress Address { get; protected set; }

    public string CustomerName { get; protected set; }
    public int EstimatedSquareFeet { get; protected set; }

    public FileRef? Photo { get; protected set; }

    public ProjectBudgetOptions BudgetOptions { get; protected set; }
    public ProjectReportOptions ReportOptions { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    public UserId? DesignerLockedById { get; protected set; }
    public DateTimeOffset? DesignerLockedUtc { get; protected set; }

    public void AcquireDesignerLock(UserId userId)
    {
        Require.NotNull(userId, "User ID is required.");
        DesignerLockedById = userId;
        DesignerLockedUtc = DateTimeOffset.UtcNow;
    }

    public void ReleaseDesignerLock()
    {
        DesignerLockedById = null;
        DesignerLockedUtc = null;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        Require.HasValue(name, "Name is required.");
        Name = name;
    }

    [MemberNotNull(nameof(ShortName))]
    public void SetShortName(string shortName)
    {
        Require.HasValue(shortName, "Short name is required.");
        ShortName = shortName;
    }

    [MemberNotNull(nameof(Description))]
    public void SetDescription(string description)
    {
        Require.HasValue(description, "Description is required.");
        Description = description;
    }

    [MemberNotNull(nameof(CustomerName))]
    public void SetCustomerName(string customerName)
    {
        Require.HasValue(customerName, "Customer name is required.");
        CustomerName = customerName;
    }

    public void SetEstimatedSquareFeet(int estimatedSquareFeet)
    {
        Require.IsTrue(estimatedSquareFeet > 0, "Estimated square feet must be greater than 0.");
        EstimatedSquareFeet = estimatedSquareFeet;
    }

    public void SetPhoto(FileRef? photo)
    {
        Photo = photo;
    }

    [MemberNotNull(nameof(Address))]
    public void SetAddress(PartialAddress address)
    {
        Require.NotNull(address, "Address is required.");
        Address = address;
    }

    [MemberNotNull(nameof(BudgetOptions))]
    public void SetBudgetOptions(ProjectBudgetOptions budgetOptions)
    {
        Require.NotNull(budgetOptions, "Budget options are required.");
        BudgetOptions = budgetOptions;
    }

    [MemberNotNull(nameof(ReportOptions))]
    public void SetReportOptions(ProjectReportOptions reportOptions)
    {
        Require.NotNull(reportOptions, "Report options are required.");

        if (ReportOptions?.CompanyContactInfo != null)
            Require.NotNull(reportOptions.CompanyContactInfo, "Company contact info cannot be cleared once it is set.");

        ReportOptions = reportOptions;
    }
}
