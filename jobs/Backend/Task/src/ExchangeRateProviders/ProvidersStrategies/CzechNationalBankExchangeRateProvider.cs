﻿using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers.ProvidersStrategies
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProviderStrategy
    {
        private readonly HttpClient httpClient;
        private const string BankUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string TargetCurrencyCode = "CZK";

        public CzechNationalBankExchangeRateProvider(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient("czech_httpClient");
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var response = await this.httpClient.GetAsync(BankUrl);

            if (!response.IsSuccessStatusCode)
            {
                return new List<ExchangeRate>();
            }

            var result = await response.Content.ReadAsStreamAsync();
            var reader = new StreamReader(result, System.Text.Encoding.UTF8);

            return this.ExchangeRatesBuilder(reader).Where(x => currencies.Any(c => c.Code == x.SourceCurrency.Code));
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

                var exchangeRate = new ExchangeRate(new Currency(items[^2]), new Currency(TargetCurrencyCode), value);
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