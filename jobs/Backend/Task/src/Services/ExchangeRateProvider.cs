using System.Collections.Generic;
using System.Linq;
using System;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;
    private readonly ExchangeRateSourceResolver _sourceResolver;
    private readonly List<string> _allowedCurrencies;

    public ExchangeRateProvider(IMemoryCache cache, HttpClient httpClient, ExchangeRateSourceResolver sourceResolver, IConfiguration configuration)
    {
        _cache = cache;
        _httpClient = httpClient;
        _sourceResolver = sourceResolver;
        _allowedCurrencies = configuration.GetSection("AllowedCurrencies").Get<List<string>>();
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(List<Currency> currencies, Currency baseCurrency)
    {
        ValidateCurrencies(currencies, baseCurrency);

        var cacheKey = $"ExchangeRates_{baseCurrency.Code}";

        if (!_cache.TryGetValue(cacheKey, out List<ExchangeRate> rates))
        {
            ExchangeRateSource source;
            IExchangeRateParser parser;
            try
            {
                (source, parser) = _sourceResolver.ResolveSourceAndParser(baseCurrency);
            }

            catch (InvalidOperationException ex)
            {
                throw new ExternalExchangeRateApiException(ex.Message, ex);
            }
            try
            {
                var response = await _httpClient.GetStringAsync(source.Url);
                rates = parser.Parse(response, baseCurrency).ToList();
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalExchangeRateApiException($"Failed to retrieve exchange rates from {source.Url}", ex);
            }

            _cache.Set(cacheKey, rates, TimeSpan.FromMinutes(5));
        }

        var requestedCodes = new HashSet<string>(currencies.Select(c => c.Code));
        return rates.Where(r => requestedCodes.Contains(r.SourceCurrency.Code));
    }

    private void ValidateCurrencies(List<Currency> currencies, Currency baseCurrency)
    {
        if (!_allowedCurrencies.Contains(baseCurrency.Code.ToUpper()))
        {
            throw new ArgumentException($"Base currency '{baseCurrency}' is not allowed.");
        }

        var unallowedCurrencies = currencies.Where(c => !_allowedCurrencies.Contains(c.Code.ToUpper())).ToList();
        if (unallowedCurrencies.Any())
        {
            throw new ArgumentException($"The following currencies are not allowed: {string.Join(", ", unallowedCurrencies.Select(c => c.Code))}");
        }
    }
}

