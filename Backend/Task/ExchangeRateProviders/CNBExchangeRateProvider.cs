using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
    public class CNBExchangeRateProvider
    {
        const string EXCHANGE_RATE_BASE_URL = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

        readonly Currency TARGET_CURRENCY = new Currency("CZK");

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, DateTime? date = null)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");

            using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = date.HasValue ?
                    $"{EXCHANGE_RATE_BASE_URL}?date={date.Value.ToString("dd.MM.yyyy")}" :
                    EXCHANGE_RATE_BASE_URL;

                IEnumerable<ExchangeRate> exchangeRates = client.DownloadString(url)
                    .Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(2)
                    .Select(line => ParseExchangeRate(line))
                    .Where(c => currencies.Contains(c.SourceCurrency)); // TODO: sort based on input?

                Thread.CurrentThread.CurrentCulture = currentCulture;

                return exchangeRates;
            }
        }

        private ExchangeRate ParseExchangeRate(string line)
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
