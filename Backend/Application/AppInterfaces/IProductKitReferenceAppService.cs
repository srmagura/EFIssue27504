namespace AppInterfaces;

public interface IProductKitReferenceAppService
{
    Task<ProductKitReferenceSummaryDto[]> ListAsync(ProjectId projectId);

    Task SetProductKitVersionAsync(ProductKitReferenceId id, ProductKitVersionId productKitVersionId);
    Task SetTagAsync(ProductKitReferenceId id, string? tag);

    Task UpdateAllAsync(ProjectId projectId);
}
