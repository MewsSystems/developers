
namespace ExchangeRatesGetterWorkerService.Helpers
{
    /// <summary>   A date time helper. </summary>
    ///
    /// <remarks>   , 13.08.2023. </remarks>

    public class DateTimeHelper
    {
        private static TimeSpan publishingMainRatesCetTime = TimeSpan.FromHours(14.5);
        /// <summary>   The cest zone. </summary>
        private static TimeZoneInfo cestZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

        /// <summary>   Gets cest time from UTC time. </summary>
        ///
        ///
        /// <param name="utcTime">  The UTC time. </param>
        ///
        /// <returns>   The cest time from UTC time. </returns>
        public static DateTime GetCestTimeFromUtcTime(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, cestZone);
        }

        /// <summary>   Gets main currencies last publication date. </summary>
        ///
        ///
        /// <returns>   The main currencies last publication date. </returns>
        public static DateTime GetMainCurrenciesLastPublicationDate()
        {
            DateTime currentDate = GetCestTimeFromUtcTime(DateTime.UtcNow);
            if (!IsWorkingDay(currentDate))
            {
                while (!IsWorkingDay(currentDate))
                {
                    currentDate = currentDate.AddDays(-1);
                }
                return currentDate;
            }

            bool isRatesPublishedToday = currentDate.TimeOfDay > publishingMainRatesCetTime;
            currentDate = isRatesPublishedToday ? currentDate : currentDate.AddDays(-1);
            if (!IsWorkingDay(currentDate))
            {
                while (!IsWorkingDay(currentDate))
                {
                    currentDate = currentDate.AddDays(-1);
                }
            }
            return currentDate;

        }

        /// <summary>   Query if 'date' is working day. </summary>
        ///
        ///
        /// <param name="date"> The date Date/Time. </param>
        ///
        /// <returns>   True if working day, false if not. </returns>
        public static bool IsWorkingDay(DateTime date)
        {
            bool isWeekend = date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
            if (isWeekend) { return false; }

            foreach (DateTime holiday in GetPublicHolidaysCurrentYear())
            {
                if (date.Date == holiday.Date)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>   Gets public holidays current year. </summary>
        ///
        ///
        /// <returns>   An array of date time. </returns>
        public static DateTime[] GetPublicHolidaysCurrentYear()
        {

            /*   
             *   ToDo: Implement parsing Bank holidays from CNB website or implement interface for inserting
             *   the bank holidays manually
             */

            DateTime[] holidays2023 = new DateTime[]
{
                    new DateTime(2023, 01, 01),
                    new DateTime(2023, 04, 07),
                    new DateTime(2023, 04, 10),
                    new DateTime(2023, 05, 01),
                    new DateTime(2023, 05, 08),
                    new DateTime(2023, 07, 05),
                    new DateTime(2023, 07, 06),
                    new DateTime(2023, 09, 28),
                    new DateTime(2023, 10, 28),
                    new DateTime(2023, 11, 17),
                    new DateTime(2023, 12, 24),
                    new DateTime(2023, 12, 25),
                    new DateTime(2023, 12, 26)

};
            return holidays2023;
        }
    }
}
