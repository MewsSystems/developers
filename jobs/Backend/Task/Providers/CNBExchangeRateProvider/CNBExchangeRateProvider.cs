using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Providers
{
    internal class CNBExchangeRateProvider : IRateProvider
    {
        private const string cnbRatesMainUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string cnbRatesOthersUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year=2022&month=2";
        public string BaseCurrencyCode => "CZK";

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;


        public CNBExchangeRateProvider(HttpClient client, ILogger<CNBExchangeRateProvider> logger)
        {
            _httpClient = client;
            _logger = logger;
        }

        public CNBExchangeRateProvider(HttpClient client, ILogger logger)
        {
            _httpClient = client;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            _logger.LogDebug("Geting exhange rates");
            try
            {
                string mainRates = await FetchRates(cnbRatesMainUrl);
                string otherRates = await FetchRates(cnbRatesOthersUrl);

                var mainRatesParsed = ParseRates(mainRates);
                var otherRatesParsed = ParseRates(otherRates);

                return mainRatesParsed.Concat(otherRatesParsed);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get exhange rates ex:{ex} for {BaseCurrencyCode}");
                return null;
            }

        }
         
        private async Task<string> FetchRates(string url)
        {
            _logger.LogDebug($"Fetching rates from {url}");
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string rates = await response.Content.ReadAsStringAsync();
                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch rates ex: {ex}, from: {url}");
                return null;
            }

        }

        private IEnumerable<ExchangeRate> ParseRates(string rates)
        {
            _logger.LogDebug($"Parsing rates: {rates}");
            try
            { 
                var baseCurrency = new Currency(BaseCurrencyCode);

                IEnumerable<ExchangeRate> exchangeRates = rates.Replace(" ", string.Empty).Split("\n").Skip(2).Where(x => x.Length > 0).Select(x =>
                {
                    return ParseRate(x, baseCurrency);
                }).Where(x => x is not null);
                return exchangeRates;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to parse rates ex: {ex}, for rates: {rates}");
                return null;
            }
        }

        private ExchangeRate ParseRate(string rateAsString, Currency baseCurrency)
        {      
            try
            {
                var i = rateAsString.Split("|");
                var rate = decimal.Parse(i[4]);
                var anmount = int.Parse(i[2]);
                var actualRate = rate / anmount;
                return new ExchangeRate(baseCurrency, new Currency(i[3]), actualRate);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error parsing currency {ex.Message} for currency: {rateAsString}");
                return null;
            }

        }
    }
}
