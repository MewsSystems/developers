using System;

namespace ExchangeRateUpdater.Common.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the previous month in "yyyy-MM" format, handling January correctly.
        /// </summary>
        public static string GetPreviousYearMonthUtc()
        {
            var now = DateTime.UtcNow;
            var prevMonth = now.Month == 1
                ? new DateTime(now.Year - 1, 12, 1)
                : new DateTime(now.Year, now.Month - 1, 1);
            return prevMonth.ToString("yyyy-MM");
        }
    }
}