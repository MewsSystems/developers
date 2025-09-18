using System.Text.Json;
using ExchangeRateUpdater.Domain.ApiClients.Interfaces;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Domain.Services.Implementations
{
    public sealed class ExchangeRateUpdaterProvider(
        IExchangeRateApiClient apiClient,
        IExchangeRateParser parser,
        IDistributedCache cache,
        ILogger<ExchangeRateUpdaterProvider> logger)
        : IExchangeRateProvider
    {
        private const string CacheKey = "ExchangeRates";

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesForCurrenciesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            List<ExchangeRate>? exchangeRatesPerCurrency = [];
            var currenciesContainer = currencies.ToArray();
            var targetCurrencyCodes = currenciesContainer.Select(x => x.Code).ToHashSet();
            
            if (targetCurrencyCodes.Count == 0)
            {
                logger.LogWarning("Input currencies should contain codes. Currently none is existing");
                return [];
            }
            
            try
            {
                var cached = await cache.GetStringAsync(CacheKey, token: cancellationToken);
                
                if (!string.IsNullOrEmpty(cached))
                    exchangeRatesPerCurrency = JsonSerializer.Deserialize<List<ExchangeRate>>(cached);
                
                if (exchangeRatesPerCurrency is not { Count: > 0 })
                {
                    var xmlRates = await apiClient.GetExchangeRatesXml(cancellationToken);
                    
                    var parsedRates = await parser.ParseAsync(xmlRates);

                    var exchangeRates = parsedRates as ExchangeRate[] ?? parsedRates.ToArray();
                    if (exchangeRates.Length <= 0)
                    {
                        logger.LogWarning("Parsed Rates yielding no values");
                        return [];
                    }
                    
                    var exchangeRatesSerialized = JsonSerializer.Serialize(exchangeRates);

                    if (string.IsNullOrEmpty(exchangeRatesSerialized))
                    {
                        logger.LogWarning("Parsed Rates serialized is empty. Returning zero results");
                        return [];
                    }
                        
                    await PersistDataIntoCache(exchangeRatesSerialized, cancellationToken);
                    
                    return exchangeRates.Where(x => targetCurrencyCodes.Contains(x.TargetCurrency.Code));
                }
                
            }
            catch (JsonException jsonException)
            {
                logger.LogError("Json Exception while deserializing Cached Entries : {JsonException}", jsonException);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("Unhandled exception : {Exception}", ex);
                throw;
            }

            return exchangeRatesPerCurrency.Where(x => targetCurrencyCodes.Contains(x.TargetCurrency.Code));
        }
        
        private async Task PersistDataIntoCache(string exchangeRatesSerialized, CancellationToken cancellationToken)
        {
            await cache.SetStringAsync(CacheKey, exchangeRatesSerialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            }, token: cancellationToken);
        }
    }
}
