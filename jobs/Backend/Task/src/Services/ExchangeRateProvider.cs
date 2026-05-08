using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;
    private readonly IExchangeRateSettingsResolver _settingsResolver;
    private readonly List<string> _allowedCurrencies;

    public ExchangeRateProvider(IMemoryCache cache, HttpClient httpClient, IExchangeRateSettingsResolver settingsResolver, IConfiguration configuration)
    {
        _cache = cache;
        _httpClient = httpClient;
        _settingsResolver = settingsResolver;
        _allowedCurrencies = configuration.GetSection("AllowedCurrencies").Get<List<string>>();
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(List<Currency> currencies, Currency baseCurrency)
    {
        ValidateCurrencies(currencies, baseCurrency);

        var cacheKey = $"ExchangeRates_{baseCurrency.Code}";

        if (!_cache.TryGetValue(cacheKey, out List<ExchangeRate> rates))
        {
            ExchangeRateSettings settings;
            try
            {
                settings = _settingsResolver.ResolveSourceSettings(baseCurrency);
            }

            catch (InvalidOperationException ex)
            {
                throw new ExchangeRateApiException(ex.Message, ex);
            }
            try
            {
                var response = await _httpClient.GetStringAsync(settings.Url);
                rates = settings.Parser.Parse(response, baseCurrency).ToList();
            }
            catch (HttpRequestException ex)
            {
                throw new ExchangeRateApiException($"Failed to retrieve exchange rates from {settings.Url}", ex);
            }
            _cache.Set(cacheKey, rates, TimeSpan.FromMinutes(5));
        }

        var requestedCodes = new HashSet<string>(currencies.Select(c => c.Code));
        return rates.Where(r => requestedCodes.Contains(r.TargetCurrency.Code));
    }

    private void ValidateCurrencies(List<Currency> currencies, Currency baseCurrency)
    {
        if (!_allowedCurrencies.Contains(baseCurrency.Code.ToUpper()))
        {
            throw new ArgumentException($"Base currency '{baseCurrency}' is not allowed.");
        }

        if (!currencies.Any())
        {
            throw new ArgumentException("Target currencies cannot be empty.");
        }

        var unallowedCurrencies = currencies.Where(c => !_allowedCurrencies.Contains(c.Code.ToUpper())).ToList();
        if (unallowedCurrencies.Any())
        {
            throw new ArgumentException($"The following currencies are not allowed: {string.Join(", ", unallowedCurrencies.Select(c => c.Code))}");
        }
    }
}

