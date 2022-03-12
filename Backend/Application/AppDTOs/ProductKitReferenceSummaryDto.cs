namespace AppDTOs;

public class ProductKitReferenceSummaryDto
{
    public ProductKitReferenceSummaryDto(
        ProductKitReferenceId id,
        ProductKitId productKitId,
        ProductKitVersionId productKitVersionId,
        string versionName,
        string productKitName,
        string mainComponentName,
        ProductKitVersionId latestProductKitVersionId,
        string latestVersionName
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        ProductKitId = productKitId ?? throw new ArgumentNullException(nameof(productKitId));
        ProductKitVersionId = productKitVersionId ?? throw new ArgumentNullException(nameof(productKitVersionId));
        VersionName = versionName ?? throw new ArgumentNullException(nameof(versionName));
        ProductKitName = productKitName ?? throw new ArgumentNullException(nameof(productKitName));
        MainComponentName = mainComponentName ?? throw new ArgumentNullException(nameof(mainComponentName));
        LatestProductKitVersionId = latestProductKitVersionId ?? throw new ArgumentNullException(nameof(latestProductKitVersionId));
        LatestVersionName = latestVersionName ?? throw new ArgumentNullException(nameof(latestVersionName));
    }

    public ProductKitReferenceId Id { get; set; }

    public ProductKitId ProductKitId { get; set; }
    public ProductKitVersionId ProductKitVersionId { get; set; }
    public string VersionName { get; set; }
    public string ProductKitName { get; set; }
    public string MainComponentName { get; set; }
    public decimal SellPrice { get; set; }

    public ProductKitVersionId LatestProductKitVersionId { get; set; }
    public string LatestVersionName { get; set; }

    public string? Tag { get; set; }
}
