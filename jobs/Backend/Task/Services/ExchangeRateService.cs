using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateRetriever _provider;
    private readonly IExchangeRateCache _memoryCache;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly IConfiguration _configuration;

    public ExchangeRateService(IConfiguration configuration, IExchangeRateRetriever provider, ILogger<ExchangeRateService> logger, IExchangeRateCache memoryCache)
    {
        _provider = provider;
        _logger = logger;
        _memoryCache = memoryCache;
        _configuration = configuration;
    }

    public async Task<Result<ExchangeRate[]>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var cachedRates = await _memoryCache.GetCachedExchangeRatesAsync();

        if (cachedRates == null || ShouldUpdateCache())
        {
            var result = await _provider.GetExchangeRatesAsync();
            if (!result.IsSuccess)
            {
                _logger.LogError($"Failed to fetch exchange rates: {result.Error}");
                return Result<ExchangeRate[]>.Failure(result.Error);
            }

            cachedRates = result.Value;

            await _memoryCache.SetExchangeRatesCacheAsync(cachedRates);
        }

        var filteredRates = FilterExchangeRates(currencies, cachedRates);

        return Result<ExchangeRate[]>.Success(filteredRates);
    }

    private ExchangeRate[] FilterExchangeRates(IEnumerable<Currency> currencies, ExchangeRate[] cachedRates)
    {
        var currencyCodes = new HashSet<string>();
        foreach (var currency in currencies)
        {
            currencyCodes.Add(currency.Code);
        }

        var filteredRates = cachedRates
            .Where(rate => currencyCodes.Contains(rate.TargetCurrency.Code))
            .ToArray();

        return filteredRates;
    }

    public bool ShouldUpdateCache()
    {
        if (!TimeSpan.TryParse(_configuration["BankSettings:DailyUpdate"], out var dailyUpdate))
        {
            return true;
        }

        var nextUpdate = DateTime.Today.Add(dailyUpdate);
        
        return DateTime.Now >= nextUpdate;
    }
}
