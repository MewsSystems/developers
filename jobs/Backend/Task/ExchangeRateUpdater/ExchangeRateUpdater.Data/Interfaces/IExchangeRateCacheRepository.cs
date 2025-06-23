
using ExchangeRateUpdater.Data.Responses;

namespace ExchangeRateUpdater.Data.Interfaces;
public interface IExchangeRateCacheRepository
{
    ExchangeRatesResponseDto GetExchangeRates(DateTime date);
    void SetExchangeRates(ExchangeRatesResponseDto rates);
}
