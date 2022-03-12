namespace DataInterfaces.Repositories;

public interface IProductKitRepository
{
    Task<ProductKit?> GetAsync(ProductKitId id);

    Task<ProductKitVersion?> GetVersionAsync(ProductKitVersionId id);

    void Add(ProductKit productKit);

    void AddVersion(ProductKitVersion version);
}
