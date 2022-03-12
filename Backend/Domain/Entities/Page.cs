using ValueObjects;

namespace Entities;

public class Page : AggregateRoot
{
    public Page(OrganizationId organizationId, ProjectId projectId, FileRef pdf, FileRef thumbnail, int index)
    {
        Require.NotNull(organizationId, "Organization");
        OrganizationId = organizationId;

        Require.NotNull(projectId, "Project");
        ProjectId = projectId;

        Require.NotNull(pdf, "PDF");
        Pdf = pdf;

        Require.NotNull(thumbnail, "Thumbnail");
        Thumbnail = thumbnail;

        Index = index;
    }

    public PageId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }
    public ProjectId ProjectId { get; protected set; }

    public FileRef Pdf { get; protected set; }
    public FileRef Thumbnail { get; protected set; }

    public int Index { get; protected set; }

    // The following three properties fully describe the sheet number & sheet name of
    // this page
    public SheetTypeId? SheetTypeId { get; protected set; }
    public string? SheetNumberSuffix { get; protected set; }
    public string? SheetNameSuffix { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    public void SetSheetNumberAndName(SheetType sheetType, string sheetNumberSuffix, string? sheetNameSuffix)
    {
        Require.NotNull(sheetType, "Sheet type is required.");
        Require.IsTrue(sheetType.OrganizationId == OrganizationId, "Organization ID mismatch.");
        SheetTypeId = sheetType.Id;

        Require.HasValue(sheetNumberSuffix, "Sheet number suffix is required.");
        SheetNumberSuffix = sheetNumberSuffix;

        SheetNameSuffix = sheetNameSuffix;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
