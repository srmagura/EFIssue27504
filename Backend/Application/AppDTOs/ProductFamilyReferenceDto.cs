namespace AppDTOs;

public class ProductFamilyReferenceDto
{
    public ProductFamilyReferenceDto(ProductFamilyId id, string name)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public ProductFamilyId Id { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }
}
