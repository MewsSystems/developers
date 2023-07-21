using ExchangeRate.Core.ExchangeRateSourceClients;
using ExchangeRate.Core.Providers.Interfaces;
using ExchangeRate.Core.Models;
using ExchangeRate.Core.Models.ClientResponses;
using ExchangeRate.Core.Services;
using ExchangeRate.Core.Constants;
using ExchangeRate.Core.Extentions;
using ExchangeRate.Core.Configuration;
using Microsoft.Extensions.Options;

namespace ExchangeRate.Core.Providers;

public class CnbExchangeRateProvider : CachedExchangeRateProviderBase, IExchangeRateProvider
{
    private const string DailyCacheKey = "Daily";

    private const string MonthlyCacheKey = "Monthly";

    private readonly IExchangeRateSourceClient<CnbExchangeRate> _cnbExchangeRateClient;

    private readonly IOptions<CnbSettings> _cnbSettings;

    public CnbExchangeRateProvider(
        IExchangeRateSourceClient<CnbExchangeRate> cnbExchangeRateClient,
        ICacheService cacheService,
        IOptions<CnbSettings> cnbSettings) 
            : base(cacheService, $"{ExchangeRateSourceCodes.CzechNationalBank}_Exng_Rate")
    {
        _cnbExchangeRateClient = cnbExchangeRateClient;
        _cnbSettings = cnbSettings;
    }

    /// <summary>
    /// Rerurns list of avilable exchange rates for provided currencies based on the Czech National Bank (CNB) sources.
    /// </summary>
    /// <param name="currencies">List of currencies for which to return the list of available exchange rates.</param>
    /// <returns>Returns the collection of <c>ExchangeRate.Contracts.Models.ExchangeRate</c>.</returns>
    public async Task<IEnumerable<Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        ArgumentNullException.ThrowIfNull(currencies);

        var dailyExchangeRates = await GetCacheAsync<IEnumerable<CnbExchangeRate>>(DailyCacheKey);
        if (dailyExchangeRates == null)
        {
            dailyExchangeRates = await _cnbExchangeRateClient.GetExchangeRatesAsync(_cnbSettings.Value.DailyExchangeRatesUrl);
            await SetCacheAsync(DailyCacheKey, dailyExchangeRates);
        }

        var exchangeRates = dailyExchangeRates
            .MapToExchangeRates()
            .ToList();

        if (exchangeRates.HasAllCurrencies(currencies))
        {
            return exchangeRates.FilterByCurrencies(currencies);
        }

        var monthlyExchangeRates = await GetCacheAsync<IEnumerable<CnbExchangeRate>>(MonthlyCacheKey);
        if (monthlyExchangeRates == null)
        {
            monthlyExchangeRates = await _cnbExchangeRateClient.GetExchangeRatesAsync($"{_cnbSettings.Value.MonthlyExchangeRateUrl}?yearMonth={DateTime.UtcNow.AddMonths(-1):yyyy-MM}");
            await SetCacheAsync(MonthlyCacheKey, monthlyExchangeRates);
        }

        exchangeRates.AddRange(monthlyExchangeRates.MapToExchangeRates());

        return exchangeRates.FilterByCurrencies(currencies);
    }
}
