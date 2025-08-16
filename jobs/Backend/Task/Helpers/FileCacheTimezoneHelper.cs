using System;

namespace ExchangeRateUpdater.Helpers
{

    public static class FileCacheTimezoneHelper
    {
        private const string CEST_TIME_ZONE_ID = "Central European Standard Time";
        private const int CUTOFF_HOUR = 14;
        private const int CUTOFF_MINUTE = 30;

        public static string GetCacheKeyForDate(DateTime utcNow, string cacheIdentifier)
        {
            TimeZoneInfo cestTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CEST_TIME_ZONE_ID);
            DateTime currentCestTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, cestTimeZone);
            DateTime cutoffTime = new DateTime(currentCestTime.Year, currentCestTime.Month, currentCestTime.Day, CUTOFF_HOUR, CUTOFF_MINUTE, 0);

            if (currentCestTime.TimeOfDay < cutoffTime.TimeOfDay)
            {
                return $"{cacheIdentifier}_{currentCestTime.AddDays(-1):yyyyMMdd}";
            }
            return $"{cacheIdentifier}_{currentCestTime:yyyyMMdd}";
        }
    }
}
