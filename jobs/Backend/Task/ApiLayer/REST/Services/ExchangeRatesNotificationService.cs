using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using REST.Response.Converters;
using REST.Hubs;

namespace REST.Services;

/// <summary>
/// Service that sends real-time exchange rate notifications via SignalR.
/// </summary>
public class ExchangeRatesNotificationService : IExchangeRatesNotificationService
{
    private readonly IHubContext<ExchangeRatesHub> _hubContext;
    private readonly IMediator _mediator;
    private readonly ILogger<ExchangeRatesNotificationService> _logger;

    public ExchangeRatesNotificationService(
        IHubContext<ExchangeRatesHub> hubContext,
        IMediator mediator,
        ILogger<ExchangeRatesNotificationService> logger)
    {
        _hubContext = hubContext;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task NotifyHistoricalRatesUpdatedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sending historical rates update notification to SignalR clients");

            // Query all latest exchange rates
            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query, cancellationToken);

            // Convert to grouped response format
            var groupedRates = rates.ToNestedGroupedResponse();

            // Send to all connected clients
            await _hubContext.Clients.All.SendAsync(
                "HistoricalRatesUpdated",
                groupedRates,
                cancellationToken
            );

            _logger.LogInformation("Historical rates notification sent successfully to all clients");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending historical rates notification");
        }
    }

    public async Task NotifyLatestRatesUpdatedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sending latest rates update notification to SignalR clients");

            // Query all latest exchange rates
            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query, cancellationToken);

            // Convert to grouped response format
            var groupedRates = rates.ToNestedGroupedResponse();

            // Send to all connected clients
            await _hubContext.Clients.All.SendAsync(
                "LatestRatesUpdated",
                groupedRates,
                cancellationToken
            );

            _logger.LogInformation("Latest rates notification sent successfully to all clients");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending latest rates notification");
        }
    }
}
