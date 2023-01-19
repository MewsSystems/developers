using Entities.Dtos;
using Entities.Concrete;

namespace Tests.Helpers
{
    public static class FakeDataHelper
    {
        public static IEnumerable<ExchangeRate> CreateFakeExchangeRateList()
        {
            var srcCurrency = "CZK";
            var data = new List<ExchangeRate>
            {
                new( new Currency(srcCurrency), new Currency("AUD"), new decimal(15.408)),
                new(new Currency(srcCurrency), new Currency("CAD"), new decimal(16.516)),
                new(new Currency(srcCurrency), new Currency("EUR"), new decimal(23.955)),
                new(new Currency(srcCurrency), new Currency("JPY"), new decimal(17.137)),
                new(new Currency(srcCurrency), new Currency("MXN"), new decimal(1.185)),
                new(new Currency(srcCurrency), new Currency("TRY"), new decimal(21.176)),
            };
            return data;
        }

        public static IEnumerable<ExchangeRate> CreateFakeEmptyExchangeRateList()
        {          
            var data = new List<ExchangeRate>
            {
                
            };
            return data;
        }

        public static CurrencyListRecord CreateFakeCurrencyListRecord()
        {
            var data = new List<Currency>
            {
                new("EUR"),
                new("TRY"),
                new("AUD")
            };
            var currncyListRecord = new CurrencyListRecord(data);
            return currncyListRecord;
        }

        public static CurrencyListRecord CreateFakeWrongCurrencyListRecord()
        {
            var data = new List<Currency>
            {
                new("aaa"),
            };
            var currncyListRecord = new CurrencyListRecord(data);
            return currncyListRecord;
        }

        public static string SourceCurrencyKey
        = "ExchangeRateSourceCurrency";
        public static string SourceCurrencyValue
       = "CZK";
        public static string SourceUrlKey
       = "ExchangeRateSourceUrl";
        public static string SourceUrlValue
       = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

    }
}
