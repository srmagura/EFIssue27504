namespace Entities;

public class SheetType : AggregateRoot
{
    public SheetType(
        OrganizationId organizationId,
        string sheetNumberPrefix,
        string sheetNamePrefix
    )
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        SetSheetNumberPrefix(sheetNumberPrefix);
        SetSheetNamePrefix(sheetNamePrefix);
    }

    public SheetTypeId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string SheetNumberPrefix { get; protected set; }
    public string SheetNamePrefix { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    [MemberNotNull(nameof(SheetNumberPrefix))]
    public void SetSheetNumberPrefix(string sheetNumberPrefix)
    {
        Require.HasValue(sheetNumberPrefix, "Sheet number prefix is required.");
        SheetNumberPrefix = sheetNumberPrefix;
    }

    [MemberNotNull(nameof(SheetNamePrefix))]
    public void SetSheetNamePrefix(string sheetNamePrefix)
    {
        Require.HasValue(sheetNamePrefix, "Sheet name prefix is required.");
        SheetNamePrefix = sheetNamePrefix;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }
}
