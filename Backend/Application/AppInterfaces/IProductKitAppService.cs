namespace AppInterfaces;

public interface IProductKitAppService
{
    Task<ProductKitDto?> GetAsync(ProductKitId id);
    Task<ProductKitVersionDto?> GetVersionAsync(ProductKitVersionId versionId);

    Task<ProductKitSummaryDto[]> ListAsync(OrganizationId organizationId);
    Task<ProductKitDesignerDto[]> ListForDesignerAsync(ProjectId projectId);
    Task<ProductKitVersionReferenceDto[]> ListForComponentAsync(ComponentId componentId);
    Task<ProductKitVersionReferenceDto[]> ListForComponentVersionAsync(ComponentVersionId componentVersionId);

    Task<string> GetNewVersionNameAsync(ProductKitId id);

    Task<ProductKitId> AddAsync(
        OrganizationId organizationId,
        CategoryId categoryId,
        string name,
        string description,
        string versionName,
        SymbolId symbolId,
        ProductPhotoId? productPhotoId,
        ComponentVersionId mainComponentVersionId,
        List<ProductKitComponentMapInputDto> componentMaps,
        // TODO:SLP-325 ProductRequirementId[] productRequirementIds,
        ProductFamilyId? productFamilyId
    );

    Task<ProductKitVersionId> AddVersionAsync(
        OrganizationId organizationId,
        ProductKitId productKitId,
        string name,
        string description,
        string versionName,
        ComponentVersionId mainComponentVersionId,
        List<ProductKitComponentMapInputDto> componentMaps
    );

    Task SetCategoryAsync(ProductKitId id, CategoryId categoryId);
    Task SetProductFamilyAsync(ProductKitId id, ProductFamilyId? productFamilyId);
    Task SetNameAsync(ProductKitVersionId versionId, string name);
    Task SetDescriptionAsync(ProductKitVersionId versionId, string description);
    Task SetSymbolAsync(ProductKitVersionId versionId, SymbolId symbolId);
    Task SetProductPhotoAsync(ProductKitVersionId versionId, ProductPhotoId? productPhotoId);
    // TODO:SLP-325
    // Task SetProductRequirementsAsync(ProductKitVersionId versionId, ProductRequirementId[] productRequirementIds);

    Task SetActiveAsync(ProductKitId id, bool active);
}
