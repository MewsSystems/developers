using System;

namespace ExchangeRateUpdater.Helpers
{
    public static class CacheHelpers
    {
        /// <summary>
        /// Calculates the expiration time span for the cache entry. Calculation is based on the rates being updated at 14:30pm every working day.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetExpirationTimeSpan(DateTime dateTime)
        {
            TimeZoneInfo czechTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");

            DateTime czechNow = TimeZoneInfo.ConvertTime(dateTime, czechTimeZone);

            TimeSpan updateTime = new TimeSpan(14, 30, 0);

            DateTime expirationDate = czechNow.TimeOfDay < updateTime ? czechNow.Date : czechNow.Date.AddDays(1);

            TimeSpan expiration = (expirationDate.Date + updateTime) - czechNow;

            return expiration;
        }
    }
}
