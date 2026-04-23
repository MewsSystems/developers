using ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates.Models;
using ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider.Configuration;
using ExchangeRateDemo.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider
{
    public class ExchangeRateProvider(ILogger<ExchangeRateProvider> logger, ICacheService cacheService) : RestProvider, IExchangeRateProvider
    {
        public override string Name => ExchangeRateProviderConfiguration.Name;

        private const string GetExchangeRatesCacheKey = "GetExchangeRates-";

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(string date)
        {
            try
            {
                var rates = await cacheService.GetOrCreateAsync(
                   GetCacheKey(date),
                   async () =>
                   {
                       var httpResponse = await Client.GetAsync($"cnbapi/exrates/daily?date={date}&lang=EN");
                       httpResponse.EnsureSuccessStatusCode();

                       var response = await httpResponse.Content.ReadFromJsonAsync<ExchangeRateResponse>();

                       return response.Rates;
                   });

                return rates;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error when trying to retrieve exchange rates");
                return [];
            }

        }
        private static string GetCacheKey(string date) => $"{GetExchangeRatesCacheKey}-{date}";
    }
}
