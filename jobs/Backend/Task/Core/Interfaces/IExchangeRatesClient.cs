using ExchangeRateUpdater.Infrastructure.Cnb;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRatesClient
{   
    Task<ExchangeRates> GetTodayExchangeRatesAsync();
}