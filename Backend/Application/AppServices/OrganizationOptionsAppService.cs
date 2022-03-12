namespace AppServices;

public class OrganizationOptionsAppService : ApplicationService, IOrganizationOptionsAppService
{
    private readonly IAppPermissions _perms;
    private readonly IOrganizationOptionsQueries _queries;
    private readonly IOrganizationOptionsRepository _repo;

    public OrganizationOptionsAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IOrganizationOptionsQueries queries,
        IOrganizationOptionsRepository repo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
    }

    public Task<string?> GetDefaultProjectDescriptionAsync(OrganizationId organizationId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewOrganizationOptionsAsync(organizationId)),
            () => _queries.GetDefaultProjectDescriptionAsync(organizationId)
        );
    }

    public Task<string?> GetNotForConstructionDisclaimerTextAsync(OrganizationId organizationId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewOrganizationOptionsAsync(organizationId)),
            () => _queries.GetNotForConstructionDisclaimerTextAsync(organizationId)
        );
    }

    public Task SetDefaultProjectDescriptionAsync(OrganizationId organizationId, string description)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageOrganizationOptionsAsync(organizationId)),
            async () =>
            {
                var defaultProjectDescription = await _repo.GetDefaultProjectDescriptionAsync(organizationId);

                if (defaultProjectDescription != null)
                {
                    defaultProjectDescription.SetText(description);
                }
                else
                {
                    defaultProjectDescription = new DefaultProjectDescription(organizationId, description);
                    _repo.Add(defaultProjectDescription);
                }
            }
        );
    }

    public Task SetNotForConstructionDisclaimerTextAsync(OrganizationId organizationId, string disclaimer)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageOrganizationOptionsAsync(organizationId)),
            async () =>
            {
                var disclaimerEntity = await _repo.GetNotForConstructionDisclaimerAsync(organizationId);

                if (disclaimerEntity != null)
                {
                    disclaimerEntity.SetText(disclaimer);
                }
                else
                {
                    disclaimerEntity = new NotForConstructionDisclaimer(organizationId, disclaimer);
                    _repo.Add(disclaimerEntity);
                }
            }
        );
    }
}
