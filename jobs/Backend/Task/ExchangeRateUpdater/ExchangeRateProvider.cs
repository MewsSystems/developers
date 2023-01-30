using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private HttpClient HttpClient { get; }

        public ExchangeRateProvider(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> requiredCurrencies)
        {
            var data = await QueryUrl();

            return ParseData(data, requiredCurrencies);
        }

        private async Task<string> QueryUrl()
        {
            const string url =
                "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

            var response = await this.HttpClient.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        private IEnumerable<ExchangeRate> ParseData(string data, IEnumerable<Currency> requiredCurrencies)
        {
            var targetCurrency = new Currency("CZK");
            var exchangeRates = new List<ExchangeRate>();

            var lines = data.Split("\n");

            foreach (var line in lines)
            {
                var currencyLine = line.Split("|");

                if (String.IsNullOrEmpty(line) || currencyLine.Length != 5)
                {
                    continue;
                }

                var currencyCode = currencyLine[3];

                if (requiredCurrencies.Any(x => x.Code == currencyCode))
                {
                    var rate = currencyLine[4];
                    try
                    {
                        exchangeRates.Add(new ExchangeRate(new Currency(currencyCode), targetCurrency,
                            Convert.ToDecimal(rate)));
                    }
                    catch
                    {
                        // Do not process invalid Decimals
                    }
                }
            }

            return exchangeRates;
        }
    }
}