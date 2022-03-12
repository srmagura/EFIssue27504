using Enumerations;

namespace Permissions
{
    public interface IUserRoleService
    {
        HashSet<UserRole> GetGrantableRoles(bool targetUserIsInHostOrganization);
    }
}
