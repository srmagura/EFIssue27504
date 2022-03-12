using System.Security.Claims;
using DataContext;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FunctionApp.ApiServices.AuthContext
{
    internal class ApiAuthContext : IAppAuthContext
    {
        private readonly ClaimsAccessor _claimsAccessor;

        public ApiAuthContext(ClaimsAccessor claimsAccessor)
        {
            _claimsAccessor = claimsAccessor;
        }

        private IReadOnlyList<Claim> Claims => _claimsAccessor.Claims;

        private Guid? GetGuid(string claimType)
        {
            var idString = Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            if (idString == null) return null;

            try
            {
                return Guid.Parse(idString);
            }
            catch
            {
                return null;
            }
        }

        public bool IsSystemProcess => SystemSecurityScope.IsSystem;
        public bool IsAuthenticated => UserIdString != null;
        public string? UserIdString => Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        public string? UserName => Claims.FirstOrDefault(c => c.Type == SlateplanClaimTypes.UserName)?.Value;

        public OrganizationId? OrganizationId
        {
            get
            {
                var guid = GetGuid(SlateplanClaimTypes.OrganizationId);
                return guid != null ? new OrganizationId(guid) : null;
            }
        }

        public UserId? UserId
        {
            get
            {
                var guid = GetGuid(JwtRegisteredClaimNames.Sub);
                return guid != null ? new UserId(guid) : null;
            }
        }

        public UserRole? Role
        {
            get
            {
                var roleString = Claims.FirstOrDefault(c => c.Type == SlateplanClaimTypes.Role)?.Value;
                if (roleString == null) return null;

                try
                {
                    return Enum.Parse<UserRole>(roleString);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
