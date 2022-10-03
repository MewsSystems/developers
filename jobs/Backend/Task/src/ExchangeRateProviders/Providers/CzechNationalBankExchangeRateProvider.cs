using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers.Providers
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient httpClient;
        private const string BankUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string targetCurrencyCode = "CZK";

        public CzechNationalBankExchangeRateProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            var response = await this.httpClient.GetAsync(BankUrl);

            if (!response.IsSuccessStatusCode)
            {
                return new List<ExchangeRate>();
            }

            var result = await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync();
            var reader = new StreamReader(result, System.Text.Encoding.UTF8);
            return this.ExchangeRatesBuilder(reader);
        }

        private IEnumerable<ExchangeRate> ExchangeRatesBuilder(StreamReader reader)
        {
            var exchangeRateList = new List<ExchangeRate>();
            string line;

            this.SkipFileHeaders(reader);

            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('|');
                decimal value;
                decimal.TryParse(items[^1], out value);

                var exchangeRate = new ExchangeRate(new Currency(items[^2]), new Currency(targetCurrencyCode), value);
                exchangeRateList.Add(exchangeRate);
            }

            return exchangeRateList;
        }

        private void SkipFileHeaders(StreamReader reader)
        {
            reader.ReadLine();
            reader.ReadLine();
        }
    }
}
