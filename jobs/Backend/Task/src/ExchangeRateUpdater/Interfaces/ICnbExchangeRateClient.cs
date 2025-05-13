using ExchangeRateUpdater.Entities;

namespace ExchangeRateUpdater.Interfaces;

public interface ICnbExchangeRateClient
{
    Task<IEnumerable<ExchangeRate>> FetchCommonExchangeRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRate>> FetchUncommonExchangeRatesAsync(CancellationToken cancellationToken = default);
}
