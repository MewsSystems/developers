using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var webRequest = WebRequest.Create(@"http://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");

            IEnumerable<string> supportedCurrencyCodes = currencies.Select(c => c.Code);
            IEnumerable<ExchangeRate> results = Enumerable.Empty<ExchangeRate>();

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                reader.ReadLine(); // Ignore first line
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = "|";

                    csv.Configuration.RegisterClassMap<ExchangeRateMap>();
                    List<FxRateCnb> records = csv.GetRecords<FxRateCnb>().ToList();

                    results = from currCode in supportedCurrencyCodes
                              from record in records
                              where currCode == record.SourceCurrency && supportedCurrencyCodes.Contains(record.TargetCurrency)
                              select new ExchangeRate(new Currency(record.SourceCurrency), new Currency(record.TargetCurrency), record.Value);
                }
            }

            return results;
        }
    }
}
