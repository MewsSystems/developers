using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    ///<inheritDoc/>
    internal class BankDateProvider : IBankDateProvider
    {
        // TODO consider also local time offset and light saving time
        private static readonly TimeOnly _NewDailyListingTime = new TimeOnly(14, 30);

        // TODO use a library for national holiday dates (currently Easters are missing)
        private static readonly ICollection<DateOnly> _NationalHolidaysDates = new DateOnly[] {
            new DateOnly(1, 1, 1), new DateOnly(1,5,1), new DateOnly(1,5,8), new DateOnly(1,6,5),
            new DateOnly(1,6,6), new DateOnly(1,9,28), new DateOnly(1,10,28), new DateOnly(1,11,17),
            new DateOnly(1,12,24), new DateOnly(1,12,25),new DateOnly(1,12,26)
        };

        ///<inheritDoc/>
        public DateOnly GetDailyListingBankDateForDateTime(DateTime requestedDateTime) {
            var isBeforeTodaysListing = TimeOnly.FromDateTime(requestedDateTime) < _NewDailyListingTime;
            if (isBeforeTodaysListing) {
                requestedDateTime = requestedDateTime.AddDays(-1);
            }

            return GetLastWorkingDayDate(DateOnly.FromDateTime(requestedDateTime));
        }

        ///<inheritDoc/>
        public DateOnly GetMonthlyListingBankDateForDateTime(DateTime requestedDateTime) {
            var previousMonthDate = requestedDateTime.AddMonths(-1);
            var previousMonthDaysCount = DateTime.DaysInMonth(previousMonthDate.Year, previousMonthDate.Month);
            var lastDayOfPreviousMonth = new DateOnly(previousMonthDate.Year, previousMonthDate.Month, previousMonthDaysCount);
            return GetLastWorkingDayDate(lastDayOfPreviousMonth);
        }

        /// <summary>
        /// Gets latest work day date which is on or before given date.
        /// </summary>
        /// <param name="date">Starting date.</param>
        /// <returns>Work day date.</returns>
        private DateOnly GetLastWorkingDayDate(DateOnly date) {
            while (IsWeekendDay(date) || IsNationalHoliday(date)) {
                date = date.AddDays(-1);
            }

            return date;
        }

        private bool IsWeekendDay(DateOnly date) {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        private bool IsNationalHoliday(DateOnly date) {
            var dateWithoutYear = new DateOnly(1, date.Month, date.Day);
            return _NationalHolidaysDates.Contains(dateWithoutYear);
        }
    }
}
