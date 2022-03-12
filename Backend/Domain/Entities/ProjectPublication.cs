using Events;

namespace Entities;

public class ProjectPublication : AggregateRoot
{
    [Obsolete("AutoMapper only.")]
    public ProjectPublication(
        OrganizationId organizationId,
        ProjectId projectId,
        UserId publishedById
    )
    {
        OrganizationId = organizationId;
        ProjectId = projectId;
        PublishedById = publishedById;
    }

    public ProjectPublication(
        Project project,
        UserId publishedById,
        int revisionNumber,
        bool reportsSentToCustomer
    )
    {
        Require.NotNull(project, "Project is required.");
        OrganizationId = project.OrganizationId;
        ProjectId = project.Id;

        Require.NotNull(publishedById, "Published by is required.");
        PublishedById = publishedById;

        Require.IsTrue(revisionNumber > 0, "Revision number must be greater than 0.");
        RevisionNumber = revisionNumber;

        ReportsSentToCustomer = reportsSentToCustomer;

        Raise(new ProjectPublishedEvent(project.Id, Id));
    }

    public ProjectPublicationId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }
    public ProjectId ProjectId { get; protected set; }
    public UserId PublishedById { get; protected set; }

    public int RevisionNumber { get; protected set; }

    public bool ReportsSentToCustomer { get; protected set; }

    public void SetReportsSentToCustomer(bool reportsSentToCustomer)
    {
        ReportsSentToCustomer = reportsSentToCustomer;
    }
}
