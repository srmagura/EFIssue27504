namespace AppDTOs;

public class ProjectPublicationSummaryDto
{
    public ProjectPublicationSummaryDto(
        ProjectPublicationId id,
        UserReferenceDto publishedBy,
        int revisionNumber,
        bool reportsSentToCustomer
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        PublishedBy = publishedBy ?? throw new ArgumentNullException(nameof(publishedBy));
        RevisionNumber = revisionNumber;
        ReportsSentToCustomer = reportsSentToCustomer;
        // Reports = reports;
    }

    public ProjectPublicationId Id { get; set; }
    public DateTimeOffset DateCreatedUtc { get; set; }

    public UserReferenceDto PublishedBy { get; set; }

    public int RevisionNumber { get; set; }

    public bool ReportsSentToCustomer { get; set; }

    public List<ReportDto> Reports { get; set; } = new();
}
