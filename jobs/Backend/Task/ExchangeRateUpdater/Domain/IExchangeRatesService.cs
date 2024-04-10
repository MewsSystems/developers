using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain
{
    internal interface IExchangeRatesService
    {
        Task<IList<ExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken);
    }
}
