using System.Runtime.InteropServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace ExchangeRateProvider.Infrastructure
{
    /// <summary>
    /// Configuration options for CNB cache strategy behavior.
    /// </summary>
    public sealed class CnbCacheOptions
    {
        /// <summary>
        /// Cache duration during CNB publication window (2:31 PM - 3:31 PM Prague time).
        /// Default: 5 minutes.
        /// </summary>
        public TimeSpan PublicationWindowDuration { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Cache duration during regular weekday hours.
        /// Default: 1 hour.
        /// </summary>
        public TimeSpan WeekdayDuration { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// Cache duration during weekends when no new data is published.
        /// Default: 12 hours.
        /// </summary>
        public TimeSpan WeekendDuration { get; set; } = TimeSpan.FromHours(12);

        /// <summary>
        /// Multiplier for fail-safe max duration (e.g., 2 means 2x normal duration).
        /// Default: 2.
        /// </summary>
        public double FailSafeMultiplier { get; set; } = 2.0;

        /// <summary>
        /// Start time of CNB publication window in Prague timezone.
        /// Default: 14:31 (2:31 PM).
        /// </summary>
        public TimeSpan PublicationWindowStart { get; set; } = new TimeSpan(14, 31, 0);

        /// <summary>
        /// End time of CNB publication window in Prague timezone.
        /// Default: 15:31 (3:31 PM).
        /// </summary>
        public TimeSpan PublicationWindowEnd { get; set; } = new TimeSpan(15, 31, 0);
    }

    /// <summary>
    /// Provides intelligent caching strategies for Czech National Bank (CNB) exchange rate data
    /// based on their publication schedule and Prague timezone.
    /// Implements robust error handling, configurability, and cross-platform timezone support.
    /// </summary>
    public class CnbCacheStrategy
    {
        private static readonly Lazy<TimeZoneInfo> LazyPragueTimeZone = new(() => ResolvePragueTimeZone());
        private readonly CnbCacheOptions _options;
        private readonly ILogger<CnbCacheStrategy>? _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CnbCacheStrategy"/> class.
        /// </summary>
        /// <param name="options">Configuration options for cache behavior.</param>
        /// <param name="logger">Optional logger for diagnostics.</param>
        public CnbCacheStrategy(IOptions<CnbCacheOptions>? options = null, ILogger<CnbCacheStrategy>? logger = null)
        {
            _options = options?.Value ?? new CnbCacheOptions();
            _logger = logger;
            ValidateOptions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CnbCacheStrategy"/> class with default options.
        /// Used for testing purposes.
        /// </summary>
        public CnbCacheStrategy()
        {
            _options = new CnbCacheOptions();
            _logger = null;
            ValidateOptions();
        }

        /// <summary>
        /// Determines optimal cache duration based on Prague time and CNB publication schedule.
        /// CNB publishes rates after 2:30 PM Prague time on working days only.
        /// 
        /// Cache Strategy:
        /// - Publication window (2:31 PM - 3:31 PM weekdays): Short duration for fresh data
        /// - Regular weekday hours: Medium duration for stable data
        /// - Weekends: Long duration as no new data is published
        /// </summary>
        /// <returns>Configured <see cref="FusionCacheEntryOptions"/> with appropriate duration and fail-safe settings.</returns>
        /// <exception cref="TimeZoneNotFoundException">Thrown when Prague timezone cannot be resolved.</exception>
        public FusionCacheEntryOptions GetCacheOptions()
        {
            try
            {
                var pragueTime = GetPragueTime();
                var cacheContext = DetermineCacheContext(pragueTime);
                var duration = GetDurationForContext(cacheContext);

                _logger?.LogDebug(
                    "Cache strategy determined: Context={Context}, Duration={Duration}, PragueTime={PragueTime}",
                    cacheContext, duration, pragueTime);

                return CreateCacheOptions(duration);
            }
            catch (Exception ex) when (ex is TimeZoneNotFoundException or InvalidTimeZoneException)
            {
                _logger?.LogWarning(ex, "Failed to resolve Prague timezone, falling back to default strategy");
                
                // Fallback: Use weekday strategy as safe default
                var fallbackDuration = _options.WeekdayDuration;
                return CreateCacheOptions(fallbackDuration);
            }
        }

        /// <summary>
        /// Gets current time in Prague timezone (Central European Time/Central European Summer Time).
        /// </summary>
        /// <returns>Current Prague time.</returns>
        /// <exception cref="TimeZoneNotFoundException">Thrown when Prague timezone cannot be found.</exception>
        private static DateTime GetPragueTime()
        {
            var pragueTimeZone = LazyPragueTimeZone.Value;
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, pragueTimeZone);
        }

        /// <summary>
        /// Determines the appropriate cache context based on Prague time.
        /// </summary>
        private CacheContext DetermineCacheContext(DateTime pragueTime)
        {
            if (IsWeekend(pragueTime))
            {
                return CacheContext.Weekend;
            }

            if (IsWithinPublicationWindow(pragueTime.TimeOfDay))
            {
                return CacheContext.PublicationWindow;
            }

            return CacheContext.RegularWeekday;
        }

        /// <summary>
        /// Checks if the given date falls on a weekend.
        /// </summary>
        private static bool IsWeekend(DateTime date) =>
            date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        /// <summary>
        /// Checks if the given time falls within the CNB publication window.
        /// </summary>
        private bool IsWithinPublicationWindow(TimeSpan time) =>
            time >= _options.PublicationWindowStart && time <= _options.PublicationWindowEnd;

        /// <summary>
        /// Gets the appropriate cache duration for the given context.
        /// </summary>
        private TimeSpan GetDurationForContext(CacheContext context) => context switch
        {
            CacheContext.PublicationWindow => _options.PublicationWindowDuration,
            CacheContext.RegularWeekday => _options.WeekdayDuration,
            CacheContext.Weekend => _options.WeekendDuration,
            _ => throw new ArgumentOutOfRangeException(nameof(context), context, "Unknown cache context")
        };

        /// <summary>
        /// Creates FusionCache options with the specified duration and fail-safe settings.
        /// </summary>
        private FusionCacheEntryOptions CreateCacheOptions(TimeSpan duration)
        {
            var failSafeDuration = TimeSpan.FromTicks((long)(duration.Ticks * _options.FailSafeMultiplier));

            return new FusionCacheEntryOptions
            {
                Duration = duration,
                FailSafeMaxDuration = failSafeDuration,
                // Enable jitter to prevent thundering herd scenarios
                JitterMaxDuration = TimeSpan.FromSeconds(30),
                // Set priority to normal for balanced eviction
                Priority = CacheItemPriority.Normal
            };
        }

        /// <summary>
        /// Resolves Prague timezone using cross-platform approach with fallback strategy.
        /// </summary>
        /// <returns>Prague timezone information.</returns>
        /// <exception cref="TimeZoneNotFoundException">Thrown when Prague timezone cannot be resolved on any platform.</exception>
        private static TimeZoneInfo ResolvePragueTimeZone()
        {
            // Primary attempt: Use platform-specific timezone identifiers
            var timeZoneIds = GetPragueTimeZoneIds();
            
            foreach (var timeZoneId in timeZoneIds)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                }
                catch (TimeZoneNotFoundException)
                {
                    // Continue to next identifier
                }
                catch (InvalidTimeZoneException)
                {
                    // Continue to next identifier
                }
            }

            // Fallback: Try to find by display name patterns
            var allTimeZones = TimeZoneInfo.GetSystemTimeZones();
            var pragueTimeZone = allTimeZones.FirstOrDefault(tz => 
                tz.DisplayName.Contains("Prague", StringComparison.OrdinalIgnoreCase) ||
                tz.DisplayName.Contains("Central Europe", StringComparison.OrdinalIgnoreCase) ||
                tz.Id.Contains("Prague", StringComparison.OrdinalIgnoreCase));

            if (pragueTimeZone != null)
            {
                return pragueTimeZone;
            }

            throw new TimeZoneNotFoundException(
                "Could not resolve Prague timezone. Attempted identifiers: " + 
                string.Join(", ", timeZoneIds));
        }

        /// <summary>
        /// Gets platform-specific timezone identifiers for Prague, ordered by preference.
        /// </summary>
        private static IEnumerable<string> GetPragueTimeZoneIds()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return ["Central Europe Standard Time", "Central European Standard Time"];
            }
            
            // Unix-like systems (Linux, macOS, etc.)
            return ["Europe/Prague", "CET", "Europe/Vienna"];
        }

        /// <summary>
        /// Validates configuration options and throws if invalid.
        /// </summary>
        private void ValidateOptions()
        {
            if (_options.PublicationWindowDuration <= TimeSpan.Zero)
                throw new ArgumentException("Publication window duration must be positive", nameof(_options.PublicationWindowDuration));

            if (_options.WeekdayDuration <= TimeSpan.Zero)
                throw new ArgumentException("Weekday duration must be positive", nameof(_options.WeekdayDuration));

            if (_options.WeekendDuration <= TimeSpan.Zero)
                throw new ArgumentException("Weekend duration must be positive", nameof(_options.WeekendDuration));

            if (_options.FailSafeMultiplier <= 1.0)
                throw new ArgumentException("Fail-safe multiplier must be greater than 1.0", nameof(_options.FailSafeMultiplier));

            if (_options.PublicationWindowStart >= _options.PublicationWindowEnd)
                throw new ArgumentException("Publication window start must be before end time", nameof(_options.PublicationWindowStart));
        }

        /// <summary>
        /// Represents different caching contexts based on CNB schedule.
        /// </summary>
        private enum CacheContext
        {
            /// <summary>Regular weekday hours outside publication window.</summary>
            RegularWeekday,
            /// <summary>Within CNB publication window (2:31 PM - 3:31 PM).</summary>
            PublicationWindow,
            /// <summary>Weekend when no new data is published.</summary>
            Weekend
        }
    }

    /// <summary>
    /// Extension methods for easier integration with dependency injection.
    /// </summary>
    public static class CnbCacheStrategyExtensions
    {
        /// <summary>
        /// Registers CNB cache strategy services with the DI container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configureOptions">Optional configuration action.</param>
        /// <returns>Service collection for chaining.</returns>
        public static IServiceCollection AddCnbCacheStrategy(
            this IServiceCollection services,
            Action<CnbCacheOptions>? configureOptions = null)
        {
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            services.AddSingleton<CnbCacheStrategy>();
            return services;
        }
    }
}