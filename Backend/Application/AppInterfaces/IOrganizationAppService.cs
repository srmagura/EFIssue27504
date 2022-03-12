using ITI.Baseline.Util;

namespace AppInterfaces
{
    public interface IOrganizationAppService
    {
        Task<OrganizationDto?> GetAsync(OrganizationId id);
        Task<OrganizationDto?> GetByShortNameAsync(string shortName);

        Task<bool> NameIsAvailableAsync(string name);
        Task<bool> ShortNameIsAvailableAsync(string shortName);

        Task<FilteredList<OrganizationSummaryDto>> ListAsync(
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );

        Task<OrganizationId> AddAsync(
            string name,
            string shortName,
            EmailAddressDto ownerEmail,
            PersonNameDto ownerName,
            string ownerPassword
        );

        Task SetActiveAsync(OrganizationId id, bool active);
        Task SetNameAsync(OrganizationId id, string name);
        Task SetShortNameAsync(OrganizationId id, string shortName);
    }
}
