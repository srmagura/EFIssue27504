using AppDTOs.Enumerations;
using ITI.Baseline.Util;
using ITI.Baseline.ValueObjects;

namespace DataInterfaces.Queries
{
    public interface IUserQueries
    {
        Task<UserDto?> GetAsync(UserId id);

        Task<FilteredList<UserSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );

        Task<bool> EmailIsAvailableAsync(EmailAddress email);
    }
}
