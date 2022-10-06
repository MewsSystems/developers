using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers.ProvidersStrategies;

namespace ExchangeRateUpdater.Providers
{
    public interface IExchangeRateProviderStrategyFactory
    {
        IExchangeRateProviderStrategy GetStrategy(ExchangeRateProviderCountry exchangeRateProviderCountry);
    }
}