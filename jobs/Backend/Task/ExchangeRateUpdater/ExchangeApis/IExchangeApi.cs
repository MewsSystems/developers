using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeApis;

public interface IExchangeApi
{
    public Task<IEnumerable<ExchangeRate>> GetAllRates(CancellationToken cancellationToken = default);
}
