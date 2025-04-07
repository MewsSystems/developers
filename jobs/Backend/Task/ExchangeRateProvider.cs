using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<ExchangeRate> getRateFromInputFile(string content, IEnumerable<Currency> currencies)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Content cannot be null or empty.", nameof(content));
            }

            IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            string[] lines = content.Split('\n');
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 5 && parts[3].Length == 3)
                {
                    Currency sourceCurrency = new Currency(parts[3]);
                    if (currencies.Contains(sourceCurrency))
                    {
                        decimal sourceAmount = decimal.Parse(parts[2]);
                        decimal targetValue = decimal.Parse(parts[4]);
                        ExchangeRate exchangeRate = new ExchangeRate(sourceCurrency, sourceAmount, targetValue);
                        exchangeRates.Add(exchangeRate);
                    }
                }
            }
            return exchangeRates;
        }

        public IEnumerable<ExchangeRate> GetInputFiles(IEnumerable<Currency> currencies){
            string sourceCommonCurrencies = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
            string sourceOtherCurrencies = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";
            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(sourceCommonCurrencies);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                getRateFromInputFile(content);
            }
            else
            {
                throw new Exception($"Failed to retrieve data from {sourceCommonCurrencies}");
            }
            response = await httpClient.GetAsync(sourceOtherCurrencies);
            if (response.IsSuccessStatusCode)
            {
                sring content = await response.Content.ReadAsStringAsync();
                getRateFromInputFile(content);
            }
            else
            {
                throw new Exception($"Failed to retrieve data from {sourceOtherCurrencies}");
            }
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }
            return GetInputFiles(currencies);
        }
    }
}
