using DataContext;

namespace FunctionApp.ApiServices.OrganizationToken
{
    internal class ApiOrganizationContext : IOrganizationContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrganizationTokenService _organizationTokenService;
        private const string HttpContextOrganizationIdKey = "SecuritySession_OrganizationId";

        public ApiOrganizationContext(
            IHttpContextAccessor httpContextAccessor,
            IOrganizationTokenService organizationTokenService
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _organizationTokenService = organizationTokenService;
        }

        // For API requests, the organization token is in a header. For content requests, like
        // loading an image, we cannot set headers on the request so we put the token
        // in the query parameters.
        private static string? GetToken(HttpRequest request)
        {
            var token = request.Headers["Slateplan-Organization-Token"].FirstOrDefault();

            if (token == null)
                token = request.Query["organizationToken"].FirstOrDefault();

            return token;
        }

        public OrganizationId? OrganizationId
        {
            get
            {
                var cachedOrganizationId = _httpContextAccessor.HttpContext.Items[HttpContextOrganizationIdKey] as OrganizationId;
                if (cachedOrganizationId != null) return cachedOrganizationId;

                var token = GetToken(_httpContextAccessor.HttpContext.Request);
                if (token == null) return null;

                var organizationId = _organizationTokenService.ValidateToken(token);
                _httpContextAccessor.HttpContext.Items[HttpContextOrganizationIdKey] = organizationId;

                return organizationId;
            }
        }
    }
}
