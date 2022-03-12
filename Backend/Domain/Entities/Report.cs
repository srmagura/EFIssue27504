using System.Text.RegularExpressions;
using Enumerations;
using Events;
using ValueObjects;

namespace Entities;

public class Report : AggregateRoot
{
    [Obsolete("AutoMapper only.")]
    public Report(
        ReportId id,
        OrganizationId organizationId,
        ProjectId projectId
    )
    {
        Id = id;
        OrganizationId = organizationId;
        ProjectId = projectId;
    }

    public Report(Project project, ProjectPublication? projectPublication, ReportType type)
    {
        Require.NotNull(project, "Project is required.");
        OrganizationId = project.OrganizationId;
        ProjectId = project.Id;

        if (projectPublication != null)
        {
            Require.IsTrue(project.Id == projectPublication.ProjectId, "Project publication must belong to the same project.");
            ProjectPublicationId = projectPublication.Id;
        }

        Type = type;

        Raise(new ReportAddedEvent(Id));
    }

    public ReportId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public ProjectId ProjectId { get; protected set; }

    public ProjectPublicationId? ProjectPublicationId { get; protected set; }

    public ReportType Type { get; protected set; }

    public FileRef? File { get; protected set; }
    public string? Filename { get; protected set; }

    public DateTimeOffset? ProcessingStartUtc { get; protected set; }
    public DateTimeOffset? ProcessingEndUtc { get; protected set; }

    public Percentage PercentComplete { get; protected set; } = new Percentage(0);

    public ReportStatus Status { get; protected set; } = ReportStatus.Pending;
    public string? ErrorMessage { get; protected set; }

    public void BeginProcessing()
    {
        Status = ReportStatus.Processing;
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

    public void SetCompleted(FileRef file, string projectShortName, int? revisionNumber)
    {
        var typeString = Type == ReportType.DrawingSet ? "Drawing Set" : "Proposal";

        Require.NotNull(file, "File is required.");
        File = file;

        Require.HasValue(projectShortName, "Project short name is required.");
        var sanitizedShortname = Regex.Replace(projectShortName, "[^a-zA-Z0-9-_. ]", "");

        if (revisionNumber.HasValue) Filename = $"{typeString} - {sanitizedShortname} v{revisionNumber.Value}.pdf";
        else Filename = $"{typeString} - {sanitizedShortname} DRAFT.pdf";

        Status = ReportStatus.Completed;
        ProcessingEndUtc = DateTimeOffset.UtcNow;
        SetPercentComplete(new Percentage(1));
    }

    public void SetError(string errorMessage)
    {
        Status = ReportStatus.Error;
        ProcessingEndUtc = DateTimeOffset.UtcNow;

        ErrorMessage = errorMessage;
    }

    public void SetCanceled()
    {
        Status = ReportStatus.Canceled;
        ProcessingEndUtc = DateTimeOffset.UtcNow;
    }
}
