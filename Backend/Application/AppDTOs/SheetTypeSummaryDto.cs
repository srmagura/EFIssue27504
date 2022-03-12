namespace AppDTOs;

public class SheetTypeSummaryDto
{
    public SheetTypeSummaryDto(SheetTypeId id, string sheetNumberPrefix, string sheetNamePrefix)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        SheetNumberPrefix = sheetNumberPrefix ?? throw new ArgumentNullException(nameof(sheetNumberPrefix));
        SheetNamePrefix = sheetNamePrefix ?? throw new ArgumentNullException(nameof(sheetNamePrefix));
    }

    public SheetTypeId Id { get; set; }

    public string SheetNumberPrefix { get; set; }
    public string SheetNamePrefix { get; set; }

    public bool IsActive { get; set; }
}
