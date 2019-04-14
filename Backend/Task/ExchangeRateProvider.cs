using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();

            var client = new HttpClient();
            var response =
                await client.GetStringAsync(
                    "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt");

            string[] split = response.Split('\n');

            for (int i = 2; i < split.Length - 1; i++)
            {
                var line = split[i].Split('|');
                Currency lineCurrency = new Currency(line[3]);
                if (currencies.Any(c => c.Code == lineCurrency.Code))
                {
                    if (decimal.TryParse(line[4], NumberStyles.Number, new CultureInfo("cs-CZ"), 
                        out decimal rate)) ;
                    rates.Add(new ExchangeRate(new Currency("CZK"), lineCurrency, rate));
                }
            }

            return rates;
        }
    }
}