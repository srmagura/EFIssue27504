namespace AppInterfaces;

public interface ICategoryAppService
{
    Task<TreeDto> GetCategoryTreeAsync(OrganizationId organizationId, ActiveFilter activeFilter);

    Task SetCategoryTreeAsync(OrganizationId organizationId, TreeInputDto tree);
}
