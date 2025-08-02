using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Models.Cache;
using ExchangeRateUpdater.Models.Countries.CZE;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services.Countries.CZE;

public class CzeExchangeRateProvider : IExchangeRateProvider
{
    private const string BaseCurrencyCode = "CZK";
    private const string CacheKey = "CZE_data";
    private const string TimeZone = "Central European Standard Time";
    private readonly HttpClient _httpClient;
    public readonly CzeSettings _settings;
    private readonly ICacheService _cache;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CzeExchangeRateProvider> _logger;

    public CzeExchangeRateProvider(
        HttpClient httpClient,
        IOptionsSnapshot<CzeSettings> options,
        ICacheService cache,
        IDateTimeProvider dateTimeProvider,
        ILogger<CzeExchangeRateProvider> logger)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _cache = cache;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        _logger.LogInformation("Starting exchange rate retrieval.");

        if (string.IsNullOrWhiteSpace(_settings.BaseUrl))
        {
            _logger.LogWarning("Base URL is not configured. Aborting exchange rate retrieval.");
            return Enumerable.Empty<ExchangeRate>();
        }

        var cachedRates = await TryGetValidCachedRatesAsync(currencies);
        if (cachedRates is not null)
        {
            _logger.LogInformation("Valid cached exchange rates found. Returning cached data.");
            return cachedRates;
        }

        _logger.LogInformation("No valid cache found. Fetching exchange rates from remote source.");

        var latestRates = await FetchRatesFromCzechBankAsync();
        if (latestRates is null)
        {
            _logger.LogWarning("Failed to retrieve or deserialize exchange rates from remote source.");
            return Enumerable.Empty<ExchangeRate>();
        }

        _logger.LogInformation("Successfully fetched exchange rates. Caching results.");
        await CacheRatesAsync(latestRates);

        _logger.LogInformation("Returning newly fetched exchange rates.");
        return MapToExchangeRates(latestRates, currencies);
    }

    private async Task<IEnumerable<ExchangeRate>> TryGetValidCachedRatesAsync(IEnumerable<Currency> currencies)
    {
        var cached = await _cache.GetAsync<CacheObject<CzeExchangeRatesResponse>>(CacheKey);
        if (cached is null)
        {
            _logger.LogDebug("No cache entry found for exchange rates.");
            return null;
        }

        var updateCutoff = GetUpdateHourInUTC();
        if (cached.DataExtractionTimeUTC > updateCutoff)
        {
            _logger.LogDebug("Cached exchange rates are still valid (extracted at {ExtractionTime}, cutoff is {Cutoff}).",
                cached.DataExtractionTimeUTC, updateCutoff);
            return MapToExchangeRates(cached.Data, currencies);
        }

        _logger.LogDebug("Cached exchange rates are expired (extracted at {ExtractionTime}, cutoff is {Cutoff}).",
            cached.DataExtractionTimeUTC, updateCutoff);

        return null;
    }

    private async Task<CzeExchangeRatesResponse?> FetchRatesFromCzechBankAsync()
    {
        try
        {
            _logger.LogDebug("Sending HTTP request to {Url}.", _settings.BaseUrl);

            var response = await _httpClient.GetAsync(_settings.BaseUrl);
            response.EnsureSuccessStatusCode();

            _logger.LogDebug("Received successful HTTP response.");

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = XmlReader.Create(stream, new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse
            });

            var serializer = new XmlSerializer(typeof(CzeExchangeRatesResponse));
            var deserialized = serializer.Deserialize(reader) as CzeExchangeRatesResponse;

            _logger.LogDebug("Successfully deserialized exchange rates XML.");
            return deserialized;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching or parsing exchange rates from {Url}.", _settings.BaseUrl);
            return null;
        }
    }

    private async Task CacheRatesAsync(CzeExchangeRatesResponse rates)
    {
        var cacheObject = new CacheObject<CzeExchangeRatesResponse>
        {
            Data = rates,
            DataExtractionTimeUTC = _dateTimeProvider.UtcNow
        };

        var ttl = TimeSpan.FromSeconds(_settings.TtlInSeconds);
        await _cache.SetAsync(CacheKey, cacheObject, ttl);
        _logger.LogDebug("Exchange rates cached with TTL of {TtlSeconds} seconds.", _settings.TtlInSeconds);
    }

    private IEnumerable<ExchangeRate> MapToExchangeRates(
        CzeExchangeRatesResponse response,
        IEnumerable<Currency> requestedCurrencies)
    {
        var requestedCodes = requestedCurrencies.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);

        return response.Table.Rates
            .Where(rate => requestedCodes.Contains(rate.Code))
            .Select(rate =>
            {
                var source = new Currency(BaseCurrencyCode);
                var target = new Currency(rate.Code);
                var value = rate.Rate / rate.Amount;

                return new ExchangeRate(source, target, value);
            });
    }

    private DateTimeOffset GetUpdateHourInUTC()
    {
        TimeSpan time = TimeSpan.ParseExact(_settings.UpdateHourInLocalTime, "c", CultureInfo.InvariantCulture);
        var now = _dateTimeProvider.UtcNow;
        var localDate = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.FindSystemTimeZoneById(TimeZone)).Date;
        DateTime localDateTime = localDate.Add(time);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
        DateTimeOffset czechDateTimeOffset = new(localDateTime, timeZone.GetUtcOffset(localDateTime));
        return czechDateTimeOffset.ToUniversalTime();
    }
}
