namespace AppDTOs;

public class ProductFamilyDto
{
    public ProductFamilyDto(ProductFamilyId id, string name)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public ProductFamilyDto(ProductFamilyId id, string name, bool isActive, ProductKitReferenceDto[] productKits)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsActive = isActive;
        ProductKits = productKits ?? throw new ArgumentNullException(nameof(productKits));
    }

    public ProductFamilyId Id { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public ProductKitReferenceDto[] ProductKits { get; set; } = Array.Empty<ProductKitReferenceDto>();
}
