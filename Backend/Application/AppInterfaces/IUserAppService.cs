using ITI.Baseline.Util;

namespace AppInterfaces
{
    public interface IUserAppService
    {
        Task<UserDto?> GetAsync(UserId id);

        Task<FilteredList<UserSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        );

        Task<bool> UserEmailIsAvailableAsync(EmailAddressDto email);

        Task<UserId> AddAsync(
            OrganizationId organizationId,
            EmailAddressDto email,
            PersonNameDto name,
            UserRole role,
            string password
        );
        Task SetNameAsync(UserId id, PersonNameDto name);
        Task SetActiveAsync(UserId id, bool active);
        Task SetRoleAsync(UserId id, UserRole role);

        Task<LogInResultDto> LogInAsync(EmailAddressDto email, string password);

        Task ChangePasswordAsync(UserId id, string currentPassword, string newPassword);
        Task SetPasswordAsync(UserId id, string newPassword);
    }
}
