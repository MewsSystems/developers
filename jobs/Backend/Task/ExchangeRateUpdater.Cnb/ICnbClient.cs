using ExchangeRateUpdater.Cnb.Dtos;

namespace ExchangeRateUpdater.Cnb
{
    public interface ICnbClient
    {
        Task<DailyExchangeRates> GetLatestExchangeRatesAsync();
    }
}