namespace AppDTOs;

// This is a reference to a ProductKit, not a ProductKitReference
public class ProductKitReferenceDto
{
    public ProductKitReferenceDto(ProductKitId id, string name, decimal sellPrice)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        SellPrice = sellPrice;
    }

    public ProductKitId Id { get; set; }

    public string Name { get; set; }
    public decimal SellPrice { get; set; }
}
