namespace AppDTOs;

public class ProductKitComponentMapInputDto
{
    public ProductKitComponentMapInputDto(ProductKitComponentMapId? id, ComponentVersionId componentVersionId, int count)
    {
        Id = id;
        ComponentVersionId = componentVersionId ?? throw new ArgumentNullException(nameof(componentVersionId));
        Count = count;
    }

    public ProductKitComponentMapId? Id { get; set; }

    public ComponentVersionId ComponentVersionId { get; set; }
    public int Count { get; set; }
}
