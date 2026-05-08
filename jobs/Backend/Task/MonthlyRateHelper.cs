using System;

namespace ExchangeRateUpdater
{
    public static class MonthlyRateHelper
    {
        /// <summary>
        /// Returns the month (as "yyyy-MM") in which the active "other currency" CNB exchange rates were declared.
        /// According to the CNB, "other currency" exchange rates are declared on the last working day of the month,
        /// applying to the entire following month. This method converts the provided UTC time (or current time if
        /// none is supplied) to Central European Time (CET) and then returns the previous month, which corresponds
        /// to the declaration month used by the CNB.
        /// </summary>
        public static string GetDeclarationMonth(DateTime? nowUtc = null)
        {
            var cet = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCet = TimeZoneInfo.ConvertTimeFromUtc(nowUtc ?? DateTime.UtcNow, cet);

            var prevMonth = nowCet.AddMonths(-1);
            return $"{prevMonth:yyyy-MM}";
        }
    }
}