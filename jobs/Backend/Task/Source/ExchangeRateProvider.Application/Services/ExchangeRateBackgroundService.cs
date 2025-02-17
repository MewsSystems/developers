namespace ExchangeRateProvider.Application.Services;

using Interfaces;
using Domain.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


/// <summary>
///     Background service that fetches and updates exchange rates in cache at a scheduled time.
/// </summary>
public class ExchangeRateBackgroundService(
    IMemoryCache cache,
    IExchangeRateProvider exchangeRateProvider,
    ILogger<ExchangeRateBackgroundService> logger,
    IOptions<CnbApiOptions> options) : BackgroundService
{
    private static readonly string CacheKey = "ExchangeRates";
    private readonly CnbApiOptions _options = options.Value;
    private Timer? _timer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await UpdateExchangeRatesAsync().ConfigureAwait(false);
        ScheduleNextUpdate();
    }

    private void ScheduleNextUpdate()
    {
        var now = DateTime.Now;
        var updateTime = new DateTime(now.Year, now.Month, now.Day, _options.UpdateHour, _options.UpdateMinute, 0);
        if (now > updateTime) updateTime = updateTime.AddDays(1);

        var delay = updateTime - now;
        logger.LogInformation(
            $"Next exchange rate update scheduled for tomorrow at {_options.UpdateHour}:{_options.UpdateMinute}.");
        _timer = new Timer(async void (_) => await UpdateExchangeRatesAsync().ConfigureAwait(false), null, delay,
            TimeSpan.FromDays(1));
    }

    private async Task UpdateExchangeRatesAsync()
    {
        try
        {
            logger.LogInformation("Fetching new exchange rates from CNB API...");

            // Make sure we clean the cache before updating
            if (cache.TryGetValue(CacheKey, out _)) cache.Remove(CacheKey);

            var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync().ConfigureAwait(false);

            if (exchangeRates.Any())
            {
                cache.Set(CacheKey, exchangeRates, TimeSpan.FromHours(_options.CacheDurationHours));
                logger.LogInformation("Exchange rates updated successfully.");
            }
            else
            {
                logger.LogWarning("Failed to retrieve exchange rates. The cache was not updated.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating exchange rates.");
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}
