using ITI.DDD.Core;

namespace Events;

public abstract class BaseDomainEvent : IDomainEvent
{
    public DateTimeOffset DateCreatedUtc { get; } = DateTimeOffset.UtcNow;
}
