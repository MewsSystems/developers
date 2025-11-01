namespace ExchangeRates.Infrastructure.Cache
{
    public static class CacheExpirationHelper
    {
        public static TimeSpan GetCacheExpirationToNextCzTime(TimeOnly expirationTime, DateTime? utcNow = null)
        {
            var nowUtc = utcNow ?? DateTime.UtcNow;
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, czTimeZone);

            var nextExpirationCz = nowCz.Date.AddHours(expirationTime.Hour)
                                             .AddMinutes(expirationTime.Minute)
                                             .AddSeconds(expirationTime.Second);

            if (nowCz >= nextExpirationCz)
                nextExpirationCz = nextExpirationCz.AddDays(1);

            var nextExpirationUtc = TimeZoneInfo.ConvertTimeToUtc(nextExpirationCz, czTimeZone);
            return nextExpirationUtc - nowUtc;
        }
    }

}
