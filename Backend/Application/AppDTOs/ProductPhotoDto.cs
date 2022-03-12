namespace AppDTOs;

public class ProductPhotoDto
{
    public ProductPhotoDto(ProductPhotoId id, OrganizationId organizationId, string name, FileRefDto photo)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Photo = photo ?? throw new ArgumentNullException(nameof(photo));
    }

    public ProductPhotoId Id { get; set; }
    public OrganizationId OrganizationId { get; set; }

    public string Name { get; set; }
    public FileRefDto Photo { get; set; }

    public bool IsActive { get; set; }
}
