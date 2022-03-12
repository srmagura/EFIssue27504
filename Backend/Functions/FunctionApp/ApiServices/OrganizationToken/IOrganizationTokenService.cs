namespace FunctionApp.ApiServices.OrganizationToken
{
    public interface IOrganizationTokenService
    {
        Task<string> CreateTokenAsync(string organizationShortName);

        OrganizationId? ValidateToken(string token);
    }
}
