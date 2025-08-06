using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;

public class CnbExchangeRateProvider(ICzechNationalBankApiClient cnbApiClient, ICacheService cacheService, IOptions<CzechNationalBankExchangeRateConfig> exchangeRateConfig, ILogger<CnbExchangeRateProvider> logger) : IExchangeRateProvider
{
    public string Name => "CzechNationalBank";
    public string DefaultLanguage => "EN";
    public string DefaultCurrency => "CZK";
    private static TimeZoneInfo DefaultTimezone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
    
    public async Task<ExchangeRate[]> FetchAllCurrentAsync()
    {
        logger.LogInformation("Fetching current exchange rates from {ProviderName}", Name);
        try
        {
            var result = await FetchByDateAsync(DateTime.UtcNow);
            logger.LogInformation("Successfully fetched {Count} current exchange rates from {ProviderName}", result.Length, Name);
            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "HTTP request failed while fetching current exchange rates from {ProviderName}. Status: {StatusCode}", Name, ex.StatusCode);
            return Array.Empty<ExchangeRate>();
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Request timeout while fetching current exchange rates from {ProviderName}", Name);
            return Array.Empty<ExchangeRate>();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unexpected error while fetching current exchange rates from {ProviderName}", Name);
            return Array.Empty<ExchangeRate>();
        }
    }

    public async Task<ExchangeRate[]> FetchByDateAsync(DateTime date)
    {
        logger.LogInformation("Fetching exchange rates from {ProviderName} for date {Date}", Name, date.ToString("yyyy-MM-dd"));
        
        try
        {
            // If the date is in the future, use the current date
            if (date > DateTime.UtcNow)
            {
                logger.LogWarning("Requested date {RequestedDate} is in the future, using current date", date.ToString("yyyy-MM-dd"));
                date = DateTime.UtcNow;
            }
            
            date = TimeZoneInfo.ConvertTimeFromUtc(date, DefaultTimezone);
            
            var dailyCacheKey = CacheKeyGenerator.GenerateDailyRatesKey(Name, date);
            var monthlyCacheKey = CacheKeyGenerator.GenerateMonthlyRatesKey(Name, date);
            
            logger.LogDebug("Generated cache keys - Daily: {DailyKey}, Monthly: {MonthlyKey}", dailyCacheKey, monthlyCacheKey);
            
            var cachedDailyRates = await cacheService.GetAsync<CnbExchangeRateResponse>(dailyCacheKey);
            var cachedMonthlyRates = await cacheService.GetAsync<CnbExchangeRateResponse>(monthlyCacheKey);

            if (cachedDailyRates != null && cachedMonthlyRates != null)
            {
                logger.LogInformation("Retrieved both daily and monthly rates from cache for {ProviderName}", Name);
                return FlattenAndConvertRatesToExchangeRates([cachedDailyRates, cachedMonthlyRates]);
            }

            logger.LogInformation("Cache miss for {ProviderName}. Daily cached: {DailyCached}, Monthly cached: {MonthlyCached}", 
                Name, cachedDailyRates != null, cachedMonthlyRates != null);

            var tasks = new List<Task<CnbExchangeRateResponse>>
            {
                cachedDailyRates == null
                    ? FetchAndCacheDailyRatesByDateAsync(date, dailyCacheKey)
                    : Task.FromResult(cachedDailyRates),
                cachedMonthlyRates == null
                    ? FetchAndCacheMonthlyRatesByDateAsync(date, monthlyCacheKey)
                    : Task.FromResult(cachedMonthlyRates)
            };

            var responses = await Task.WhenAll(tasks);
            var result = FlattenAndConvertRatesToExchangeRates(responses);
            
            logger.LogInformation("Successfully fetched {Count} exchange rates from {ProviderName} for date {Date}", 
                result.Length, Name, date.ToString("yyyy-MM-dd"));
            
            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "HTTP request failed while fetching exchange rates from {ProviderName} for date {Date}. Status: {StatusCode}", Name, date.ToString("yyyy-MM-dd"), ex.StatusCode);
            return Array.Empty<ExchangeRate>();
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Request timeout while fetching exchange rates from {ProviderName} for date {Date}", Name, date.ToString("yyyy-MM-dd"));
            return Array.Empty<ExchangeRate>();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unexpected error while fetching exchange rates from {ProviderName} for date {Date}", Name, date.ToString("yyyy-MM-dd"));
            return Array.Empty<ExchangeRate>();
        }
    }

    private async Task<CnbExchangeRateResponse> FetchAndCacheDailyRatesByDateAsync(DateTime date, string cacheKey)
    {
        logger.LogInformation("Fetching daily rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM-dd"));
        
        try
        {
            var response = await cnbApiClient.GetFrequentExchangeRatesAsync(date.ToString("yyyy-MM-dd"));
            logger.LogInformation("Successfully fetched daily rates from API for {ProviderName}. Rate count: {Count}", Name, response.Rates?.Length ?? 0);
            
            if (date.Date == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DefaultTimezone).Date)
            {
                logger.LogDebug("Caching current day rates with absolute expiration for {ProviderName}", Name);
                await cacheService.SetAsync(cacheKey, response, date.Date.AddDays(1), null, null);
            }
            
            logger.LogDebug("Caching daily rates with sliding expiration for {ProviderName}. Cache key: {CacheKey}", Name, cacheKey);
            await cacheService.SetAsync(cacheKey, response, null, TimeSpan.FromMinutes(exchangeRateConfig.Value.Cache.DailyRatesAbsoluteExpirationInMinutes), TimeSpan.FromMinutes(exchangeRateConfig.Value.Cache.DailyRatesSlidingExpirationInMinutes));
            
            return response;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "HTTP request failed while fetching daily rates from API for {ProviderName} on {Date}. Status: {StatusCode}", Name, date.ToString("yyyy-MM-dd"), ex.StatusCode);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Request timeout while fetching daily rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM-dd"));
            throw;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unexpected error while fetching daily rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM-dd"));
            throw;
        }
    }

    private async Task<CnbExchangeRateResponse> FetchAndCacheMonthlyRatesByDateAsync(DateTime date, string cacheKey)
    {
        logger.LogInformation("Fetching monthly rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM"));
        
        try
        {
            var response = await cnbApiClient.GetOtherExchangeRatesAsync(date.ToString("yyyy-MM"));
            logger.LogInformation("Successfully fetched monthly rates from API for {ProviderName}. Rate count: {Count}", Name, response.Rates?.Length ?? 0);
            
            logger.LogDebug("Caching monthly rates for {ProviderName}. Cache key: {CacheKey}", Name, cacheKey);
            await cacheService.SetAsync(cacheKey, response, null, TimeSpan.FromMinutes(exchangeRateConfig.Value.Cache.MonthlyRatesAbsoluteExpirationInMinutes), TimeSpan.FromMinutes(exchangeRateConfig.Value.Cache.MonthlyRatesSlidingExpirationInMinutes));
            
            return response;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "HTTP request failed while fetching monthly rates from API for {ProviderName} on {Date}. Status: {StatusCode}", Name, date.ToString("yyyy-MM"), ex.StatusCode);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Request timeout while fetching monthly rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM"));
            throw;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unexpected error while fetching monthly rates from API for {ProviderName} on {Date}", Name, date.ToString("yyyy-MM"));
            throw;
        }
    }

    private static ExchangeRate[] FlattenAndConvertRatesToExchangeRates(CnbExchangeRateResponse[] responses)
    {
        return responses.SelectMany(fxModel => fxModel.Rates)
            .Select(rateModel => rateModel.ToExchangeRate()).ToArray();
    }
}
    