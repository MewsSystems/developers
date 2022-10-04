using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers.Providers;

namespace ExchangeRateUpdater.Providers
{
    public interface IExchangeRateProviderStrategyFactory
    {
        IExchangeRateProviderStrategy GetStrategy(ExchangeRateProviderCountry exchangeRateProviderCountry);
    }
}