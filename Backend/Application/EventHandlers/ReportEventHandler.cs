using Events;
using InfraInterfaces;
using ITI.DDD.Application.DomainEvents;

namespace EventHandlers;

public class ReportEventHandler : IDomainEventHandler<ReportAddedEvent>
{
    public static void Register(DomainEventHandlerRegistryBuilder builder)
    {
        builder.Register<ReportAddedEvent, ReportEventHandler>();
    }

    private readonly IReportProcessor _reportProcessor;

    public ReportEventHandler(IReportProcessor reportProcessor)
    {
        _reportProcessor = reportProcessor;
    }

    public Task HandleAsync(ReportAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        return _reportProcessor.ProcessAsync(domainEvent.ReportId, cancellationToken);
    }
}
