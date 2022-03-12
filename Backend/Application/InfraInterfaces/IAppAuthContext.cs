using Enumerations;
using Identities;
using ITI.DDD.Auth;

namespace InfraInterfaces
{
    public interface IAppAuthContext : IAuthContext
    {
        public bool IsSystemProcess { get; }

        OrganizationId? OrganizationId { get; }
        UserId? UserId { get; }
        UserRole? Role { get; }
    }
}
