using AppDTOs.Enumerations;

namespace DataInterfaces.Queries
{
    public interface IComponentTypeQueries
    {
        Task<ComponentTypeDto?> GetAsync(ComponentTypeId id);

        Task<ComponentTypeDto[]> ListAsync(OrganizationId organizationId, ActiveFilter activeFilter);

        Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);
    }
}
