using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace ExchangeRateUpdater.CnbRates;

public interface ICnbClient
{
    [Get("/cnbapi/exrates/daily")]
    public Task<CnbRatesResult> RetrieveExchangeRatesAsync(CancellationToken cancellationToken);
}