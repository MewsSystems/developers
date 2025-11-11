using System.Collections.Concurrent;
using gRPC.Protos.ExchangeRates;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace gRPC.Streaming;

/// <summary>
/// Thread-safe implementation of stream manager for broadcasting exchange rate updates to connected gRPC clients.
/// </summary>
public class ExchangeRatesStreamManager : IExchangeRatesStreamManager
{
    private readonly ConcurrentDictionary<string, ClientStreamInfo> _clients = new();
    private readonly ILogger<ExchangeRatesStreamManager> _logger;

    public ExchangeRatesStreamManager(ILogger<ExchangeRatesStreamManager> logger)
    {
        _logger = logger;
    }

    public Task RegisterClientAsync(
        string clientId,
        IServerStreamWriter<ExchangeRateUpdateEvent> stream,
        IEnumerable<string> subscriptionTypes,
        CancellationToken cancellationToken)
    {
        var clientInfo = new ClientStreamInfo
        {
            Stream = stream,
            SubscriptionTypes = new HashSet<string>(subscriptionTypes, StringComparer.OrdinalIgnoreCase),
            CancellationToken = cancellationToken,
            ConnectedAt = DateTime.UtcNow
        };

        if (_clients.TryAdd(clientId, clientInfo))
        {
            _logger.LogInformation(
                "Client {ClientId} registered for streaming. Subscriptions: {Subscriptions}. Total clients: {TotalClients}",
                clientId,
                string.Join(", ", subscriptionTypes),
                _clients.Count);
            return Task.CompletedTask;
        }

        _logger.LogWarning("Failed to register client {ClientId} - already exists", clientId);
        return Task.CompletedTask;
    }

    public Task UnregisterClientAsync(string clientId)
    {
        if (_clients.TryRemove(clientId, out var clientInfo))
        {
            var duration = DateTime.UtcNow - clientInfo.ConnectedAt;
            _logger.LogInformation(
                "Client {ClientId} unregistered. Connection duration: {Duration}. Remaining clients: {TotalClients}",
                clientId,
                duration,
                _clients.Count);
        }
        else
        {
            _logger.LogWarning("Failed to unregister client {ClientId} - not found", clientId);
        }

        return Task.CompletedTask;
    }

    public async Task BroadcastLatestRatesUpdatedAsync(
        ExchangeRatesGroupedData data,
        CancellationToken cancellationToken = default)
    {
        var updateEvent = new ExchangeRateUpdateEvent
        {
            EventType = "LatestRatesUpdated",
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
            Data = data
        };

        await BroadcastToSubscribersAsync(updateEvent, "latest", cancellationToken);
    }

    public async Task BroadcastHistoricalRatesUpdatedAsync(
        ExchangeRatesGroupedData data,
        CancellationToken cancellationToken = default)
    {
        var updateEvent = new ExchangeRateUpdateEvent
        {
            EventType = "HistoricalRatesUpdated",
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
            Data = data
        };

        await BroadcastToSubscribersAsync(updateEvent, "historical", cancellationToken);
    }

    public int GetConnectedClientCount() => _clients.Count;

    private async Task BroadcastToSubscribersAsync(
        ExchangeRateUpdateEvent updateEvent,
        string eventCategory,
        CancellationToken cancellationToken)
    {
        // Filter clients that are subscribed to this event type
        var subscribedClients = _clients
            .Where(kvp =>
                kvp.Value.SubscriptionTypes.Contains(eventCategory) ||
                kvp.Value.SubscriptionTypes.Contains("all"))
            .ToList();

        if (subscribedClients.Count == 0)
        {
            _logger.LogInformation(
                "No clients subscribed to '{EventCategory}' updates. Skipping broadcast.",
                eventCategory);
            return;
        }

        _logger.LogInformation(
            "Broadcasting '{EventType}' event to {ClientCount} client(s)",
            updateEvent.EventType,
            subscribedClients.Count);

        // Broadcast to all subscribed clients in parallel
        var broadcastTasks = subscribedClients.Select(kvp =>
            SendToClientAsync(kvp.Key, kvp.Value, updateEvent, cancellationToken));

        await Task.WhenAll(broadcastTasks);

        _logger.LogInformation(
            "Completed broadcasting '{EventType}' event",
            updateEvent.EventType);
    }

    private async Task SendToClientAsync(
        string clientId,
        ClientStreamInfo clientInfo,
        ExchangeRateUpdateEvent updateEvent,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check if client's connection is still active
            if (clientInfo.CancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Client {ClientId} connection cancelled, removing from active clients", clientId);
                await UnregisterClientAsync(clientId);
                return;
            }

            // Write the update to the client's stream
            await clientInfo.Stream.WriteAsync(updateEvent, cancellationToken);

            _logger.LogDebug(
                "Successfully sent '{EventType}' update to client {ClientId}",
                updateEvent.EventType,
                clientId);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled ||
                                       ex.StatusCode == StatusCode.Unavailable)
        {
            _logger.LogInformation(
                "Client {ClientId} connection lost (Status: {StatusCode}), removing from active clients",
                clientId,
                ex.StatusCode);
            await UnregisterClientAsync(clientId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send update to client {ClientId}. Error: {ErrorMessage}",
                clientId,
                ex.Message);

            // Remove client if we can't send to them
            await UnregisterClientAsync(clientId);
        }
    }

    /// <summary>
    /// Internal class to track client stream information
    /// </summary>
    private class ClientStreamInfo
    {
        public required IServerStreamWriter<ExchangeRateUpdateEvent> Stream { get; init; }
        public required HashSet<string> SubscriptionTypes { get; init; }
        public required CancellationToken CancellationToken { get; init; }
        public DateTime ConnectedAt { get; init; }
    }
}
