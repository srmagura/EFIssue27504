using Identities;

namespace Events;

public class ProjectPublishedEvent : BaseDomainEvent
{
    public ProjectPublishedEvent(ProjectId projectId, ProjectPublicationId projectPublicationId)
    {
        ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        ProjectPublicationId = projectPublicationId ?? throw new ArgumentNullException(nameof(projectPublicationId));
    }

    public ProjectId ProjectId { get; protected init; }
    public ProjectPublicationId ProjectPublicationId { get; protected init; }
}
