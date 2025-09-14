using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Abstractions.Exceptions;
using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.CnbClient.Implementation;

/// <summary>
/// An exchange rates client strategy that uses caching to store and retrieve exchange rates.
/// </summary>
/// <param name="innerClient"></param>
/// <param name="cache"></param>
/// <param name="logger"></param>
public class HttpExchangeRatesClientWithCacheStrategy(
    HttpExchangeRatesClientStrategy innerClient,
    ICache<CurrencyValue> cache,
    ILogger<HttpExchangeRatesClientWithCacheStrategy> logger)
    : IExchangeRatesClientStrategy
{
    private readonly HttpExchangeRatesClientStrategy innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
    private readonly ICache<CurrencyValue> cache = cache ?? throw new ArgumentNullException(nameof(cache));

    /// <summary>
    /// Obtains the Exchange Rates, using cache when possible.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ExchangeRateNotFoundException"></exception>
    public async Task<IReadOnlyList<CurrencyValue>> GetExchangeRates()
    {
        if (cache.IsEmpty())
        {
            return await InitializeCacheAndReturnRates();
        }

        var rates = cache.GetAll();
        var isAnyRateExpired = rates.Any(rate => rate.ValidFor < DateTimeOffset.UtcNow);
        if (!isAnyRateExpired)
        {
            return cache.GetAll();
        }
        var result = await TryGetFreshRates();
        if (!result.sourceAvailable || result.isAnyValueExpired)
        {
            logger.LogWarning("Using expired cached exchange rates as the source is not available.");
        }
        return cache.GetAll();
    }

    /// <summary>
    /// Initializes the cache by attempting to fetch fresh rates, and returns the cached rates.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ExchangeRateNotFoundException"></exception>
    private async Task<IReadOnlyList<CurrencyValue>> InitializeCacheAndReturnRates()
    {
        var result = await TryGetFreshRates();
        if (!result.sourceAvailable)
        {
            throw new ExchangeRateNotFoundException("Exchange rates are not available from the source and no cached rates exist.");
        }

        if (result.isAnyValueExpired)
        {
            logger.LogWarning("Using expired cached exchange rates as the source is not available.");
        }
            
        return cache.GetAll();
    }

    /// <summary>
    /// Tries to get fresh exchange rates from the inner client and updates the cache.
    /// </summary>
    /// <returns></returns>
    private async Task<(bool sourceAvailable, bool isAnyValueExpired)> TryGetFreshRates()
    {
        try
        {
            var exchangeRates = (await innerClient.GetExchangeRates()).ToList();
            foreach (var exchangeRate in exchangeRates)
            {
                cache.Set(exchangeRate.CurrencyCode, exchangeRate);
            }

            var isAnyRateExpired = exchangeRates.Any(rate => rate.ValidFor < DateTimeOffset.UtcNow);
            return (true, isAnyRateExpired);
        }
        catch
        {
            return (false, false);
        }
    }
}