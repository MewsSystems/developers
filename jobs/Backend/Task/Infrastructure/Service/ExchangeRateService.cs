using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Service
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly string _dailyExchangeRatesPath = "cnbapi/exrates/daily?lang=EN";
        private readonly string cacheKey = "DailyExchangeRates";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;

        private readonly JsonSerializerOptions _options =
            new()
            {
                PropertyNameCaseInsensitive = true
            };

        public ExchangeRateService(ILogger logger, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;            
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ExchangeRateResponse> GetDailyExchangeRates(CancellationToken cancellationToken)
        {
            var exchangeRateResponse = new ExchangeRateResponse();

            try
            {
                if (!_memoryCache.TryGetValue(cacheKey, out exchangeRateResponse))
                {
                    var httpClient = _httpClientFactory.CreateClient("CNBHttpClient");
                    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _dailyExchangeRatesPath);
                    var response = await httpClient.SendAsync(httpRequestMessage, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                        exchangeRateResponse = await JsonSerializer.DeserializeAsync<ExchangeRateResponse>
                            (stream, _options, cancellationToken);

                        _memoryCache.Set(cacheKey, exchangeRateResponse,
                            new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(CalculateExpiryTime()));
                    }
                    else
                    {
                        _logger.LogError($"{response.ReasonPhrase}: {response.StatusCode}");
                    }
                }

                _logger.LogDebug("Exchange rates successfully downloaded.");
                return exchangeRateResponse;                
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Unexpected error when downloading exchange rates.");
                throw;
            }
        }

        //Exchange rates are updated every day at 14:30.
        //Calculates how far we are from the next update time, in order to set the expiry time of cache.
        private TimeSpan CalculateExpiryTime()
        {
            var timeSpan = new TimeSpan();

            var now = DateTime.Now;
            var todayUpdateTime = new DateTime(now.Year, now.Month, now.Day, 14, 30, 0);

            if(now < todayUpdateTime)
                timeSpan = todayUpdateTime - now;
            else
                timeSpan = todayUpdateTime.AddDays(1) - now;

            return timeSpan;
        }
    }
}
