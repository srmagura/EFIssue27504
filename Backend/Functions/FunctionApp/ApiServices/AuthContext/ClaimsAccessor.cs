using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Settings;

namespace FunctionApp.ApiServices.AuthContext
{
    internal class ClaimsAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiAuthenticationSettings _apiAuthenticationSettings;

        public ClaimsAccessor(
            IHttpContextAccessor httpContextAccessor,
            ApiAuthenticationSettings apiAuthenticationSettings
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _apiAuthenticationSettings = apiAuthenticationSettings;
        }

        private IReadOnlyList<Claim> ParseAccessToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidAlgorithms = new[] { _apiAuthenticationSettings.TokenAlgorithm },
                IssuerSigningKey = _apiAuthenticationSettings.TokenSecurityKey,

                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
            };

            var validationResult = new JsonWebTokenHandler().ValidateToken(accessToken, tokenValidationParameters);

            if (!validationResult.IsValid)
                return new List<Claim>();

            return validationResult.ClaimsIdentity.Claims.ToList();
        }

        private IReadOnlyList<Claim> DetermineClaims()
        {
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies;
            var accessToken = cookies[_apiAuthenticationSettings.AccessTokenCookieName];

            if (accessToken == null) return new List<Claim>();

            return ParseAccessToken(accessToken);
        }

        private IReadOnlyList<Claim>? _claims;

        internal IReadOnlyList<Claim> Claims
        {
            get
            {
                if (_claims == null)
                    _claims = DetermineClaims();

                return _claims;
            }
        }
    }
}
