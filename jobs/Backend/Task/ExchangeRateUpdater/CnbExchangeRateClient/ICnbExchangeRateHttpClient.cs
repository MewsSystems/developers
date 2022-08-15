using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public interface ICnbExchangeRateHttpClient
{
    Task<IEnumerable<ExchangeRateLine>> GetTodaysExchangeRates(CancellationToken cancellationToken);
}