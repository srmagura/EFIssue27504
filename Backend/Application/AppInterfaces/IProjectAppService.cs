using ITI.Baseline.Util;

namespace AppInterfaces;

public interface IProjectAppService
{
    Task<ProjectDto?> GetAsync(ProjectId id);
    Task<string?> GetPhotoAsync(ProjectId id, Stream outputStream); // returns the image file type

    Task<FilteredList<ProjectSummaryDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    );

    Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);
    Task<ProjectId> AddAsync(
        OrganizationId organizationId,
        string name,
        string shortName,
        string description,
        PartialAddressDto address,
        string customerName,
        string signeeName,
        LogoSetId logoSetId,
        TermsDocumentId termsDocumentId,
        int estimatedSquareFeet
    );

    Task SetNameAsync(ProjectId id, string name, string shortName);
    Task SetDescriptionAsync(ProjectId id, string description);
    Task SetCustomerNameAsync(ProjectId id, string customerName);
    Task SetEstimatedSquareFeetAsync(ProjectId id, int estimatedSquareFeet);
    Task SetPhotoAsync(ProjectId id, Stream? stream, string? fileType);
    Task SetActiveAsync(ProjectId id, bool active);
    Task SetAddressAsync(ProjectId id, PartialAddressDto address);

    Task SetBudgetOptionsAsync(ProjectId id, ProjectBudgetOptionsDto budgetOptions);
    Task SetReportOptionsAsync(ProjectId id, ProjectReportOptionsDto reportOptions);

    Task AcquireDesignerLockAsync(ProjectId id);
    Task ReleaseDesignerLockAsync(ProjectId id);
}
