using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Provider.Cnb.Client;
using ExchangeRateUpdater.Provider.Cnb.Dtos;
using ExchangeRateUpdater.Services.Options;
using ExchangeRateUpdater.Services.Time;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ExchangeRateUpdater.Services.Refreshing
{
    public class CnbDailyRefreshService : BackgroundService
    {
        private readonly ICnbClient _cnbClient;
        private readonly IRatesStore _ratesStore;
        private readonly CnbRefreshOptions _refreshOptions;
        private readonly IAppClock _appClock;
        private readonly ILogger<CnbDailyRefreshService> _logger;
        private readonly IHostApplicationLifetime _lifetime;

        public CnbDailyRefreshService(
            ICnbClient cnbClient,
            IRatesStore ratesStore,
            IOptions<CnbRefreshOptions> refreshOptions,
            IAppClock appClock,
            ILogger<CnbDailyRefreshService> logger,
            IHostApplicationLifetime lifetime)
        {
            _cnbClient = cnbClient ?? throw new ArgumentNullException(nameof(cnbClient));
            _ratesStore = ratesStore ?? throw new ArgumentNullException(nameof(ratesStore));
            _refreshOptions = refreshOptions?.Value ?? 
                throw new ArgumentNullException(nameof(refreshOptions));
            _appClock = appClock ?? throw new ArgumentNullException(nameof(appClock));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            // initial refresh on startup
            var initialOk = await TryRefreshAsync();
            if (!initialOk)
            {
                _logger.LogCritical("Initial CNB refresh failed. Stopping host.");
                _lifetime.StopApplication();
                return;
            }

            try
            {
                while (!ct.IsCancellationRequested)
                {
                    var now = GetLocalNow();
                    var today = DateOnly.FromDateTime(now);

                    if (!IsWithinWindow(now))
                    {
                        await Task.Delay(GetDelayUntilNextWindow(now), ct);
                        continue;
                    }

                    var storeRates = _ratesStore.Get();
                    if (storeRates == null || storeRates.GetValidForDate() != today)
                    {
                        await TryRefreshAsync();

                        var storeRatesAfterRefresh = _ratesStore.Get();
                        var haveToday = storeRatesAfterRefresh != null 
                            && storeRatesAfterRefresh.GetValidForDate() == today;
                        var delay =
                            haveToday ? GetDelayUntilNextWindow(now) : _refreshOptions.RetryInterval;

                        await Task.Delay(delay, ct);
                        continue;
                    }

                    await Task.Delay(GetDelayUntilNextWindow(now), ct);
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.LogInformation("CnbDailyRefreshService is stopping due to cancellation.");
            }
        }

        private TimeSpan GetDelayUntilNextWindow(DateTime localNow)
        {
            var startToday = localNow.Date + _refreshOptions.FirstAttemptAt;

            DateTime target;
            if (localNow < startToday)
            {
                target = startToday;
            }
            else
            {
                target = localNow.Date.AddDays(1) + _refreshOptions.FirstAttemptAt;
            }

            while (IsWeekend(target)) 
            {
                target = target.Date.AddDays(1) + _refreshOptions.FirstAttemptAt;
            }

            var delay = target - localNow;
            return delay;
        }

        private async Task<bool> TryRefreshAsync()
        {
            CnbResponse? resp;
            try
            {
                resp = await _cnbClient.GetDailyRatesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CNB call failed.");
                return false;
            }

            if (resp?.Rates == null || resp.Rates.Count == 0)
            {
                _logger.LogError("CNB returned no rates.");
                return false;
            }

            _ratesStore.SetIfNewer(resp);
            _logger.LogInformation(
                $"CNB rates fetched. ValidFor={resp.GetValidForDate()} Count={resp.Rates.Count}");
            return true;
        }

        private bool IsWithinWindow(DateTime localNow)
        {
            if (IsWeekend(localNow)) return false;

            var currentTime = localNow.TimeOfDay;
            var windowStart = _refreshOptions.FirstAttemptAt;
            var windowEnd = _refreshOptions.CutoffAt;

            if (windowStart <= windowEnd)
                return currentTime >= windowStart && currentTime <= windowEnd;

            // window passes midnight
            return currentTime >= windowStart || currentTime <= windowEnd;
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        }

        private DateTime GetLocalNow()
        {           
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_refreshOptions.TimeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(_appClock.UtcNow, timeZone);
        }       
    }
}