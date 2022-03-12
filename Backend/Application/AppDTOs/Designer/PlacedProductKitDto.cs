namespace AppDTOs.Designer;

// Be very careful when changing since the JSON in the database uses this format
public class PlacedProductKitDto
{
    public PlacedProductKitDto(
        Guid id,
        ProductKitId productKitId,
        double[] position,
        double rotation,
        int? lengthInches
    )
    {
        Id = id;
        ProductKitId = productKitId;
        Position = position ?? throw new ArgumentNullException(nameof(position));
        Rotation = rotation;
        LengthInches = lengthInches;
    }

    public Guid Id { get; set; }
    public ProductKitId ProductKitId { get; set; }

    public double[] Position { get; set; }
    public double Rotation { get; set; }

    public int? LengthInches { get; set; }
}
