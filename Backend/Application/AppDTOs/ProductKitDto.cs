namespace AppDTOs;

public class ProductKitDto
{
    public ProductKitDto(ProductKitId id, OrganizationId organizationId, CategoryId categoryId)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        CategoryId = categoryId ?? throw new ArgumentNullException(nameof(categoryId));
    }

    public ProductKitId Id { get; set; }
    public OrganizationId OrganizationId { get; set; }

    public CategoryId CategoryId { get; set; }

    public MeasurementType MeasurementType { get; set; }

    public ProductFamilyReferenceDto? ProductFamily { get; set; }

    public bool IsActive { get; set; }

    public List<ProductKitVersionSummaryDto> Versions { get; set; } = new List<ProductKitVersionSummaryDto>();
}
