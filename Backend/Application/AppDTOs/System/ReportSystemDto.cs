namespace AppDTOs;

public class ReportSystemDto
{
    public ReportSystemDto(
        ReportId id,
        OrganizationId organizationId,
        ProjectId projectId
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
    }

    public ReportId Id { get; set; }

    public OrganizationId OrganizationId { get; set; }
    public ProjectId ProjectId { get; set; }
    public ProjectPublicationId? ProjectPublicationId { get; set; }

    public ReportType Type { get; set; }

    public FileRefDto? File { get; set; }
    public string? Filename { get; set; }

    public DateTimeOffset? ProcessingStartUtc { get; set; }
    public DateTimeOffset? ProcessingEndUtc { get; set; }

    public decimal PercentComplete { get; set; }

    public ReportStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
