using Enumerations;
using Identities;
using InfraInterfaces;

namespace AppConfig
{
    public class SystemAuthContext : IAppAuthContext
    {
        public bool IsAuthenticated => true;
        public string UserIdString => "SYSTEM";
        public string UserName => "SYSTEM";

        public bool IsSystemProcess => true;

        public OrganizationId? OrganizationId => null;
        public UserId? UserId => null;
        public UserRole? Role => null;
    }
}
