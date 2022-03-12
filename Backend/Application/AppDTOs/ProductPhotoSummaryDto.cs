namespace AppDTOs;

public class ProductPhotoSummaryDto
{
    public ProductPhotoSummaryDto(ProductPhotoId id, string name)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public ProductPhotoId Id { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }
}
