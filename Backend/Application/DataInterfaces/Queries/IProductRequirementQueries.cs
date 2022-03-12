using AppDTOs.Enumerations;

namespace DataInterfaces.Queries;

public interface IProductRequirementQueries
{
    Task<ProductRequirementDto?> GetAsync(ProductRequirementId id);

    Task<ProductRequirementDto[]> ListAsync(OrganizationId organizationId);

    Task<int> GetNextIndexAsync(OrganizationId organizationId);
}
