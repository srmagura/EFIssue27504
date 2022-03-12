using ITI.Baseline.Util;
using ValueObjects;

namespace AppServices;

public class ComponentAppService : ApplicationService, IComponentAppService
{
    private readonly IAppPermissions _perms;
    private readonly IComponentQueries _queries;
    private readonly IComponentRepository _repo;
    private readonly IComponentTypeRepository _componentTypeRepo;

    public ComponentAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IComponentQueries queries,
        IComponentRepository repo,
        IComponentTypeRepository componentTypeRepo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
        _componentTypeRepo = componentTypeRepo;
    }

    public Task<ComponentDto?> GetAsync(ComponentId id)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var component = await _queries.GetAsync(id);

                if (component != null)
                    Authorize.Require(await _perms.CanViewComponentsAsync(component.OrganizationId));

                return component;
            }
        );
    }

    public Task<string> GetNewVersionNameAsync(ComponentId id)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(id)),
            () => _queries.GetNewVersionNameAsync(id)
        );
    }

    public Task<ComponentSummaryDto[]> ListAsync(OrganizationId organizationId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewComponentsAsync(organizationId)),
            () => _queries.ListAsync(organizationId)
        );
    }

    public Task<bool> VendorPartNumberIsAvailableAsync(OrganizationId organizationId, ComponentId? componentId, string vendorPartNumber)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(organizationId)),
            () => _queries.VendorPartNumberIsAvailableAsync(organizationId, componentId, vendorPartNumber)
        );
    }

    public Task<bool> OrganizationPartNumberIsAvailableAsync(OrganizationId organizationId, ComponentId? componentId, string organizationPartNumber)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(organizationId)),
            () => _queries.OrganizationPartNumberIsAvailableAsync(organizationId, componentId, organizationPartNumber)
        );
    }

    private async Task<Component> GetDomainEntityAsync(ComponentId id)
    {
        var component = await _repo.GetAsync(id);
        Require.NotNull(component, "Could not find component.");
        Authorize.Require(await _perms.CanManageComponentsAsync(component.OrganizationId));

        return component;
    }

    public Task<ComponentId> AddAsync(
        OrganizationId organizationId,
        ComponentTypeId componentTypeId,
        MeasurementType measurementType,
        bool isVideoDisplay,
        bool visibleToCustomer,
        string displayName,
        string versionName,
        decimal sellPrice,
        string? url,
        string make,
        string model,
        string vendorPartNumber,
        string? organizationPartNumber,
        string? whereToBuy,
        string? style,
        string? color,
        string? internalNotes
    )
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageComponentsAsync(organizationId)),
            async () =>
            {
                await RequireUniquePartNumbers(organizationId, componentId: null, vendorPartNumber, organizationPartNumber);

                var component = new Component(organizationId, componentTypeId, measurementType, isVideoDisplay, visibleToCustomer);
                _repo.Add(component);

                var version = new ComponentVersion(
                    organizationId,
                    component,
                    displayName,
                    versionName,
                    new Money(sellPrice),
                    url.HasValue() ? new Url(url) : null,
                    make,
                    model,
                    vendorPartNumber,
                    organizationPartNumber,
                    whereToBuy,
                    style,
                    color,
                    internalNotes
                );
                _repo.AddVersion(version);

                return component.Id;
            }
        );
    }

    public Task<ComponentVersionId> AddVersionAsync(
        ComponentId id,
        string displayName,
        string versionName,
        decimal sellPrice,
        string? url,
        string make,
        string model,
        string vendorPartNumber,
        string? organizationPartNumber,
        string? whereToBuy,
        string? style,
        string? color,
        string? internalNotes
    )
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var component = await GetDomainEntityAsync(id);

                await RequireUniquePartNumbers(component.OrganizationId, id, vendorPartNumber, organizationPartNumber);

                var version = new ComponentVersion(
                    component.OrganizationId,
                    component,
                    displayName,
                    versionName,
                    new Money(sellPrice),
                    url.HasValue() ? new Url(url) : null,
                    make,
                    model,
                    vendorPartNumber,
                    organizationPartNumber,
                    whereToBuy,
                    style,
                    color,
                    internalNotes
                );
                _repo.AddVersion(version);

                return version.Id;
            }
        );
    }

    public Task SetActiveAsync(ComponentId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }

    public Task UpdateVersionAsync(
        ComponentVersionId versionId,
        string displayName,
        string? url,
        string make,
        string model,
        string vendorPartNumber,
        string? organizationPartNumber,
        string? whereToBuy,
        string? style,
        string? color,
        string? internalNotes
    )
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var version = await _repo.GetVersionAsync(versionId);
                Require.NotNull(version, "Could not find component version.");
                Authorize.Require(await _perms.CanManageComponentsAsync(version.OrganizationId));

                await RequireUniquePartNumbers(version.OrganizationId, version.ComponentId, vendorPartNumber, organizationPartNumber);

                version.SetDisplayName(displayName);
                version.SetUrl(url.HasValue() ? new Url(url) : null);
                version.SetMake(make);
                version.SetModel(model);
                version.SetVendorPartNumber(vendorPartNumber);
                version.SetOrganizationPartNumber(organizationPartNumber);
                version.SetWhereToBuy(whereToBuy);
                version.SetStyle(style);
                version.SetColor(color);
                version.SetInternalNotes(internalNotes);
            }
        );
    }

    public Task SetComponentTypeAsync(ComponentId id, ComponentTypeId componentTypeId)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var component = await GetDomainEntityAsync(id);
                var componentType = await _componentTypeRepo.GetAsync(componentTypeId);
                Require.NotNull(componentType, "Component type not found.");

                component.SetComponentType(componentType);
            }
        );
    }

    public Task SetVisibleToCustomerAsync(ComponentId id, bool visibleToCustomer)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetVisibleToCustomer(visibleToCustomer)
        );
    }

    private async Task RequireUniquePartNumbers(
        OrganizationId organizationId,
        ComponentId? componentId,
        string vendorPartNumber,
        string? organizationPartNumber
    )
    {
        var vendorPartNumberIsAvailable = await _queries.VendorPartNumberIsAvailableAsync(organizationId, componentId, vendorPartNumber);
        Require.IsTrue(vendorPartNumberIsAvailable, "There is already a component with this vendor part number.");

        if (!string.IsNullOrWhiteSpace(organizationPartNumber))
        {
            var organizationPartNumberIsAvailable = await _queries.OrganizationPartNumberIsAvailableAsync(organizationId, componentId, organizationPartNumber);
            Require.IsTrue(organizationPartNumberIsAvailable, "There is already a component with this organization part number.");
        }
    }
}
