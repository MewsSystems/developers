using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;

public class CnbExchangeRateProvider(ICzechNationalBankApiClient cnbApiClient, ICacheService cacheService, IConfiguration configuration) : IExchangeRateProvider
{
    public string Name => "CzechNationalBank";
    public string DefaultLanguage => "EN";
    public string DefaultCurrency => "CZK";
    private static TimeZoneInfo DefaultTimezone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");

    // Cache configuration
    private CacheConfiguration CacheConfig => new()
    {
        DailyRatesExpiration = TimeSpan.FromHours(
            configuration.GetValue<int>("Caching:DailyRatesExpirationHours", 6)),
        MonthlyRatesExpiration = TimeSpan.FromDays(
            configuration.GetValue<int>("Caching:MonthlyRatesExpirationDays", 1))
    };

    public async Task<ExchangeRate[]> FetchAllAsync()
    {
        // Try to get from cache first
        var dailyCacheKey = CacheKeyGenerator.GenerateDailyRatesKey(Name);
        var monthlyCacheKey = CacheKeyGenerator.GenerateMonthlyRatesKey(Name);
        
        var cachedDailyRates = await cacheService.GetAsync<CnbExchangeRateResponse>(dailyCacheKey);
        var cachedMonthlyRates = await cacheService.GetAsync<CnbExchangeRateResponse>(monthlyCacheKey);

        // If both are cached, return combined results
        if (cachedDailyRates != null && cachedMonthlyRates != null)
        {
            return ConvertRatesToExchangeRates([cachedDailyRates, cachedMonthlyRates]);
        }

        // Fetch missing data from API
        var tasks = new List<Task<CnbExchangeRateResponse>>();
        
        if (cachedDailyRates == null)
        {
            tasks.Add(FetchAndCacheDailyRatesAsync(dailyCacheKey));
        }
        else
        {
            tasks.Add(Task.FromResult(cachedDailyRates));
        }

        if (cachedMonthlyRates == null)
        {
            tasks.Add(FetchAndCacheMonthlyRatesAsync(monthlyCacheKey));
        }
        else
        {
            tasks.Add(Task.FromResult(cachedMonthlyRates));
        }

        var responses = await Task.WhenAll(tasks);
        return ConvertRatesToExchangeRates(responses);
    }

    public async Task<ExchangeRate[]> FetchByDateAsync(DateTime date)
    {
        date = TimeZoneInfo.ConvertTimeFromUtc(date, DefaultTimezone);
        
        // For specific dates, use date-specific cache keys
        var dailyCacheKey = CacheKeyGenerator.GenerateDailyRatesKey(Name, date);
        var monthlyCacheKey = CacheKeyGenerator.GenerateMonthlyRatesKey(Name, date);
        
        var cachedDailyRates = await cacheService.GetAsync<CnbExchangeRateResponse>(dailyCacheKey);
        var cachedMonthlyRates = await cacheService.GetAsync<CnbExchangeRateResponse>(monthlyCacheKey);

        if (cachedDailyRates != null && cachedMonthlyRates != null)
        {
            return ConvertRatesToExchangeRates([cachedDailyRates, cachedMonthlyRates]);
        }

        var tasks = new List<Task<CnbExchangeRateResponse>>();
        
        if (cachedDailyRates == null)
        {
            tasks.Add(FetchAndCacheDailyRatesByDateAsync(date, dailyCacheKey));
        }
        else
        {
            tasks.Add(Task.FromResult(cachedDailyRates));
        }

        if (cachedMonthlyRates == null)
        {
            tasks.Add(FetchAndCacheMonthlyRatesByDateAsync(date, monthlyCacheKey));
        }
        else
        {
            tasks.Add(Task.FromResult(cachedMonthlyRates));
        }

        var responses = await Task.WhenAll(tasks);
        return ConvertRatesToExchangeRates(responses);
    }

    private async Task<CnbExchangeRateResponse> FetchAndCacheDailyRatesAsync(string cacheKey)
    {
        var response = await cnbApiClient.GetFrequentExchangeRatesAsync();
        await cacheService.SetAsync(cacheKey, response, CacheConfig.DailyRatesExpiration);
        return response;
    }

    private async Task<CnbExchangeRateResponse> FetchAndCacheMonthlyRatesAsync(string cacheKey)
    {
        var response = await cnbApiClient.GetOtherExchangeRatesAsync();
        await cacheService.SetAsync(cacheKey, response, CacheConfig.MonthlyRatesExpiration);
        return response;
    }

    private async Task<CnbExchangeRateResponse> FetchAndCacheDailyRatesByDateAsync(DateTime date, string cacheKey)
    {
        var response = await cnbApiClient.GetFrequentExchangeRatesAsync(date.ToString("yyyy-MM-dd"));
        await cacheService.SetAsync(cacheKey, response, CacheConfig.DailyRatesExpiration);
        return response;
    }

    private async Task<CnbExchangeRateResponse> FetchAndCacheMonthlyRatesByDateAsync(DateTime date, string cacheKey)
    {
        var response = await cnbApiClient.GetOtherExchangeRatesAsync(date.ToString("yyyy-MM"));
        await cacheService.SetAsync(cacheKey, response, CacheConfig.MonthlyRatesExpiration);
        return response;
    }

    private static ExchangeRate[] ConvertRatesToExchangeRates(CnbExchangeRateResponse[] responses)
    {
        return responses.SelectMany(fxModel => fxModel.Rates)
            .Select(rateModel => rateModel.ToExchangeRate()).ToArray();
    }
}
    