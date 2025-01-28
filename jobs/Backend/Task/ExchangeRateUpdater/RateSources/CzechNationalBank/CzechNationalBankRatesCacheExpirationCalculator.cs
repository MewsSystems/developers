using System;
using PublicHoliday;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public class CzechNationalBankRatesCacheExpirationCalculator
{
    static readonly TimeZoneInfo CETTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
    static readonly TimeOnly MainRatesUpdateTime = new TimeOnly(14, 30);

    private readonly CzechRepublicPublicHoliday _holidays;
    private readonly TimeProvider _timeProvider;

    public CzechNationalBankRatesCacheExpirationCalculator(TimeProvider timeProvider)
    {
        _holidays = new CzechRepublicPublicHoliday() { UseCachingHolidays = true };
        _timeProvider = timeProvider;
    }

    public DateTime? GetSecondaryRateExpirationDate(DateOnly targetDate)
    {
        // Secondary dates are refreshed monthly on last working day of the month and are valid for the whole month. Unfortunately there's no information on the CNB website when exactly it's happening, so I have to assume that it's done at the end of the day, at 23:59.
        /*
          1. Target date is in future -> throw exception
          2. Target date is today -> set expiration date to the last working day of the month.
          3. Target date is in the past:
              3.1 Target date's month eq to the current month -> set expiration date to the last working day of the month
              3.2 Target date's month neq to the current month -> historical data, cache indefinetly
         */

        var now = _timeProvider.GetUtcNow();
        var czNow = TimeZoneInfo.ConvertTimeFromUtc(now.UtcDateTime, CETTimeZone);
        var nowDateOnly = DateOnly.FromDateTime(czNow.Date);

        if (targetDate > nowDateOnly)
        {
            throw new ArgumentException("Target date can not be in the future", nameof(targetDate));
        }

        if (targetDate == nowDateOnly || targetDate.Month == nowDateOnly.Month)
        {
            var lastWorkingDay = GetLastWorkingDayOfTheMonth(now.UtcDateTime);
            return lastWorkingDay.Date.AddHours(23).AddMinutes(59);
        }

        return null;
    }

    public DateTime? GetPrimaryRateExpirationDate(DateOnly targetDate)
    {
        // Primary rates are refreshed daily on work days at 14:30 CET.
        /* 1. Target date is in future -> throw exception
           2. Target date is today:
             2.1 If today is a non-working day -> set expiration date to 14:30 CET next working day.
             2.2 If today is a working day:
                 2.2.1 If it's before 14:30 -> set expiration date to 14:30 CET today
                 2.2.2 If it's after 14:30 -> set expiration to the next working day at 14:30 CET
           3. Target date is in the past:
             3.1 Target date is a working day -> historical data, cache indefinetely
             3.2 Target date is a non-working day:
                 3.2.1 Next working day after target date in the past -> historical data, cache indefinetely
                 3.2.2 Next working day after target date is in future -> set expiration date to the next working day 14:30 CET
                 3.2.3 Next working day after target date is today:
                     3.2.3.1 Now is before 14:30 CET -> set expiration date to today 14:30
                     3.2.3.2 Now is after 14:30 CET -> set expiration date to next working day 14:30
         */
        var now = _timeProvider.GetUtcNow();
        var nowDateOnly = DateOnly.FromDateTime(now.Date);

        if (targetDate > nowDateOnly)
        {
            throw new ArgumentException("Target date can not be in future", nameof(targetDate));
        }
        else if (targetDate < nowDateOnly)
        {
            return ProcessTargetDateInPast(now.UtcDateTime, targetDate);
        }
        else
        {
            return ProcessTargetDateToday(now.DateTime);
        }
    }

    private DateTime? ProcessTargetDateInPast(DateTime utcNow, DateOnly targetDate)
    {
        var czNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, CETTimeZone);
        var czTime = TimeOnly.FromDateTime(czNow);
        var targetDateTime = targetDate.ToDateTime(TimeOnly.MinValue);

        if (_holidays.IsWorkingDay(targetDateTime.Date))
        {
            return null;
        }

        var nextWorkingDay = _holidays.NextWorkingDay(targetDateTime);
        if (nextWorkingDay.Date < czNow.Date)
        {
            return null;
        }
        else if (nextWorkingDay.Date > czNow.Date)
        {
            var expirationDate = nextWorkingDay.Date.AddHours(MainRatesUpdateTime.Hour).AddMinutes(MainRatesUpdateTime.Minute);
            return TimeZoneInfo.ConvertTimeToUtc(expirationDate, CETTimeZone);
        }

        // nextWorkingDay is today
        if (czTime <= MainRatesUpdateTime)
        {
            var expirationDate = czNow.Date.AddHours(MainRatesUpdateTime.Hour).AddMinutes(MainRatesUpdateTime.Minute);

            return TimeZoneInfo.ConvertTimeToUtc(expirationDate, CETTimeZone);
        }
        else
        {
            var nextWorkingDayAfterToday = _holidays.NextWorkingDayNotSameDay(czNow).Date.AddHours(MainRatesUpdateTime.Hour).AddMinutes(MainRatesUpdateTime.Minute);

            return TimeZoneInfo.ConvertTimeToUtc(nextWorkingDayAfterToday);
        }
    }

    private DateTime ProcessTargetDateToday(DateTime utcNow)
    {
        var czNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, CETTimeZone);
        var nowTime = TimeOnly.FromDateTime(czNow);

        DateTime NextWorkingDayUpdateTime() => _holidays.NextWorkingDayNotSameDay(czNow).Date.AddHours(MainRatesUpdateTime.Hour).AddMinutes(MainRatesUpdateTime.Minute);

        if (!_holidays.IsWorkingDay(czNow))
        {

            return TimeZoneInfo.ConvertTimeToUtc(NextWorkingDayUpdateTime());
        }

        if (nowTime <= MainRatesUpdateTime)
        {
            var expirationDateCet = czNow.Date.AddHours(MainRatesUpdateTime.Hour).AddMinutes(MainRatesUpdateTime.Minute);
            return TimeZoneInfo.ConvertTimeToUtc(expirationDateCet);
        }
        else
        {
            return TimeZoneInfo.ConvertTimeToUtc(NextWorkingDayUpdateTime());
        }
    }

    private DateTime GetLastWorkingDayOfTheMonth(DateTime startDate)
    {
        var lastDayOfMonth = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));

        return _holidays.PreviousWorkingDay(lastDayOfMonth);
    }
}