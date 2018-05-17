using ExchangeRateUpdater.ExchangeRateStrategies;

namespace ExchangeRateUpdater.Factory
{
    public class ProviderInfo
    {
        public Currency BaseCurrency { get; }
        public IExchangeRateProviderStrategy Provider { get; }

        public ProviderInfo(Currency baseCurrency, IExchangeRateProviderStrategy provider)
        {
            BaseCurrency = baseCurrency;
            Provider = provider;
        }
    }
}
