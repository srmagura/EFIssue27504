using Enumerations;
using InfraInterfaces;

namespace Permissions
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IAppAuthContext _auth;

        public UserRoleService(IAppAuthContext auth)
        {
            _auth = auth;
        }

        public HashSet<UserRole> GetGrantableRoles(bool targetUserIsInHostOrganization)
        {
            var roles = Enum.GetValues<UserRole>()?.ToHashSet();
            if (roles == null) throw new Exception("This should never happen.");

            if (_auth.IsSystemProcess)
                return roles;

            switch (_auth.Role)
            {
                case UserRole.OrganizationAdmin:
                    roles.Remove(UserRole.SystemAdmin);
                    return roles;
                case UserRole.SystemAdmin:
                    if (!targetUserIsInHostOrganization) roles.Remove(UserRole.SystemAdmin);
                    return roles;
                default:
                    return new HashSet<UserRole>();
            }
        }
    }
}
