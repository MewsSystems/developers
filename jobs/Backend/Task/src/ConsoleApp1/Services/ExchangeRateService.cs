using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.Const;
using ExchangeRateUpdater.Domain.DTO;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateService(IHttpClientFactory clientFactory, ILogger<ExchangeRateService> _logger, IMemoryCache _memoryCache): IExchangeRateService
    {
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRateAsync()
        {
            var cacheConstant = MemoryCacheConstants.ExchangeRateKey(DateTime.Now.Date);
            if (_memoryCache.TryGetValue(cacheConstant, out IEnumerable<ExchangeRate> result))
            {
                _logger.LogInformation("Exchange rates retrieved from cache");
                return result;
            }
            using var client = clientFactory.CreateClient("exchange");
            _logger.LogInformation($"Sending GET request for getting exchange rates for {DateTime.Now:yyyy-MM-dd}");
            var response = await client.GetAsync(client.BaseAddress + $"?date={DateTime.Now:yyyy-MM-dd}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Status code: {response.StatusCode}; Failed to fetch rates on {DateTime.Now:yyyy-MM-dd}");
                throw new Exception($"Status code: {response.StatusCode}; Failed to fetch rates on{DateTime.Now:yyyy-MM-dd}");
            }
            result = (await response.Content.ReadFromJsonAsync<ExchangeRatesDTO>()).ToExchangeRates();
            if (result.Any())
            {
                _memoryCache.Set(cacheConstant, result, TimeSpan.FromSeconds(60));
            }
            return result;
        }
    }
}
