using System;

namespace ExchangeRates.Infrastructure.Cache
{
    public static class CacheExpirationHelper
    {
        public static TimeSpan GetCacheExpirationToNextCzTime(TimeOnly dailyRefreshTime, DateTime? utcNow = null)
        {
            var nowUtc = utcNow ?? DateTime.UtcNow;
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, czTimeZone);

            var candidateRefreshCz = nowCz.Date.AddHours(dailyRefreshTime.Hour)
                                             .AddMinutes(dailyRefreshTime.Minute)
                                             .AddSeconds(dailyRefreshTime.Second);

            // Move to next business day if today is weekend or the time has passed
            var nextRefreshCz = GetNextBusinessDay(candidateRefreshCz);
            if (nowCz >= nextRefreshCz)
                nextRefreshCz = GetNextBusinessDay(nextRefreshCz.AddDays(1));

            var nextRefreshUtc = TimeZoneInfo.ConvertTimeToUtc(nextRefreshCz, czTimeZone);

            return nextRefreshUtc - nowUtc;
        }

        private static DateTime GetNextBusinessDay(DateTime date)
        {
            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
            }
            return date;
        }
    }
}
