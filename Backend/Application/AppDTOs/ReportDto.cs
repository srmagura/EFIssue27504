namespace AppDTOs;

public class ReportDto
{
    public ReportDto(
        ReportId id,
        OrganizationId organizationId,
        ReportType type,
        DateTimeOffset? processingStartUtc,
        DateTimeOffset? processingEndUtc,
        decimal percentComplete,
        ReportStatus status,
        string? errorMessage
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        Type = type;
        ProcessingStartUtc = processingStartUtc;
        ProcessingEndUtc = processingEndUtc;
        PercentComplete = percentComplete;
        Status = status;
        ErrorMessage = errorMessage;
    }

    public ReportId Id { get; set; }
    public OrganizationId OrganizationId { get; set; }

    public ReportType Type { get; set; }

    public string? Filename { get; set; }

    public DateTimeOffset? ProcessingStartUtc { get; set; }
    public DateTimeOffset? ProcessingEndUtc { get; set; }

    public decimal PercentComplete { get; set; }

    public ReportStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
