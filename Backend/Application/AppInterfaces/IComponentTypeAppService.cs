namespace AppInterfaces;

public interface IComponentTypeAppService
{
    Task<ComponentTypeDto?> GetAsync(ComponentTypeId id);

    Task<ComponentTypeDto[]> ListAsync(OrganizationId organizationId, ActiveFilter activeFilter);

    Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);

    Task<ComponentTypeId> AddAsync(OrganizationId organizationId, string name);
    Task SetNameAsync(ComponentTypeId id, string name);
    Task SetActiveAsync(ComponentTypeId id, bool active);
}
