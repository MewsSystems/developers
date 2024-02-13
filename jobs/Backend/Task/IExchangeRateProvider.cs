using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    internal interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
    }
}
