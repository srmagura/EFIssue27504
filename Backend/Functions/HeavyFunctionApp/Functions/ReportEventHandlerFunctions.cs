namespace HeavyFunctionApp.Functions;

public class ReportEventHandlerFunctions : EventHandlerFunctions
{
    private readonly ReportEventHandler _reportEventHandler;

    public ReportEventHandlerFunctions(ILogger logger, ReportEventHandler reportEventHandler)
        : base(logger)
    {
        _reportEventHandler = reportEventHandler;
    }

    [FunctionName("reportEventHandler_reportAddedEvent")]
    public Task Process(
        [ServiceBusTrigger(nameof(ReportAddedEvent), nameof(ReportEventHandler))]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken
    )
    {
        return HandleEventAsync<ReportAddedEvent>(
            message,
            messageActions,
            completeMessageImmediately: true,
            _reportEventHandler,
            cancellationToken
        );
    }
}
