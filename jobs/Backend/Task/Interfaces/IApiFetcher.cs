using ExchangeRateUpdater.Helpers;

namespace ExchangeRateUpdater.Interfaces;

public interface IApiFetcher
{
    ApiResponse GetExchangeRates();
}