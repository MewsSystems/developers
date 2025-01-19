using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.DTO;
using ExchangeRateUpdater.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateService(IHttpClientFactory clientFactory, ILogger<ExchangeRateService> logger, IMemoryCache memoryCache): IExchangeRateService
    {
        public async Task<ExchangeRatesDTO> GetExchangeRateAsync()
        {
            using var client = clientFactory.CreateClient("exchange");
            logger.LogInformation($"Sending GET request for getting exchange rates foon {DateTime.Now:yyyy-MM-dd}");
            var response = await client.GetAsync(client.BaseAddress + $"?date={DateTime.Now:yyyy-MM-dd}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Status code: {response.StatusCode}; Failed to fetch rates on {DateTime.Now:yyyy-MM-dd}");
                throw new Exception($"Status code: {response.StatusCode}; Failed to fetch rates on{DateTime.Now:yyyy-MM-dd}");
            }
            var result = await response.Content.ReadFromJsonAsync<ExchangeRatesDTO>();
            return result;
        }
    }
}
