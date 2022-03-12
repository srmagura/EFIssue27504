namespace AppDTOs.Designer;

// Be very careful when changing since the JSON in the database uses this format
public class NoteDto
{
    public NoteDto(
        Guid id,
        Guid? placedProductKitId,
        double[]? position,
        string text
    )
    {
        Id = id;
        PlacedProductKitId = placedProductKitId;
        Position = position;
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public Guid Id { get; set; }
    public Guid? PlacedProductKitId { get; set; }

    public double[]? Position { get; set; }
    public string Text { get; set; }
}
