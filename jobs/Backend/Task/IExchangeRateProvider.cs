using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public interface IExchangeRateProvider
{
    public Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(IReadOnlySet<Currency> currencies, CancellationToken cancellationToken = default);
}