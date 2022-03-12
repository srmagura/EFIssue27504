namespace AppDTOs;

public class ImportDto
{
    public ImportDto(
        ImportId id,
        OrganizationId organizationId,
        ProjectId projectId,
        string filename,
        FileRefDto file,
        ImportStatus status
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        Filename = filename ?? throw new ArgumentNullException(nameof(filename));
        File = file ?? throw new ArgumentNullException(nameof(file));
        Status = status;
    }

    public ImportId Id { get; set; }

    public OrganizationId OrganizationId { get; set; }
    public ProjectId ProjectId { get; set; }

    public string Filename { get; set; }
    public FileRefDto File { get; set; }

    public ImportStatus Status { get; set; }

    public DateTimeOffset? ProcessingStartUtc { get; set; }
    public DateTimeOffset? ProcessingEndUtc { get; set; }

    public decimal PercentComplete { get; set; }

    public string? ErrorMessage { get; set; }
}
