using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        const string EXCHANGE_RATE_BASE_URL = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

        readonly Currency TARGET_CURRENCY = new Currency("CZK");

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            WebClient client = new WebClient()
            {
                Encoding = Encoding.UTF8
            };

            IEnumerable<ExchangeRate> exchangeRates = client.DownloadString(EXCHANGE_RATE_BASE_URL)
                .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .Skip(2)
                .Select(line => ParseExchangeRate(line));


            return exchangeRates;
        }

        public ExchangeRate ParseExchangeRate(string line)
        {
            const char DELIMITER = '|';

            string[] values = line.Split(DELIMITER);

            if (values.Length == 5)
            {
                // currently unused values:
                //string country = values[0];
                //string currencyName = values[1];

                int amount = Convert.ToInt32(values[2]);
                string currencyCode = values[3];
                decimal rate = Convert.ToDecimal(values[4]);

                Currency sourceCurrency = new Currency(currencyCode);

                return new ExchangeRate(sourceCurrency, TARGET_CURRENCY, rate / amount);
            }
            else
            {
                throw new ArgumentException($"Unable to parse exchange rate line: {line}", nameof(line));
            }
        }
    }
}
