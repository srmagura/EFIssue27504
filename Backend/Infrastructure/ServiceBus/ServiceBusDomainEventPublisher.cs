using System.Collections.Concurrent;
using Azure.Messaging.ServiceBus;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Core;

namespace ServiceBus;

public class ServiceBusDomainEventPublisher : IDomainEventPublisher
{
    private static readonly ConcurrentDictionary<string, ServiceBusSender> SenderCache = new();

    private readonly ServiceBusClient _client;

    public ServiceBusDomainEventPublisher(ServiceBusClient client)
    {
        _client = client;
    }

    private ServiceBusSender GetSender(string topic)
    {
        if (!SenderCache.ContainsKey(topic))
        {
            SenderCache[topic] = _client.CreateSender(topic);
        }

        return SenderCache[topic];
    }

    public async Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        var groupings = domainEvents.GroupBy(e => e.GetType().Name);

        foreach (var grouping in groupings)
        {
            var sender = GetSender(grouping.Key);

            var messages = grouping
                .Select(e => new ServiceBusMessage(new BinaryData(e)))
                .ToList();

            while (messages.Count > 0)
            {
                using var batch = await sender.CreateMessageBatchAsync();

                foreach (var message in messages.ToArray())
                {
                    var messageAdded = batch.TryAddMessage(message);

                    if (messageAdded)
                    {
                        messages.Remove(message);
                    }
                    else
                    {
                        // Start a new batch
                        break;
                    }
                }

                await sender.SendMessagesAsync(batch);
            }
        }
    }
}
