using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DataInterfaces.Queries;

public interface IProjectQueries
{
    Task<ProjectDto?> GetAsync(ProjectId id);
    Task<UserId?> GetDesignerLockedByIdAsync(ProjectId id);

    Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);

    Task<FilteredList<ProjectSummaryDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    );
}
