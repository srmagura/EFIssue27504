using Identities;

namespace Events;

public class ReportAddedEvent : BaseDomainEvent
{
    public ReportAddedEvent(ReportId reportId)
    {
        ReportId = reportId ?? throw new ArgumentNullException(nameof(reportId));
    }

    public ReportId ReportId { get; protected init; }
}
