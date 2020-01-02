using ExchangeRateProvider.Infrastructure;
using System.Configuration;

namespace ExchangeRateUpdater.Infrastructure
{
    public class RateProviderSettings : IRateProviderSettings
    {
        public string CZKExchangeRateProviderUrl => Properties.Settings.Default.CZKExchangeRateProviderUrl;
    }
}
