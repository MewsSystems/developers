using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.DTO.Client;
using ExchangeRateUpdater.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Services
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

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
        {
            var requestUrl = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";

            _logger.LogInformation("CzechNationalBankClient - GetExchangeRatesAsync - Sending request to {RequestUrl}", requestUrl);

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

            var rates = data.Rates.Select(x => new ExchangeRate() { SourceCurrency = "CZK", TargetCurrency = x.CurrencyCode, Value = x.Rate });
            _logger.LogInformation("CzechNationalBankClient - GetExchangeRatesAsync - {ExchangeRates} exchange rates getting returned", rates.Count());

            return rates;
        }
    }
}
