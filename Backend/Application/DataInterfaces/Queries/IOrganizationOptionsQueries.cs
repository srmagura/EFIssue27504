namespace DataInterfaces.Queries;

public interface IOrganizationOptionsQueries
{
    Task<string?> GetDefaultProjectDescriptionAsync(OrganizationId organizationId);

    Task<string?> GetNotForConstructionDisclaimerTextAsync(OrganizationId organizationId);
}
