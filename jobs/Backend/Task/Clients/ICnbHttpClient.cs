using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Clients;

public interface ICnbHttpClient
{
    Task<IReadOnlyCollection<CnbExchangeRate>> GetDailyExchangeRates(CancellationToken cancellationToken);
}