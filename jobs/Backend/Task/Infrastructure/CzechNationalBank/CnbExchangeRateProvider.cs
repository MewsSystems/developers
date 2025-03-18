using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;

        public CnbExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Provides exchange rates from the Czech National Bank.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            string apiUrl = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN"; //TODO: change to take a date that is passed by the service AND move to some config file

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode(); // TODO: What if not?

            var responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var cnbResponse = JsonSerializer.Deserialize<CnbExchangeRateResponse>(responseContent, options);


            return CnbExchangeRateMapper.Map(cnbResponse, currencies);
        }
    }

    // TODO: cache method - should it be part of this file, or part of another service? (DI) if part of this file, should it be part of the interface?
}

