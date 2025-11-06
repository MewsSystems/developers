namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Interface for Czech National Bank API client.
/// </summary>
public interface ICnbApiClient
{
    /// <summary>
    /// Fetches the raw exchange rate data from CNB.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Raw text data from CNB API.</returns>
    Task<string> FetchExchangeRatesAsync(CancellationToken cancellationToken = default);
}
