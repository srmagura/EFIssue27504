using Events;
using InfraInterfaces;
using ITI.DDD.Application.DomainEvents;

namespace EventHandlers;

public class ImportEventHandler : IDomainEventHandler<ImportAddedEvent>
{
    public static void Register(DomainEventHandlerRegistryBuilder builder)
    {
        builder.Register<ImportAddedEvent, ImportEventHandler>();
    }

    private readonly IImportProcessor _importProcessor;

    public ImportEventHandler(IImportProcessor importProcessor)
    {
        _importProcessor = importProcessor;
    }

    public Task HandleAsync(ImportAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        return _importProcessor.ProcessAsync(domainEvent.ImportId, cancellationToken);
    }
}
