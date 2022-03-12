using ITI.Baseline.Util;

namespace AppServices;

public class ProductKitReferenceAppService : ApplicationService, IProductKitReferenceAppService
{
    private readonly IAppPermissions _perms;
    private readonly IProductKitReferenceQueries _queries;
    private readonly IProductKitReferenceRepository _repo;
    private readonly IProductKitRepository _productKitRepo;

    public ProductKitReferenceAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IProductKitReferenceQueries queries,
        IProductKitReferenceRepository repo,
        IProductKitRepository productKitRepo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
        _queries = queries;
        _repo = repo;
        _productKitRepo = productKitRepo;
    }

    public Task<ProductKitReferenceSummaryDto[]> ListAsync(ProjectId projectId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProductKitReferencesAsync(projectId)),
            () => _queries.ListAsync(projectId)
        );
    }

    private async Task<ProductKitReference> GetDomainEntityAsync(ProductKitReferenceId id)
    {
        var productKitReference = await _repo.GetAsync(id);
        Require.NotNull(productKitReference, "Could not find product kit reference.");
        Authorize.Require(await _perms.CanManageProductKitReferencesAsync(productKitReference.OrganizationId));

        return productKitReference;
    }

    public Task SetProductKitVersionAsync(ProductKitReferenceId id, ProductKitVersionId productKitVersionId)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var productKitReference = await GetDomainEntityAsync(id);

                var productKitVersion = await _productKitRepo.GetVersionAsync(productKitVersionId);
                Require.NotNull(productKitVersion, "Could not find product kit version.");

                productKitReference.SetProductKitVersion(productKitVersion);
            }
        );
    }

    public Task SetTagAsync(ProductKitReferenceId id, string? tag)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetTag(tag)
        );
    }

    public Task UpdateAllAsync(ProjectId projectId)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageProductKitReferencesAsync(projectId)),
            async () =>
            {
                var tuples = await _repo.ListWithLatestVersionAsync(projectId);

                foreach (var (reference, latestVersionId) in tuples)
                {
                    reference.SetProductKitVersionId(latestVersionId);
                }
            }
        );
    }
}
