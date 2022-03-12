namespace DataInterfaces.Repositories;

public interface IProductKitReferenceRepository
{
    Task<ProductKitReference?> GetAsync(ProductKitReferenceId id);
    Task<(ProductKitReference Reference, ProductKitVersionId LatestVersionId)[]> ListWithLatestVersionAsync(ProjectId projectId);

    Task AddAndRemoveReferencesAsync(OrganizationId organizationId, ProjectId projectId);
}
