using ExchangeRateUpdater.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers;

public class CNBExchangeRateProvider : IExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CNBExchangeRateProvider> _logger;

    public CNBExchangeRateProvider(
        HttpClient httpClient,
        IConfiguration configuration,
        IMemoryCache cache,
        ILogger<CNBExchangeRateProvider> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> GetDailyExchangeRateAsync(IEnumerable<Currency> currencies, DateTime? date = null) 
    {
        if (!date.HasValue) date = DateTime.Today;

        string dateFormant = _configuration["CNBApi:DateFormat"];
        string cacheKey = $"ExchangeRates_{date?.ToString(dateFormant)}";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<ExchangeRate> cachedRates))
        {
            return cachedRates.Where(rateItem => currencies.Any(c => c.Code.Equals(rateItem.SourceCurrency.Code)));
        }
        
        try
        {
            string baseUrl = _configuration["CNBApi:BaseUrl"] + _configuration["CNBApi:ExchangeRateEndpoint"];
            string apiUrl = $"{baseUrl}daily?date={date?.ToString(dateFormant)}&lang=EN";
            string defaultCurrency = _configuration["CNBApi:DefaultCurrency"];

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var apiData = JsonConvert.DeserializeObject<ExchangeRateApiData>(responseContent);

                var exchangeRateItems = apiData.Rates
                    .Select(rateItem => new ExchangeRate(
                        new Currency(rateItem.CurrencyCode),
                        new Currency(defaultCurrency),
                        rateItem.ValidFor,
                        rateItem.Rate
                ));

                _logger.LogInformation("Fetched exchange rate succesfully");

                DateTime twoDaysAgo = DateTime.Today.AddDays(-2).Date;

                if (apiData.Rates.Any() && date?.Date >= twoDaysAgo)
                {
                    string newCacheKey = $"ExchangeRates_{exchangeRateItems.FirstOrDefault().ValidFor}";
                    UpdateCache(newCacheKey, exchangeRateItems);
                }

                return exchangeRateItems.Where(rateItem => currencies.Any(c => c.Code.Equals(rateItem.SourceCurrency.Code)));
            }
            else
            {
                _logger.LogError($"Failed to fetch exchange. Status code: {response.StatusCode}");
                throw new Exception("Failed to fetch exchange");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while fetching exchange rate {ex.Message}");
            throw;
        }
    }

    private void UpdateCache(string cacheKey, IEnumerable<ExchangeRate> data)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Today.AddDays(2)
        };
        _cache.Set(cacheKey, data, cacheEntryOptions);

        _logger.LogInformation($"Cache entry for {cacheKey} updated");
    }
}

public record ExchangeRateApiData
{
    public IEnumerable<ExchangeRateItem> Rates { get; set; }
}

public record ExchangeRateItem
{
    public string ValidFor { get; set; }
    public int Order { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public string CurrencyCode { get; set; }
    public decimal Rate { get; set; }
}
