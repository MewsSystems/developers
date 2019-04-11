using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string czechRatesUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = GetCzechRates(czechRatesUrl);

            return rates
                .Where(rate =>
                    currencies
                        .Select(currency => currency.Code)
                        .ToList()
                        .Contains(rate.TargetCurrency.Code));
        }

        private IEnumerable<ExchangeRate> GetCzechRates(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.Delimiter = "|";
                reader.ReadLine();

                var exchangeRates = new List<ExchangeRate>();

                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var code = csv.GetField<string>("kód");
                    var value = csv.GetField<string>("kurz");

                    if (Decimal.TryParse(value, NumberStyles.Number, new CultureInfo("cs-CZ"), out decimal rateValue))
                    {
                        exchangeRates.Add(new ExchangeRate(new Currency("CZK"), new Currency(code), rateValue));
                    }
                }

                return exchangeRates;
            }
        }
    }
}
