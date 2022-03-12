using FunctionApp.ApiServices.Exceptions;
using Microsoft.AspNetCore.DataProtection;
using Settings;

namespace FunctionApp.ApiServices.OrganizationToken
{
    public class OrganizationTokenService : IOrganizationTokenService
    {
        private readonly ApiAuthenticationSettings _apiAuthenticationSettings;
        private readonly IOrganizationAppService _organizationAppService;
        private readonly IDataProtector _dataProtector;

        public OrganizationTokenService(
            ApiAuthenticationSettings apiAuthenticationSettings,
            IOrganizationAppService organizationAppService,
            IDataProtectionProvider dataProtectionProvider
        )
        {
            _apiAuthenticationSettings = apiAuthenticationSettings;
            _organizationAppService = organizationAppService;
            _dataProtector = dataProtectionProvider.CreateProtector("OrganizationToken");
        }

        public async Task<string> CreateTokenAsync(string organizationShortName)
        {
            // Will throw if the user does not have permission to view this organization
            var organization = await _organizationAppService.GetByShortNameAsync(organizationShortName);

            if (organization == null)
                throw new UserPresentableException("The requested organization does not exist.");

            var expires = DateTimeOffset.UtcNow.AddDays(_apiAuthenticationSettings.AccessTokenExpiresAfterDays);
            var data = organization.Id.Guid + "|" + expires.Ticks;
            return _dataProtector.Protect(data);
        }

        public OrganizationId? ValidateToken(string token)
        {
            var data = _dataProtector.Unprotect(token);
            var split = (data ?? "").Split('|');

            if (split.Length == 2 &&
                Guid.TryParse(split[0], out var guid) &&
                long.TryParse(split[1], out var ticks) &&
                new DateTimeOffset(ticks, TimeSpan.Zero) > DateTimeOffset.UtcNow)
            {
                return new OrganizationId(guid);
            }

            return null;
        }
    }
}
