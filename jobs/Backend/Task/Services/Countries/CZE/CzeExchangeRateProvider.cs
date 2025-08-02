using System;
using System.Collections;
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

    public CzeExchangeRateProvider(
        HttpClient httpClient,
        IOptionsSnapshot<CzeSettings> options,
        ICacheService cache)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _cache = cache;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        if (string.IsNullOrWhiteSpace(_settings.BaseUrl))
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        var existing = await _cache.GetAsync<CacheObject<CzeExchangeRatesResponse>>(CacheKey);
        if (existing != null)
        {
            var updateTime = GetUpdateHourInUTC();
            if (existing.DataExtractionTimeUTC > updateTime)
            {
                return MapToExchangeRates(existing.Data, currencies);
            }
        }

        var response = await _httpClient.GetAsync(_settings.BaseUrl);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = XmlReader.Create(stream, new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Parse
        });

        var serializer = new XmlSerializer(typeof(CzeExchangeRatesResponse));
        var result = (CzeExchangeRatesResponse)serializer.Deserialize(reader);

        if (result is not null && result.Table is not null && result.Table.Rates.Any())
        {
            var cacheObject = new CacheObject<CzeExchangeRatesResponse>
            {
                Data = result,
                DataExtractionTimeUTC = DateTimeOffset.UtcNow,
            };
            await _cache.SetAsync(CacheKey, cacheObject, TimeSpan.FromSeconds(_settings.TtlInSeconds));
            return MapToExchangeRates(result, currencies);
        }

        return Enumerable.Empty<ExchangeRate>();
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
        DateTime localDateTime = DateTime.Today.Add(time);
        TimeZoneInfo czechTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
        DateTimeOffset czechDateTimeOffset = new(localDateTime, czechTimeZone.GetUtcOffset(localDateTime));
        return czechDateTimeOffset.ToUniversalTime();
    }
}
