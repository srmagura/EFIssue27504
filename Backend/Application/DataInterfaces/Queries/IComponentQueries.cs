namespace DataInterfaces.Queries
{
    public interface IComponentQueries
    {
        Task<ComponentDto?> GetAsync(ComponentId id);

        Task<ComponentSummaryDto[]> ListAsync(OrganizationId organizationId);

        Task<ComponentSummaryDto[]> ListComponentsByVersionId(List<ComponentVersionId> componentVersionIds);

        Task<string> GetNewVersionNameAsync(ComponentId id);

        // TODO:SAM
        Task<bool> VersionsAreForDistinctComponentsAsync(List<ComponentVersionId> versionIds);

        Task<bool> VendorPartNumberIsAvailableAsync(OrganizationId organizationId, ComponentId? componentId, string vendorPartNumber);

        Task<bool> OrganizationPartNumberIsAvailableAsync(OrganizationId organizationId, ComponentId? componentId, string organizationPartNumber);
    }
}
