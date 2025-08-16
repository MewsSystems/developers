using ExchangeRateUpdater.CnbApi;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Caching
{
    /// <summary>
    /// Provides information about expecting releasing dates and times of exchange rates. 
    /// </summary>
    public class ExchangeRateReleaseDatesProvider : IExchangeRateReleaseDatesProvider
    {
        private readonly TimeProvider _timeProvider;
        private readonly CnbApiOptions _config;
                
        public ExchangeRateReleaseDatesProvider(TimeProvider timeProvider, IOptions<CnbApiOptions> config)
        {
            _timeProvider = timeProvider;
            _config = config.Value;
        }

        /// <inheritdoc/>
        public DateOnly GetCurrentReleaseDate()
        {
            var utcNow = _timeProvider.GetUtcNow();

            var today = DateOnly.FromDateTime(utcNow.Date);

            return IsReleaseDay(today.DayOfWeek) && utcNow.TimeOfDay > _config.DailyReleaseTime
                ? today
                : GetPreviousReleaseDay(today);
        }

        /// <inheritdoc/>
        public TimeSpan GetTimeToNextRelease()
        {
            var utcNow = _timeProvider.GetUtcNow();
            var utcToday = DateOnly.FromDateTime(utcNow.Date);

            var nextReleaseTime = GetNextReleaseDay(utcToday).ToDateTime(TimeOnly.FromTimeSpan(_config.DailyReleaseTime));

            return nextReleaseTime.Subtract(utcNow.DateTime);
        }

        private static bool IsReleaseDay(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true
            };
        }

        private static DateOnly GetPreviousReleaseDay(DateOnly today)
        {
            return today.DayOfWeek switch
            {
                DayOfWeek.Sunday => today.AddDays(-2),
                DayOfWeek.Monday => today.AddDays(-3),
                _ => today.AddDays(-1),
            };
        }

        private static DateOnly GetNextReleaseDay(DateOnly today)
        {
            return today.DayOfWeek switch
            {
                DayOfWeek.Friday => today.AddDays(3),
                DayOfWeek.Saturday => today.AddDays(2),
                _ => today.AddDays(1),
            };
        }
    }
}
