namespace HeavyFunctionApp.Functions;

public class ProjectEventHandlerFunctions : EventHandlerFunctions
{
    private readonly ProjectEventHandler _projectEventHandler;

    public ProjectEventHandlerFunctions(ILogger logger, ProjectEventHandler projectEventHandler)
        : base(logger)
    {
        _projectEventHandler = projectEventHandler;
    }

    [FunctionName("projectEventHandler_projectPublishedEvent")]
    public Task Process(
        [ServiceBusTrigger(nameof(ProjectPublishedEvent), nameof(ProjectEventHandler))]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken
    )
    {
        return HandleEventAsync<ProjectPublishedEvent>(
            message,
            messageActions,
            completeMessageImmediately: false,
            _projectEventHandler,
            cancellationToken
        );
    }
}
