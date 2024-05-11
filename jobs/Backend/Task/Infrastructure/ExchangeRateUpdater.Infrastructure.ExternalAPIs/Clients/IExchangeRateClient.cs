using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Clients;

internal interface IExchangeRateClient
{
    public Task<IEnumerable<CnbRate>?> GetRates(CancellationToken cancellationToken);
    public Task<IEnumerable<CnbRate>?> GetRates(DateTime date, CancellationToken cancellationToken);
}