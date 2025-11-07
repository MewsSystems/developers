using ApplicationLayer.Common.Interfaces;

namespace SOAP.Services;

/// <summary>
/// No-op implementation of IExchangeRatesNotificationService for SOAP API.
/// SOAP doesn't support real-time notifications like SignalR, so this is a stub.
/// </summary>
public class NoOpExchangeRatesNotificationService : IExchangeRatesNotificationService
{
    private readonly ILogger<NoOpExchangeRatesNotificationService> _logger;

    public NoOpExchangeRatesNotificationService(ILogger<NoOpExchangeRatesNotificationService> logger)
    {
        _logger = logger;
    }

    public Task NotifyHistoricalRatesUpdatedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Historical rates updated (SOAP - no notification sent)");
        return Task.CompletedTask;
    }

    public Task NotifyLatestRatesUpdatedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Latest rates updated (SOAP - no notification sent)");
        return Task.CompletedTask;
    }
}
