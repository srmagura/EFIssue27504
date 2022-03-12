namespace AppDTOs;

public class ProductKitVersionSummaryDto
{
    public ProductKitVersionSummaryDto(ProductKitVersionId id, DateTimeOffset dateCreatedUtc, string versionName)
    {
        Id = id;
        DateCreatedUtc = dateCreatedUtc;
        VersionName = versionName;
    }

    public ProductKitVersionId Id { get; set; }
    public DateTimeOffset DateCreatedUtc { get; set; }

    public string VersionName { get; set; }
}
