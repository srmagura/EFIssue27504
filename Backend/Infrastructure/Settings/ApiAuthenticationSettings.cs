using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Settings
{
    public class ApiAuthenticationSettings
    {
        // All access tokens (auth cookies) will be invalidated if you change this key. For security,
        // must use a different signing key in development, test, and production.
        public string TokenSigningKey { get; set; } = "eQ8aTuThdAJNGtuRVyY4JZ";
        public SecurityKey TokenSecurityKey =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenSigningKey));

        public string TokenAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;

        public string AccessTokenCookieName { get; set; } = "slateplanAccessToken";
        public double AccessTokenExpiresAfterDays { get; set; } = 16;

        // After this many days, the UI should redirect the user to the log in page
        // **when they open the app** but not if they already have the app open. This
        // is to reduce the chance that the access token expires while the user is in
        // the middle of something.
        public double AccessTokenSoftExpiresAfterDays { get; set; } = 14;
    }
}
