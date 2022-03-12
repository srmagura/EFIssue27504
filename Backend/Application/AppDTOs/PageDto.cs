namespace AppDTOs;

public class PageDto
{
    public PageDto(
        PageId id,
        OrganizationId organizationId,
        ProjectId projectId,
        FileRefDto pdf,
        FileRefDto thumbnail
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        Pdf = pdf ?? throw new ArgumentNullException(nameof(pdf));
        Thumbnail = thumbnail ?? throw new ArgumentNullException(nameof(thumbnail));
    }

    public PageId Id { get; set; }

    public OrganizationId OrganizationId { get; set; }
    public ProjectId ProjectId { get; set; }

    public FileRefDto Pdf { get; set; }
    public FileRefDto Thumbnail { get; set; }

    public int Index { get; set; }

    public bool IsActive { get; set; }
}
