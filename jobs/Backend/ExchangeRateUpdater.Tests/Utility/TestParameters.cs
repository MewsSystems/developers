using ExchangeRateUpdater.BL.Models;

namespace ExchangeRateUpdater.Tests.Utility
{
    public class TestParameters
    {
        public static IEnumerable<Currency> GetSampleCurrencies()
        {
         IEnumerable<Currency> currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };
            return currencies;
        }
        public static IEnumerable<Currency> GetDefaultCurrencyOnly()
        {
            IEnumerable<Currency> currencies = new[]
            {
                new Currency("CZK")
            };
            return currencies;
        }
        public  static string GetCNBWebsite() 
        { 
            return "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/";
        }
        public static string GetWrongCNBWebsiteURL()
        {
            return "http://www.o.com/";
        }
    }
}
