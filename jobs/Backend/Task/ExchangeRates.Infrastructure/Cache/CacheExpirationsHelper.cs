namespace ExchangeRates.Infrastructure.Cache
{
    public static class CacheExpirationHelper
    {
        public static TimeSpan GetCacheExpirationToNextCzTime(
            TimeOnly dailyRefreshTime,
            DateTime dataLastUpdated,
            DateTime? utcNow = null)
        {
            var nowUtc = utcNow ?? DateTime.UtcNow;
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, czTimeZone);

            var lastUpdateCz = TimeZoneInfo.ConvertTimeFromUtc(dataLastUpdated.Date, czTimeZone);
            var candidateRefreshCz = lastUpdateCz.Date
                                                  .AddDays(1)
                                                  .AddHours(dailyRefreshTime.Hour)
                                                  .AddMinutes(dailyRefreshTime.Minute)
                                                  .AddSeconds(dailyRefreshTime.Second);

            // Move to next business day if next refresh is weekend 
            var nextRefreshCz = GetNextBusinessDay(candidateRefreshCz);

            // If the current time has already passed the scheduled refresh, set the cache to expire in 5 minutes
            if (nowCz >= nextRefreshCz)
            {
                return TimeSpan.FromMinutes(5);
            }

            return nextRefreshCz - nowCz;
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
