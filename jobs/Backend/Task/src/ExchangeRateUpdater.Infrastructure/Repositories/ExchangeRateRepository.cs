using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Repositories;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Repositories;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ExchangeRateUpdater.Infrastructure.Configuration;

namespace ExchangeRateUpdater.Infrastructure.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly Dictionary<string, IExchangeRateProvider> _providers;
    private readonly ICacheService _cacheService;
    private readonly ILogger<ExchangeRateRepository> _logger;
    private readonly IOptions<ExchangeRateProvidersConfig> _config;

    public ExchangeRateRepository(
        IEnumerable<IExchangeRateProvider> exchangeRateProviders, 
        ICacheService cacheService,
        IOptions<ExchangeRateProvidersConfig> config,
        ILogger<ExchangeRateRepository> logger)
    {
        _providers = exchangeRateProviders.ToDictionary(p => p.Name, p => p);
        _cacheService = cacheService;
        _config = config;
        _logger = logger;
    }

    public async Task<Dictionary<string, ExchangeRate[]>> FilterAsync(IEnumerable<Currency> currencies, DateTime? date = null)
    {
        var currencyArray = currencies as Currency[] ?? currencies.ToArray();
        var targetDate = date ?? DateTime.UtcNow;
        _logger.LogInformation("Filtering exchange rates for {CurrencyCount} currencies from {ProviderCount} providers for date {Date}", 
            currencyArray.Length, _providers.Count, targetDate.ToString("yyyy-MM-dd"));
        
        try
        {
            var tasks = _providers.Select(x => new { ExchangeRateProvider = x.Key, Task = GetCachedOrFetchRatesAsync(x.Value, targetDate) })
                .ToArray();
            
            _logger.LogDebug("Started fetching rates from {ProviderCount} providers", tasks.Length);
            
            try
            {
                await Task.WhenAll(tasks.Select(t => t.Task));
                _logger.LogDebug("All provider tasks completed");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Some provider tasks failed, continuing with successful ones");
            }

            var currenciesSet = new HashSet<Currency>(currencyArray);
            var successfulTasks = tasks.Where(t => t.Task.Status == TaskStatus.RanToCompletion).ToArray();
            
            _logger.LogInformation("Successfully retrieved rates from {SuccessfulCount}/{TotalCount} providers", successfulTasks.Length, tasks.Length);
            
            // Group successful results
            var result = successfulTasks
                .GroupBy(t => t.ExchangeRateProvider)
                .ToDictionary(
                    g => g.Key,
                    g => g.SelectMany(t => t.Task.Result).Where(x => currenciesSet.Count == 0 || currenciesSet.Contains(x.SourceCurrency)).ToArray()
                );
            
            var totalRates = result.Values.Sum(rates => rates.Length);
            _logger.LogInformation("Filtered {TotalRates} exchange rates from {ProviderCount} providers", totalRates, result.Count);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to filter exchange rates for currencies: {Currencies}", string.Join(", ", currencyArray.Select(c => c.Code)));
            throw;
        }
    }

    public async Task<Dictionary<string, ExchangeRate[]>> GetAllAsync(DateTime? date = null)
    {
        var targetDate = date ?? DateTime.UtcNow;
        _logger.LogInformation("Getting all exchange rates from {ProviderCount} providers for date {Date}", _providers.Count, targetDate.ToString("yyyy-MM-dd"));
        try
        {
            var result = await FilterAsync([], targetDate);
            _logger.LogInformation("Successfully retrieved all exchange rates from {ProviderCount} providers", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get all exchange rates, returning empty result");
            return new Dictionary<string, ExchangeRate[]>();
        }
    }

    public async Task<Dictionary<string, ExchangeRate[]>> GetFromProviderAsync(string providerName, IEnumerable<Currency> currencies, DateTime? date = null)
    {
        var currencyFilter = currencies as Currency[] ?? currencies.ToArray();
        var targetDate = date ?? DateTime.UtcNow;
        _logger.LogInformation("Getting exchange rates from provider '{ProviderName}' for {CurrencyCount} currencies for date {Date}", 
            providerName, currencyFilter.Length, targetDate.ToString("yyyy-MM-dd"));
        
        try
        {
            if (!_providers.TryGetValue(providerName, out var provider))
            {
                var availableProviders = string.Join(", ", _providers.Keys);
                _logger.LogError("Provider '{ProviderName}' not found. Available providers: {AvailableProviders}", providerName, availableProviders);
                throw new ArgumentException($"Provider '{providerName}' not found. Available providers: {availableProviders}");
            }

            _logger.LogDebug("Fetching rates from provider '{ProviderName}'", providerName);
            var rates = await GetCachedOrFetchRatesAsync(provider, targetDate);
            _logger.LogInformation("Retrieved {RateCount} rates from provider '{ProviderName}'", rates.Length, providerName);
            
            var currenciesSet = new HashSet<Currency>(currencyFilter);
            var filteredRates = rates.Where(x => currenciesSet.Contains(x.SourceCurrency)).ToArray();
            
            _logger.LogInformation("Filtered {FilteredCount}/{TotalCount} rates from provider '{ProviderName}' for requested currencies", 
                filteredRates.Length, rates.Length, providerName);
            
            return new Dictionary<string, ExchangeRate[]>
            {
                [providerName] = filteredRates
            };
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get exchange rates from provider '{ProviderName}' for currencies: {Currencies}, returning empty result", 
                providerName, string.Join(", ", currencyFilter.Select(c => c.Code)));
            return new Dictionary<string, ExchangeRate[]>();
        }
    }

    private async Task<ExchangeRate[]> GetCachedOrFetchRatesAsync(IExchangeRateProvider provider, DateTime date)
    {
        var providerDate = TimeZoneInfo.ConvertTimeFromUtc(date, provider.TimeZone).Date;
        var cacheKey = $"ExchangeRates:{provider.Name}:{providerDate:yyyy-MM-dd}";
        
        try
        {
            var cachedRates = await _cacheService.GetAsync<ExchangeRate[]>(cacheKey);
            if (cachedRates != null)
            {
                _logger.LogDebug("Retrieved rates from cache for provider '{ProviderName}' for date {Date}", provider.Name, providerDate.ToString("yyyy-MM-dd"));
                return cachedRates;
            }

            _logger.LogDebug("Cache miss for provider '{ProviderName}' for date {Date}, fetching from API", provider.Name, providerDate.ToString("yyyy-MM-dd"));
            var rates = await provider.FetchByDateAsync(providerDate);
            
            if (rates.Length > 0)
            {
                await CacheRatesWithTimezoneAwareExpiration(provider, cacheKey, rates, providerDate);
            }
            
            return rates;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get cached or fetch rates for provider '{ProviderName}' for date {Date}", provider.Name, providerDate.ToString("yyyy-MM-dd"));
            return [];
        }
    }

    private async Task CacheRatesWithTimezoneAwareExpiration(IExchangeRateProvider provider, string cacheKey, ExchangeRate[] rates, DateTime providerDate)
    {
        var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, provider.TimeZone).Date;
        
        if (providerDate.Date == today)
        {
            // Current day rates: expire at end of day in provider's timezone
            var endOfDay = today.AddDays(1);
            await _cacheService.SetAsync(cacheKey, rates, endOfDay, null, null);
            _logger.LogDebug("Cached {RateCount} current day rates for provider '{ProviderName}' until end of day {EndOfDay} in timezone {Timezone}", 
                rates.Length, provider.Name, endOfDay, provider.TimeZone.Id);
        }
        else
        {
            // Past/future rates: use sliding and absolute expiration from config
            var providerConfig = GetProviderCacheConfig(provider.Name);
            var absoluteExpiration = TimeSpan.FromMinutes(providerConfig.DailyRatesAbsoluteExpirationInMinutes);
            var slidingExpiration = TimeSpan.FromMinutes(providerConfig.DailyRatesSlidingExpirationInMinutes);
            
            await _cacheService.SetAsync(cacheKey, rates, null, absoluteExpiration, slidingExpiration);
            _logger.LogDebug("Cached {RateCount} rates for provider '{ProviderName}' with sliding expiration {SlidingExpiration} and absolute expiration {AbsoluteExpiration} in timezone {Timezone}", 
                rates.Length, provider.Name, slidingExpiration, absoluteExpiration, provider.TimeZone.Id);
        }
    }

    private CacheConfig GetProviderCacheConfig(string providerName)
    {
        // Try to get provider-specific cache configuration using string access
        var providerConfig = _config?.Value?[$"{providerName}.Cache"];
        
        // Return provider config if available, otherwise return default
        return providerConfig ?? new CacheConfig
        {
            DailyRatesAbsoluteExpirationInMinutes = 30,
            DailyRatesSlidingExpirationInMinutes = 10,
            MonthlyRatesAbsoluteExpirationInMinutes = 1440,
            MonthlyRatesSlidingExpirationInMinutes = 60
        };
    }
} 