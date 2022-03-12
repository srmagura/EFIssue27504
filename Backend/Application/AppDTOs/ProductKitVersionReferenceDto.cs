namespace AppDTOs;

public class ProductKitVersionReferenceDto
{
    public ProductKitVersionReferenceDto(
        ProductKitVersionId id,
        ProductKitId productKitId,
        string name,
        string versionName
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        ProductKitId = productKitId ?? throw new ArgumentNullException(nameof(productKitId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        VersionName = versionName ?? throw new ArgumentNullException(nameof(versionName));
    }

    public ProductKitVersionId Id { get; set; }
    public ProductKitId ProductKitId { get; set; }

    public string Name { get; set; }
    public string VersionName { get; set; }
}
