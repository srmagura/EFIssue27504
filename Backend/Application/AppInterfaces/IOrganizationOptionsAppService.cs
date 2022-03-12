namespace AppInterfaces;

public interface IOrganizationOptionsAppService
{
    Task<string?> GetDefaultProjectDescriptionAsync(OrganizationId organizationId);
    Task SetDefaultProjectDescriptionAsync(OrganizationId organizationId, string description);

    Task<string?> GetNotForConstructionDisclaimerTextAsync(OrganizationId organizationId);
    Task SetNotForConstructionDisclaimerTextAsync(OrganizationId organizationId, string disclaimer);
}
