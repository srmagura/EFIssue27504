namespace DataInterfaces.Repositories;

public interface IProductRequirementRepository
{
    void Add(ProductRequirement productRequirement);

    Task<ProductRequirement?> GetAsync(ProductRequirementId id);

    Task RemoveAsync(ProductRequirementId id);

    Task SetIndexAsync(ProductRequirementId id, int index);
}
