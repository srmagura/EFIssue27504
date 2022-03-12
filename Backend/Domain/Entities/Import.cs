using Enumerations;
using Events;
using ValueObjects;

namespace Entities;

public class Import : AggregateRoot
{
    [Obsolete("Only for use by AutoMapper and other constructors.")]
    public Import(
        bool placeholder,
        OrganizationId organizationId,
        ProjectId projectId,
        string filename,
        FileRef file
    )
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        Require.NotNull(projectId, "Project ID is required.");
        ProjectId = projectId;

        Require.HasValue(filename, "Filename is required.");
        Filename = filename;

        Require.NotNull(file, "File is required.");
        File = file;
    }

    public Import(OrganizationId organizationId, ProjectId projectId, string filename, FileRef file)
#pragma warning disable CS0618 // Type or member is obsolete
        : this(placeholder: true, organizationId, projectId, filename, file)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        Raise(new ImportAddedEvent(Id));
    }

    public ImportId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }
    public ProjectId ProjectId { get; protected set; }

    public string Filename { get; protected set; }
    public FileRef File { get; protected set; }

    public ImportStatus Status { get; protected set; } = ImportStatus.Pending;

    public DateTimeOffset? ProcessingStartUtc { get; protected set; }
    public DateTimeOffset? ProcessingEndUtc { get; protected set; }

    public Percentage PercentComplete { get; protected set; } = new Percentage(0);

    public string? ErrorMessage { get; protected set; }

    public void BeginProcessing()
    {
        Status = ImportStatus.Processing;
        ProcessingStartUtc = DateTimeOffset.UtcNow;
    }

    public void SetPercentComplete(Percentage percentComplete)
    {
        if (percentComplete < new Percentage(0))
            percentComplete = new Percentage(0);
        else if (percentComplete > new Percentage(1))
            percentComplete = new Percentage(1);

        PercentComplete = percentComplete;
    }

    public void SetCompleted()
    {
        Status = ImportStatus.Completed;
        ProcessingEndUtc = DateTimeOffset.UtcNow;
        SetPercentComplete(new Percentage(1));
    }

    public void SetError(string errorMessage)
    {
        Status = ImportStatus.Error;
        ProcessingEndUtc = DateTimeOffset.UtcNow;
        ErrorMessage = errorMessage;
    }

    public void SetCanceled()
    {
        Status = ImportStatus.Canceled;
        ProcessingEndUtc = DateTimeOffset.UtcNow;
    }
}
