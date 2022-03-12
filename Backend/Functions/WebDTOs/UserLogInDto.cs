namespace WebDTOs
{
    public class UserLogInDto
    {
        public UserLogInDto(string accessToken, DateTimeOffset expiresUtc, DateTimeOffset softExpiresUtc)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            ExpiresUtc = expiresUtc;
            SoftExpiresUtc = softExpiresUtc;
        }

        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresUtc { get; set; }
        public DateTimeOffset SoftExpiresUtc { get; set; }
    }
}
