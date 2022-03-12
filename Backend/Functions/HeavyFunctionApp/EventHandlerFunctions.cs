using ITI.DDD.Application.DomainEvents;

namespace HeavyFunctionApp;

public abstract class EventHandlerFunctions
{
    private readonly ILogger _logger;

    protected EventHandlerFunctions(ILogger logger)
    {
        _logger = logger;
    }

    /// <param name="message">A Service Bus message.</param>
    /// <param name="messageActions">MessageActions for possibly completing the message immediately.</param>
    /// <param name="completeMessageImmediately">
    /// Set this to true if handling the event could take more than 5 minutes.
    /// Otherwise ServiceBus will retry the message.
    /// </param>
    protected async Task HandleEventAsync<TEvent>(
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        bool completeMessageImmediately,
        IDomainEventHandler<TEvent> eventHandler,
        CancellationToken cancellationToken
    )
        where TEvent : BaseDomainEvent
    {
        try
        {
            if (completeMessageImmediately)
                await messageActions.CompleteMessageAsync(message, cancellationToken);

            var domainEvent = message.Body.ToObjectFromJson<TEvent>();
            await eventHandler.HandleAsync(domainEvent, cancellationToken);
        }
        catch (Exception e)
        {
            var logMessage = "Error while handling domain event from service bus. ";
            logMessage += $"Event type: {typeof(TEvent).Name} ";
            logMessage += $"Handler type: {eventHandler.GetType().Name}";

            _logger.Error(logMessage, e);
            throw;
        }
    }
}
