namespace AppInterfaces;

public interface IProductRequirementAppService
{
    Task<ProductRequirementDto?> GetAsync(ProductRequirementId id);
    Task<ProductRequirementDto[]> ListAsync(OrganizationId organizationId);

    Task<ProductRequirementId> AddAsync(
        OrganizationId organizationId,
        string label,
        string svgText
    );

    Task SetLabelAsync(ProductRequirementId id, string label);
    Task SetSvgTextAsync(ProductRequirementId id, string svgText);
    Task SetIndexAsync(ProductRequirementId id, int index);
    Task RemoveAsync(ProductRequirementId id);
}
