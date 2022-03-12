using Enumerations;

namespace DataInterfaces.Queries;

public interface IAppPermissionsQueries
{
    Task<OrganizationId> OrganizationOfAsync(UserId id);
    Task<OrganizationId> OrganizationOfAsync(ProjectId id);
    Task<OrganizationId> OrganizationOfAsync(ComponentId id);
    Task<OrganizationId> OrganizationOfAsync(ComponentVersionId id);
    Task<OrganizationId> OrganizationOfAsync(ProductKitId id);
    Task<OrganizationId> OrganizationOfAsync(PageId id);
    Task<OrganizationId> OrganizationOfAsync(ProductRequirementId id);
    Task<UserRole> UserRoleForAsync(UserId id);
}
