using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services;

public interface ICnbApiClient
{
    Task<IDictionary<string, DailyExRateItem>> GetExchangeRatesFromCnbApi(CancellationToken token);
}