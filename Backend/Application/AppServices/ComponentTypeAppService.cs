using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace AppServices;

public class ComponentTypeAppService : ApplicationService, IComponentTypeAppService
{
    private readonly IAppPermissions _perms;
    private readonly IComponentTypeQueries _queries;
    private readonly IComponentTypeRepository _repo;

    public ComponentTypeAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IComponentTypeQueries queries,
        IComponentTypeRepository repo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
    }

    public Task<ComponentTypeDto?> GetAsync(ComponentTypeId id)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var componentType = await _queries.GetAsync(id);
                if (componentType == null) return null;

                Authorize.Require(await _perms.CanViewComponentTypesAsync(componentType.OrganizationId));

                return componentType;
            }
        );
    }

    public Task<ComponentTypeDto[]> ListAsync(OrganizationId organizationId, ActiveFilter activeFilter)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewComponentTypesAsync(organizationId)),
            () => _queries.ListAsync(organizationId, activeFilter)
        );
    }

    public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewComponentTypesAsync(organizationId)),
            () => _queries.NameIsAvailableAsync(organizationId, name)
        );
    }

    public Task<ComponentTypeId> AddAsync(OrganizationId organizationId, string name)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageComponentTypesAsync(organizationId)),
            () =>
            {
                var componentType = new ComponentType(organizationId, name);
                _repo.Add(componentType);

                return Task.FromResult(componentType.Id);
            }
        );
    }

    private async Task<ComponentType> GetDomainEntityAsync(ComponentTypeId id)
    {
        var componentType = await _repo.GetAsync(id);
        Require.NotNull(componentType, "Could not find component type.");
        Authorize.Require(await _perms.CanManageComponentTypesAsync(componentType.OrganizationId));

        return componentType;
    }

    public Task SetNameAsync(ComponentTypeId id, string name)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetName(name)
        );
    }

    public Task SetActiveAsync(ComponentTypeId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }
}
