using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeService : IExchangeService
    {
        private const string DAILY = "daily";
        private readonly HttpClient _httpClient;

        public ExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExchangeRateRecord>> GetExchangeRatesAsync()
        {
            var rates = await _httpClient.GetFromJsonAsync<ExchangeRatesResponse>(DAILY);
            if (rates == null)
            {
                //TODO This Exception must be replace with the custom one
                throw new Exception("Something went wrong. Please try again later");
            }

            return rates.ExchangeRates;
        }
    }
}
