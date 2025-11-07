namespace ApplicationLayer.Common.Interfaces;

/// <summary>
/// Service for sending real-time exchange rate notifications to connected clients.
/// </summary>
public interface IExchangeRatesNotificationService
{
    /// <summary>
    /// Notifies all connected clients about new historical exchange rates.
    /// </summary>
    Task NotifyHistoricalRatesUpdatedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Notifies all connected clients about updated latest exchange rates.
    /// </summary>
    Task NotifyLatestRatesUpdatedAsync(CancellationToken cancellationToken = default);
}
