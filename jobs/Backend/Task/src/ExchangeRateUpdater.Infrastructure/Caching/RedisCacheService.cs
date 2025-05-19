using System.Text.Json;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodaTime;

namespace ExchangeRateUpdater.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ExchangeRateServiceOptions _options;

    public RedisCacheService(
        IDistributedCache cache,
        ILogger<RedisCacheService> logger,
        IDateTimeProvider dateTimeProvider,
        IOptions<ExchangeRateServiceOptions> options)
    {
        _cache = cache;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _options = options.Value;
    }

    public async Task<ExchangeRateData?> GetExchangeRatesAsync(LocalDate date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // If requesting today's rates before publication time, we should use the last business day's rates
            var todayDate = _dateTimeProvider.GetCurrentDate();
            var effectiveDate = date;

            if (date == todayDate && !_dateTimeProvider.IsPublicationTimePassedForDate(date))
            {
                _logger.LogDebug("Requesting today's rates before publication time, using last business day's rates");
                effectiveDate = _dateTimeProvider.GetLastWorkingDay(date.Minus(Period.FromDays(1)));
            }

            var key = GetCacheKey(effectiveDate);
            var cachedData = await _cache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
                return null;

            var data = JsonSerializer.Deserialize<ExchangeRateData>(cachedData);
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving exchange rates from cache for date {Date}", date);
            return null;
        }
    }

    public async Task SetExchangeRatesAsync(LocalDate date,
        ExchangeRateData data,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var key = GetCacheKey(date);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = GetCacheExpirationDate(date)
            };

            var serializedData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(key, serializedData, options, cancellationToken);
            _logger.LogInformation("Cached exchange rates for date {Date} until {Expiration}",
                date,
                options.AbsoluteExpiration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error caching exchange rates for date {Date}", date);
        }
    }

    public async Task RemoveExchangeRatesAsync(LocalDate date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var key = GetCacheKey(date);
            await _cache.RemoveAsync(key, cancellationToken);
            _logger.LogInformation("Removed cached exchange rates for date {Date}", date);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cached exchange rates for date {Date}", date);
        }
    }

    private string GetCacheKey(LocalDate date)
    {
        return $"{_options.CacheKeyName}:{date:yyyy-MM-dd}";
    }

    private DateTimeOffset GetCacheExpirationDate(LocalDate date)
    {
        var currentDate = _dateTimeProvider.GetCurrentDate();
        var currentDateTime = _dateTimeProvider.GetCurrentDateTime();
        var currentTime = currentDateTime.TimeOfDay;
        var publicationTime = new LocalTime(_options.PublicationHour, _options.PublicationMinute);

        // For today's date
        if (date == currentDate)
        {
            // If before publication time, cache until today's publication time
            if (currentTime < publicationTime)
            {
                var todayPublication = currentDate.At(publicationTime);
                return new DateTimeOffset(todayPublication.ToDateTimeUnspecified().ToUniversalTime());
            }
        }

        // For all other cases (including today after publication),
        // cache until the next business day's publication time
        var nextBusinessDay = _dateTimeProvider.GetNextBusinessDay(date);
        var expirationDate = nextBusinessDay.At(publicationTime);

        // Convert to DateTimeOffset for cache expiration
        var dateTimeOffset = expirationDate.ToDateTimeUnspecified().ToUniversalTime();
        var expirationOffset = new DateTimeOffset(dateTimeOffset);

        // Safety check - ensure we don't return a past date
        var now = DateTimeOffset.UtcNow;
        if (expirationOffset <= now)
        {
            // If expiration is in the past, set it to some time in the future
            // Use a fallback expiration of current time + CacheExpirationMinutes
            return now.AddMinutes(_options.CacheExpirationMinutes);
        }

        return expirationOffset;
    }
}