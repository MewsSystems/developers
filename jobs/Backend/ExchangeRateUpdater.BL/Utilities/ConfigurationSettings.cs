using ExchangeRateUpdater.BL.Models;

namespace ExchangeRateUpdater.BL.Utilities
{
    public class ConfigurationSettings
    {
        public const string DefaultCurrencyCode = "CZK";

        public static Currency GetDefaultCurrency()
        {
            return new Currency(DefaultCurrencyCode);
        }
    }
}
