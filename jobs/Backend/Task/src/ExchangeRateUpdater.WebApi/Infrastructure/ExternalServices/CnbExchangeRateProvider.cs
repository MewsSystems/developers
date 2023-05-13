using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.ExternalServices
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly Currency _sourceCurrency;

        public CnbExchangeRateProvider(HttpClient httpClient, string sourceCurrency)
        {
            _httpClient = httpClient;
            _sourceCurrency = new Currency(sourceCurrency);
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any()) throw new ArgumentException("Currencies must be provided", nameof(currencies));

            string response = await GetExchangeRatesFromApiAsync();

            IEnumerable<ExchangeRate> allExchangeRates = ParseExchangeRates(response, _sourceCurrency);

            return allExchangeRates.Where(rate => currencies.Any(currency => rate.TargetCurrency.Code == currency.Code)).ToList();            
        }

        private async Task<string> GetExchangeRatesFromApiAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("daily.txt");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private static IEnumerable<ExchangeRate> ParseExchangeRates(string response, Currency sourceCurrency)
        {            
            if(string.IsNullOrWhiteSpace(response))
                return Enumerable.Empty<ExchangeRate>();
                        
            var lines = response.Split('\n').Skip(2);
            var rates = new List<ExchangeRate>(lines.Count());


            return lines
                    .Select(line => line.Split('|')).Where(splittedLine => splittedLine.Length == 5)
                    .Select(values => new ExchangeRate(sourceCurrency, new Currency(values[3]), decimal.Parse(values[4], CultureInfo.InvariantCulture)));                    
        }       
    }
}
