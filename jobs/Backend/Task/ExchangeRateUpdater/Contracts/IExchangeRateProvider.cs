using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Contracts;

/// <summary>
/// Provides capability to retrieve exchange rates.
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Retrieves exchange rates.
    /// </summary>
    /// <param name="currencies">Currencies for which exchange rates should be retrieved.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> observed during retrieval.</param>
    /// <returns>Collection of retrieved exchange rates.</returns>
    public Task<IReadOnlyCollection<ExchangeRate>> RetrieveExchangeRatesAsync(
        IReadOnlyCollection<Currency> currencies,
        CancellationToken cancellationToken);
}