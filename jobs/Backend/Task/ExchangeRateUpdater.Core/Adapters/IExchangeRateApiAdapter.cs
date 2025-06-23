using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Adapters;

public interface IExchangeRateApiAdapter
{
	Task<IEnumerable<ExchangeRate>> GetExchangesRateAsync(CancellationToken cancellationToken);
}