using Identities;

namespace Events;

public class ImportAddedEvent : BaseDomainEvent
{
    public ImportAddedEvent(ImportId importId)
    {
        ImportId = importId ?? throw new ArgumentNullException(nameof(importId));
    }

    public ImportId ImportId { get; protected init; }
}
