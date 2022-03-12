namespace DataInterfaces.Repositories;

public interface IOrganizationOptionsRepository
{
    Task<DefaultProjectDescription?> GetDefaultProjectDescriptionAsync(OrganizationId organizationId);

    Task<NotForConstructionDisclaimer?> GetNotForConstructionDisclaimerAsync(OrganizationId organizationId);

    void Add(DefaultProjectDescription defaultProjectDescription);

    void Add(NotForConstructionDisclaimer disclaimer);
}
