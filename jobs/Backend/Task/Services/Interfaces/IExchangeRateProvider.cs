using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public interface IExchangeRateProvider
{
    Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken ct);
}
