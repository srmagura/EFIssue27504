using Events;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Core;

namespace TestUtilities;

public class NullDomainEventPublisher : IDomainEventPublisher
{
    private readonly List<BaseDomainEvent> _receivedEvents = new();

    public IReadOnlyList<IDomainEvent> ReceivedEvents => _receivedEvents;

    public Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        _receivedEvents.AddRange(domainEvents.Cast<BaseDomainEvent>());
        return Task.CompletedTask;
    }
}
