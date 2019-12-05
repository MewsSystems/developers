using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string centralBankBaseAddress = "https://www.cnb.cz/en/";
        private const string exchangeRatesPath = "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(centralBankBaseAddress)
            };

            var dateQuery = $"?date={DateTime.Now.ToString("dd.MM.yyyy")}";
            var response = await client.GetAsync($"{exchangeRatesPath}{dateQuery}");
            if (response.IsSuccessStatusCode)
            {
                var ratesResponse = await response.Content.ReadAsStringAsync();

                var exchangeRates = ParseExchangeRates(ratesResponse)
                                   .Where(r => currencies.Any(c => c.Code == r.SourceCurrency.Code));

                return exchangeRates;
            }
            return Enumerable.Empty<ExchangeRate>();
        }

        private List<ExchangeRate> ParseExchangeRates(string ratesResponse)
        {
            var rates = ratesResponse.Split(new[] { "\n" },
                             StringSplitOptions.None)
                             .Where(s => !string.IsNullOrWhiteSpace(s))
                             .Skip(2);

            var exchangeRates = new List<ExchangeRate>();

            foreach (var r in rates)
            {
                var rate = r.Split('|');
                var sourceCurrency = new Currency(rate[3]);
                var targetCurrency = new Currency("CZK");
                decimal.TryParse(rate[4], out decimal value);

                exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, value));
            }
            return exchangeRates;
        }
    }
}
