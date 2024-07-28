using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.DTO.HttpClients;
using ExchangeRateUpdater.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.HttpClients
{
    public class CzechNationalBankClient : IExchangeRateRepository
    {
        private readonly ILogger<CzechNationalBankClient> _logger;
        private readonly HttpClient _httpClient;

        public CzechNationalBankClient(ILogger<CzechNationalBankClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string currencyCode, string requestUrl)
        {
            _logger.LogInformation("CzechNationalBankClient - GetExchangeRatesAsync - Sending request to {RequestUrl} to get {CurrencyCode} exchange rates", requestUrl, currencyCode);

            var response = await _httpClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("CzechNationalBankClient - GetExchangeRatesAsync - Received response content: {Content}", content);

            CNBExchangeRateResponse? data = JsonConvert.DeserializeObject<CNBExchangeRateResponse>(content);

            if (data == null)
            {
                _logger.LogInformation("CzechNationalBankClient - GetExchangeRatesAsync - Data object was null");
                return [];
            }
            if (data.Rates == null)
            {
                _logger.LogInformation("CzechNationalBankClient - GetExchangeRatesAsync - Rates collection on data object was null");                
                return [];
            }

            //According to original Exchange Rate object ToString method we're using SourceCurrency/TargetCurrency = Value
            //using USD as an example from the CNB API we have currency: USD, Amount: 1, Rate: 23.368
            //That means for the CNB API on this example we would have the Source as USD, Target as CZK, and Rate as 23.368
            var rates = data.Rates.Select(x => new ExchangeRate(new Currency(x.CurrencyCode), x.Amount, new Currency(currencyCode), x.Rate));

            _logger.LogInformation("CzechNationalBankClient - GetExchangeRatesAsync - {ExchangeRates} exchange rates getting returned", rates.Count());

            return rates;
        }
    }
}
