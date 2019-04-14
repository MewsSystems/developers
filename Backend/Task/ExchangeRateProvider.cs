using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string RateEndpoint =
            "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync(RateEndpoint))
            using (var content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                //don't care about two first lines
                int n = 2;
                string[] lines = result
                    .Split(Environment.NewLine.ToCharArray())
                    .Skip(n)
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();
                foreach (var line in lines)
                {
                    var first = line.Split('|');
                    if (Decimal.TryParse(first[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var rate))
                        rates.Add(new ExchangeRate(new Currency("CZK"), new Currency(first[3]), rate));
                }
                return rates.Where(r => currencies.Any(c => c.Code == r.TargetCurrency.Code));
            }
        }
    }
}
