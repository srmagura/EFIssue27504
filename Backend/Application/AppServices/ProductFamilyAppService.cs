using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace AppServices;

public class ProductFamilyAppService : ApplicationService, IProductFamilyAppService
{
    private readonly IAppPermissions _perms;
    private readonly IProductFamilyQueries _queries;
    private readonly IProductFamilyRepository _repo;

    public ProductFamilyAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IProductFamilyQueries queries,
        IProductFamilyRepository repo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
    }

    public Task<FilteredList<ProductFamilyDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    )
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProductFamiliesAsync(organizationId)),
            () => _queries.ListAsync(organizationId, skip, take, activeFilter, search)
        );
    }

    public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProductFamiliesAsync(organizationId)),
            () => _queries.NameIsAvailableAsync(organizationId, name)
        );
    }

    public Task<ProductFamilyId> AddAsync(OrganizationId organizationId, string name)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageProductFamiliesAsync(organizationId)),
            () =>
            {
                var productFamily = new ProductFamily(organizationId, name);
                _repo.Add(productFamily);
                return Task.FromResult(productFamily.Id);
            }
        );
    }

    private async Task<ProductFamily> GetDomainEntityAsync(ProductFamilyId id)
    {
        var productFamily = await _repo.GetAsync(id);
        Require.NotNull(productFamily, "Could not find product family.");
        Authorize.Require(await _perms.CanManageProductFamiliesAsync(productFamily.OrganizationId));

        return productFamily;
    }

    public Task SetActiveAsync(ProductFamilyId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }

    public Task SetNameAsync(ProductFamilyId id, string name)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetName(name)
        );
    }
}
