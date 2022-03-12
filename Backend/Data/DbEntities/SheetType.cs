namespace DbEntities;

public class DbSheetType : DbEntity
{
    public DbSheetType(string sheetNumberPrefix, string sheetNamePrefix)
    {
        SheetNumberPrefix = sheetNumberPrefix
            ?? throw new ArgumentNullException(nameof(sheetNumberPrefix));
        SheetNamePrefix = sheetNamePrefix
            ?? throw new ArgumentNullException(nameof(sheetNamePrefix));
    }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    [MaxLength(FieldLengths.SheetType.SheetNumberPrefix)]
    public string SheetNumberPrefix { get; set; }

    [MaxLength(FieldLengths.SheetType.SheetNamePrefix)]
    public string SheetNamePrefix { get; set; }

    public bool IsActive { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        // For convention
    }
}
