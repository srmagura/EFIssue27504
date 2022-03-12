namespace AppDTOs;

public class PageSummaryDto
{
    public PageSummaryDto(PageId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public PageId Id { get; set; }

    public int Index { get; set; }

    public SheetTypeId? SheetTypeId { get; set; }
    public string? SheetNumberSuffix { get; set; }
    public string? SheetNameSuffix { get; set; }

    public bool IsActive { get; set; }

    // does not need OrganizationId, ProjectId, or file information
}
