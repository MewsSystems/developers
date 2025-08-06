using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;

public class CnbExchangeRateProvider(ICzechNationalBankApiClient cnbApiClient, ICacheService cacheService, IOptions<CzechNationalBankExchangeRateConfig> exchangeRateConfig, ILogger<CnbExchangeRateProvider> logger) : IExchangeRateProvider
{
    public string Name => "CzechNationalBank";
    public string DefaultLanguage => "EN";
    public string DefaultCurrency => "CZK";
    private static TimeZoneInfo DefaultTimezone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
    
    public async Task<ExchangeRate[]> FetchAllCurrentAsync()
    {
        return await FetchByDateAsync(DateTime.UtcNow);
    }

    public async Task<ExchangeRate[]> FetchByDateAsync(DateTime date)
    {
        // If the date is in the future, use the current date
        if (date > DateTime.UtcNow)
        {
            date = DateTime.UtcNow;
        }
        
        date = TimeZoneInfo.ConvertTimeFromUtc(date, DefaultTimezone);
        
        var dailyCacheKey = CacheKeyGenerator.GenerateDailyRatesKey(Name, date);
        var monthlyCacheKey = CacheKeyGenerator.GenerateMonthlyRatesKey(Name, date);
        
        var cachedDailyRates = await cacheService.GetAsync<CnbExchangeRateResponse>(dailyCacheKey);
        var cachedMonthlyRates = await cacheService.GetAsync<CnbExchangeRateResponse>(monthlyCacheKey);

        if (cachedDailyRates != null && cachedMonthlyRates != null)
        {
            return FlattenAndConvertRatesToExchangeRates([cachedDailyRates, cachedMonthlyRates]);
        }

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
        return FlattenAndConvertRatesToExchangeRates(responses);
    }

    private async Task<CnbExchangeRateResponse> FetchAndCacheDailyRatesByDateAsync(DateTime date, string cacheKey)
    {
        var response = await cnbApiClient.GetFrequentExchangeRatesAsync(date.ToString("yyyy-MM-dd"));
        
        if (date.Date == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DefaultTimezone).Date)
        {
            await cacheService.SetAsync(cacheKey, response, date.Date.AddDays(1), null, null);
        }
        await cacheService.SetAsync(cacheKey, response, null, TimeSpan.FromMinutes(exchangeRateConfig.Value.Cache.DailyRatesAbsoluteExpirationInMinutes), TimeSpan.FromMinutes(exchangeRateConfig.Value.Cache.DailyRatesSlidingExpirationInMinutes));
        return response;
    }

    private async Task<CnbExchangeRateResponse> FetchAndCacheMonthlyRatesByDateAsync(DateTime date, string cacheKey)
    {
        var response = await cnbApiClient.GetOtherExchangeRatesAsync(date.ToString("yyyy-MM"));
        await cacheService.SetAsync(cacheKey, response, null, TimeSpan.FromMinutes(exchangeRateConfig.Value.Cache.MonthlyRatesAbsoluteExpirationInMinutes), TimeSpan.FromMinutes(exchangeRateConfig.Value.Cache.MonthlyRatesSlidingExpirationInMinutes));
        return response;
    }

    private static ExchangeRate[] FlattenAndConvertRatesToExchangeRates(CnbExchangeRateResponse[] responses)
    {
        return responses.SelectMany(fxModel => fxModel.Rates)
            .Select(rateModel => rateModel.ToExchangeRate()).ToArray();
    }
}
    