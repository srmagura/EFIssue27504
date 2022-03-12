using ITI.Baseline.Util;

namespace AppServices;

public class DesignerAppService : ApplicationService, IDesignerAppService
{
    private readonly IAppAuthContext _auth;
    private readonly IAppPermissions _perms;
    private readonly IDesignerQueries _queries;
    private readonly IDesignerRepository _repo;
    private readonly IPageQueries _pageQueries;
    private readonly IProjectQueries _projectQueries;
    private readonly IProductKitReferenceRepository _productKitReferenceRepo;

    public DesignerAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IDesignerQueries queries,
        IDesignerRepository repo,
        IPageQueries pageQueries,
        IProjectQueries projectQueries,
        IProductKitReferenceRepository productKitReferenceRepo
    ) : base(uowp, logger, auth)
    {
        _auth = auth;
        _perms = perms;
        _queries = queries;
        _repo = repo;
        _pageQueries = pageQueries;
        _projectQueries = projectQueries;
        _productKitReferenceRepo = productKitReferenceRepo;
    }

    public Task<DesignerDataDto[]> ListAsync(ProjectId projectId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(projectId)),
            () => _queries.ListAsync(projectId)
        );
    }

    public async Task SetAsync(PageId pageId, DesignerDataType type, string json)
    {
        OrganizationId? organizationId = null;
        ProjectId? projectId = null;

        await CommandAsync(
            async () => Authorize.Require(await _perms.CanManageDesignerDataAsync(pageId)),
            async () =>
            {
                Require.NotNull(_auth.UserId, "Only users can acquire the designer lock.");

                var page = await _pageQueries.GetAsync(pageId);
                Require.NotNull(page, "Could not find page.");

                organizationId = page.OrganizationId;
                projectId = page.ProjectId;

                var lockedById = await _projectQueries.GetDesignerLockedByIdAsync(projectId);

                if (lockedById != _auth.UserId)
                {
                    throw new DomainException(
                        "You must own the designer lock to update designer data for a project.",
                        DomainException.AppServiceLogAs.None
                    );
                }

                await _repo.AddOrUpdateAsync(pageId, type, json);
            }
        );

        if (type != DesignerDataType.PlacedProductKits) return;

        // Not doing this as a domain event to avoid the overhead of a Service Bus
        // message since this method will be called frequently
        await CommandAsync(
            Authorize.AnyUser, // already authorized
            async () =>
            {
                if (organizationId == null)
                    throw new Exception("organizationId is unexpectedly null.");
                if (projectId == null)
                    throw new Exception("projectId is unexpectedly null.");

                await _productKitReferenceRepo.AddAndRemoveReferencesAsync(
                    organizationId,
                    projectId
                );
            }
        );
    }
}
