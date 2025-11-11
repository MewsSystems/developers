using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using gRPC.Mappers;
using MediatR;

namespace gRPC.Streaming;

/// <summary>
/// gRPC implementation of IExchangeRatesNotificationService.
/// Replaces the SignalR-based notification service with gRPC server streaming.
/// Background jobs call this service to broadcast updates to all connected clients.
/// </summary>
public class ExchangeRatesGrpcNotificationService : IExchangeRatesNotificationService
{
    private readonly IExchangeRatesStreamManager _streamManager;
    private readonly IMediator _mediator;
    private readonly ILogger<ExchangeRatesGrpcNotificationService> _logger;

    public ExchangeRatesGrpcNotificationService(
        IExchangeRatesStreamManager streamManager,
        IMediator mediator,
        ILogger<ExchangeRatesGrpcNotificationService> logger)
    {
        _streamManager = streamManager;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task NotifyLatestRatesUpdatedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Broadcasting latest rates update via gRPC streaming");

            // Query all latest exchange rates using existing MediatR handler
            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query, cancellationToken);

            // Convert DTOs to proto messages
            var protoData = ExchangeRateMappers.ToProtoGroupedData(rates);

            // Broadcast to all connected gRPC clients
            await _streamManager.BroadcastLatestRatesUpdatedAsync(protoData, cancellationToken);

            var connectedClients = _streamManager.GetConnectedClientCount();
            _logger.LogInformation(
                "Latest rates broadcast completed. {ClientCount} client(s) notified",
                connectedClients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error broadcasting latest rates via gRPC");
        }
    }

    public async Task NotifyHistoricalRatesUpdatedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Broadcasting historical rates update via gRPC streaming");

            // Query all latest exchange rates using existing MediatR handler
            // (Historical rates update uses the same data as latest rates)
            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query, cancellationToken);

            // Convert DTOs to proto messages
            var protoData = ExchangeRateMappers.ToProtoGroupedData(rates);

            // Broadcast to all connected gRPC clients
            await _streamManager.BroadcastHistoricalRatesUpdatedAsync(protoData, cancellationToken);

            var connectedClients = _streamManager.GetConnectedClientCount();
            _logger.LogInformation(
                "Historical rates broadcast completed. {ClientCount} client(s) notified",
                connectedClients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error broadcasting historical rates via gRPC");
        }
    }
}
