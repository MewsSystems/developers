using System;
using System.Runtime.InteropServices;

namespace ExchangeRateUpdater.Common.Extensions
{
    public static class CzechTimeZoneExtensions
    {
        public static TimeZoneInfo GetCzechTimeZone()
        {
            var tzId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "Central Europe Standard Time"
                : "Europe/Prague";
            return TimeZoneInfo.FindSystemTimeZoneById(tzId);
        }

        public static DateTimeOffset GetNextCzechBankUpdateUtc()
        {
            var czechTimeZone = GetCzechTimeZone();
            var nowCzech = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, czechTimeZone);
            var nextUpdate = new DateTimeOffset(nowCzech.Year, nowCzech.Month, nowCzech.Day, 14, 30, 0, nowCzech.Offset);
            if (nowCzech >= nextUpdate) nextUpdate = nextUpdate.AddDays(1);
            return nextUpdate.ToUniversalTime();
        }

        public static DateTimeOffset GetNextMonthUpdateUtc(DateTimeOffset? now = null)
        {
            var czechTz = GetCzechTimeZone();
            var current = now ?? TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, czechTz);

            int year = current.Year;
            int month = current.Month;

            if (month == 12)
            {
                year += 1;
                month = 1;
            }
            else
            {
                month += 1;
            }

            // Create the local time for the next month's first day at midnight
            var nextMonthLocal = new DateTime(year, month, 1, 0, 0, 0);
            // Get the correct offset for that date (handles DST)
            var offset = czechTz.GetUtcOffset(nextMonthLocal);
            // Create the DateTimeOffset with the correct offset
            var nextMonth = new DateTimeOffset(nextMonthLocal, offset);

            // Convert to UTC for cache expiration
            return nextMonth.ToUniversalTime();
        }
    }
}