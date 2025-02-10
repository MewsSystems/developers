using ExchangeRateUpdater.Infrastructure.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Interfaces;

/// <summary>
/// Interface for the Czech National Bank API client.
/// </summary>
public interface ICnbApiClient
{
    /// <summary>
    /// Fetches exchange rates from the CNB API for a given date.
    /// </summary>
    /// <param name="date">The date for which exchange rates should be retrieved.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, returning the CNB API response.</returns>
    Task<CnbApiResponse?> GetExchangeRatesAsync(DateTime date, CancellationToken cancellationToken = default);
}
