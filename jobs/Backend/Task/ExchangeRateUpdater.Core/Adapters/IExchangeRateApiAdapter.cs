using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Adapters;

internal interface IExchangeRateApiAdapter
{
	Task<IEnumerable<ExchangeRate>> GetExchangesRateAsync(CancellationToken cancellationToken);
}