using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application;

public interface IExchangeRateProvider
{
    Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronous wrapper preserved for compatibility with existing callers.
    /// </summary>
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}
