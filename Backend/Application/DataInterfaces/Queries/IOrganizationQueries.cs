using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DataInterfaces.Queries
{
    public interface IOrganizationQueries
    {
        Task<OrganizationDto?> GetAsync(OrganizationId id);
        Task<OrganizationDto?> GetByShortNameAsync(string shortName);

        Task<bool> IsActiveAsync(OrganizationId organizationId);

        Task<bool> NameIsAvailableAsync(string name);
        Task<bool> ShortNameIsAvailableAsync(string shortName);

        Task<FilteredList<OrganizationSummaryDto>> ListAsync(
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );
    }
}
