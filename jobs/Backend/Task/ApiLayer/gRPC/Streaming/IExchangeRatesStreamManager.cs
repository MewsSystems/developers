using gRPC.Protos.ExchangeRates;
using Grpc.Core;

namespace gRPC.Streaming;

/// <summary>
/// Manages active gRPC streaming connections for real-time exchange rate updates.
/// Replaces SignalR functionality with gRPC server streaming.
/// </summary>
public interface IExchangeRatesStreamManager
{
    /// <summary>
    /// Registers a new client stream for receiving exchange rate updates.
    /// </summary>
    /// <param name="clientId">Unique identifier for the client</param>
    /// <param name="stream">The server stream writer to push updates to the client</param>
    /// <param name="subscriptionTypes">Types of updates the client wants to receive ("latest", "historical", "all")</param>
    /// <param name="cancellationToken">Cancellation token for the client connection</param>
    Task RegisterClientAsync(
        string clientId,
        IServerStreamWriter<ExchangeRateUpdateEvent> stream,
        IEnumerable<string> subscriptionTypes,
        CancellationToken cancellationToken);

    /// <summary>
    /// Unregisters a client stream when the connection is closed.
    /// </summary>
    /// <param name="clientId">Unique identifier for the client</param>
    Task UnregisterClientAsync(string clientId);

    /// <summary>
    /// Broadcasts a "LatestRatesUpdated" event to all subscribed clients.
    /// Called by background jobs when new exchange rates are fetched.
    /// </summary>
    /// <param name="data">The updated exchange rate data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task BroadcastLatestRatesUpdatedAsync(
        ExchangeRatesGroupedData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Broadcasts a "HistoricalRatesUpdated" event to all subscribed clients.
    /// Called by background jobs when historical rates are fetched.
    /// </summary>
    /// <param name="data">The updated exchange rate data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task BroadcastHistoricalRatesUpdatedAsync(
        ExchangeRatesGroupedData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of currently connected clients.
    /// </summary>
    int GetConnectedClientCount();
}
