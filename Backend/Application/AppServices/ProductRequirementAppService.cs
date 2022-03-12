using ITI.Baseline.Util;

namespace AppServices;

public class ProductRequirementAppService : ApplicationService, IProductRequirementAppService
{
    private readonly IAppPermissions _perms;
    private readonly IProductRequirementQueries _queries;
    private readonly IProductRequirementRepository _repo;

    public ProductRequirementAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IProductRequirementQueries queries,
        IProductRequirementRepository repo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
    }

    public Task<ProductRequirementDto?> GetAsync(ProductRequirementId id)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var productRequirement = await _queries.GetAsync(id);
                if (productRequirement == null) return null;

                Authorize.Require(await _perms.CanViewProductRequirementsAsync(productRequirement.OrganizationId));

                return productRequirement;
            }
        );
    }

    public Task<ProductRequirementDto[]> ListAsync(OrganizationId organizationId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProductRequirementsAsync(organizationId)),
            () => _queries.ListAsync(organizationId)
        );
    }

    private async Task<ProductRequirement> GetDomainEntityAsync(ProductRequirementId id)
    {
        var productRequirement = await _repo.GetAsync(id);
        Require.NotNull(productRequirement, "Could not find product requirement.");
        Authorize.Require(await _perms.CanManageProductRequirementsAsync(productRequirement.OrganizationId));

        return productRequirement;
    }

    public Task<ProductRequirementId> AddAsync(OrganizationId organizationId, string label, string svgText)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageProductRequirementsAsync(organizationId)),
            async () =>
            {
                var index = await _queries.GetNextIndexAsync(organizationId);
                var productRequirement = new ProductRequirement(organizationId, label, svgText, index);
                _repo.Add(productRequirement);
                return productRequirement.Id;
            }
        );
    }

    public Task RemoveAsync(ProductRequirementId id)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageAsync(id)),
            () => _repo.RemoveAsync(id)
        );
    }

    public Task SetIndexAsync(ProductRequirementId id, int index)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageAsync(id)),
            () => _repo.SetIndexAsync(id, index)
        );
    }

    public Task SetLabelAsync(ProductRequirementId id, string label)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetLabel(label)
        );
    }

    public Task SetSvgTextAsync(ProductRequirementId id, string svgText)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetSvgText(svgText)
        );
    }
}
