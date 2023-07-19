using ExchangeRate.Core.ExchangeRateSourceClients;
using ExchangeRate.Core.Providers.Interfaces;
using ExchangeRate.Core.Models;
using ExchangeRate.Core.Models.ClientResponses;
using ExchangeRate.Core.Services;
using ExchangeRate.Core.Constants;

namespace ExchangeRate.Core.Providers;

public class CnbExchangeRateProvider : CachedExchangeRateProviderBase, IExchangeRateProvider
{
    private const string DailyCacheKey = "Daily";

    private const string MonthlyCacheKey = "Monthly";

    private const string SourceCurrencyCode = "CZK";

    private readonly IExchangeRateSourceClient<CnbExchangeRateResponse> _cnbExchangeRateClient;

    public CnbExchangeRateProvider(
        IExchangeRateSourceClient<CnbExchangeRateResponse> cnbExchangeRateClient,
        ICacheService cacheService) : base(cacheService, $"{ExchangeRateSourceCodes.CzechNationalBank}_Exng_Rate")
    {
        _cnbExchangeRateClient = cnbExchangeRateClient;
    }

    /// <summary>
    /// Rerurns list of avilable exchange rates for provided currencies based on the Czech National Bank (CNB) sources.
    /// </summary>
    /// <param name="currencies">List of currencies for which to return the list of available exchange rates.</param>
    /// <returns>Returns the collection of <c>ExchangeRate.Contracts.Models.ExchangeRate</c>.</returns>
    public async Task<IEnumerable<Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        ArgumentNullException.ThrowIfNull(currencies);

        var dailyExchangeRates = await GetCacheAsync<IEnumerable<CnbExchangeRateResponse>>(DailyCacheKey);
        if (dailyExchangeRates == null)
        {
            dailyExchangeRates = await _cnbExchangeRateClient.GetExchangeRatesAsync("/exrates/daily");
            await SetCacheAsync(DailyCacheKey, dailyExchangeRates);
        }

        var exchangeRates = dailyExchangeRates
            .Select(r => new Models.ExchangeRate(new Currency(SourceCurrencyCode), new Currency(r.CurrencyCode), (decimal)r.Rate))
            .ToList();

        if (currencies.All(c => exchangeRates.Any(r => string.Equals(r.SourceCurrency.Code, c.Code, StringComparison.InvariantCultureIgnoreCase))))
        {
            return exchangeRates
                .Where(r => currencies.Any(c => string.Equals(r.SourceCurrency.Code, c.Code, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();
        }

        var monthlyExchangeRates = await GetCacheAsync<IEnumerable<CnbExchangeRateResponse>>(MonthlyCacheKey);
        if (monthlyExchangeRates == null)
        {
            monthlyExchangeRates = await _cnbExchangeRateClient.GetExchangeRatesAsync("/fxrates/daily-month");
            await SetCacheAsync(MonthlyCacheKey, monthlyExchangeRates);
        }

        exchangeRates.AddRange(monthlyExchangeRates.Select(r => new Models.ExchangeRate(new Currency(SourceCurrencyCode), new Currency(r.CurrencyCode), (decimal)r.Rate)));

        return exchangeRates
                .Where(r => currencies.Any(c => string.Equals(r.SourceCurrency.Code, c.Code, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();
    }
}
