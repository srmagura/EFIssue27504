namespace HeavyFunctionApp.Functions;

public class ImportEventHandlerFunctions : EventHandlerFunctions
{
    private readonly ImportEventHandler _importEventHandler;

    public ImportEventHandlerFunctions(ILogger logger, ImportEventHandler importEventHandler)
        : base(logger)
    {
        _importEventHandler = importEventHandler;
    }

    [FunctionName("importEventHandler_importAddedEvent")]
    public Task Process(
        [ServiceBusTrigger(nameof(ImportAddedEvent), nameof(ImportEventHandler))]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken
    )
    {
        return HandleEventAsync<ImportAddedEvent>(
            message,
            messageActions,
            completeMessageImmediately: true,
            _importEventHandler,
            cancellationToken
        );
    }
}
