using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using NodaTime;
using PublicHoliday;

namespace ExchangeRateUpdater.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    private readonly IClock _clock;
    private readonly CzechRepublicPublicHoliday _czechHolidays;
    private readonly ExchangeRateServiceOptions _options;
    private readonly DateTimeZone _pragueZone;

    public DateTimeProvider(IClock clock,
        IOptions<ExchangeRateServiceOptions> options)
    {
        _clock = clock;
        _czechHolidays = new CzechRepublicPublicHoliday();
        _options = options.Value;
        _pragueZone = DateTimeZoneProviders.Tzdb["Europe/Prague"];
    }

    public LocalDate GetCurrentDate()
    {
        return _clock.GetCurrentInstant()
            .InZone(_pragueZone)
            .LocalDateTime.Date;
    }

    public LocalDateTime GetCurrentDateTime()
    {
        return _clock.GetCurrentInstant()
            .InZone(_pragueZone)
            .LocalDateTime;
    }

    public bool IsWorkingDay(LocalDate date)
    {
        // Convert NodaTime LocalDate to System.DateTime
        var dateTime = date.ToDateTimeUnspecified();
        return !dateTime.DayOfWeek.IsWeekend() && !_czechHolidays.IsPublicHoliday(dateTime);
    }

    public bool IsPublicationTimePassedForDate(LocalDate date)
    {
        var currentDateTime = GetCurrentDateTime();
        var currentDate = currentDateTime.Date;

        if (date < currentDate)
            // For past dates, publication time has always passed
            return true;

        if (date > currentDate)
            // For future dates, publication time hasn't passed yet
            return false;

        // For current date, check if current time is after publication time
        var publicationTime = new LocalTime(_options.PublicationHour, _options.PublicationMinute);
        return currentDateTime.TimeOfDay >= publicationTime;
    }

    public LocalDate GetNextBusinessDay(LocalDate date)
    {
        var nextDay = date.PlusDays(1);

        while (!IsWorkingDay(nextDay)) nextDay = nextDay.PlusDays(1);

        return nextDay;
    }

    public LocalDate GetLastWorkingDay(LocalDate date)
    {
        if (IsWorkingDay(date)) return date;

        var previousDay = date.Minus(Period.FromDays(1));

        while (!IsWorkingDay(previousDay)) previousDay = previousDay.Minus(Period.FromDays(1));

        return previousDay;
    }
}

public static class DayOfWeekExtensions
{
    public static bool IsWeekend(this DayOfWeek dayOfWeek)
    {
        return dayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }
}