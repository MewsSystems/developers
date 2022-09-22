using System;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

internal class DateProvider : IDateProvider
{
    private readonly IDateTimeService _dateTime;

    public DateProvider(IDateTimeService dateTime)
    {
        _dateTime = dateTime;
    }

    public DateOnly ForToday()
    {
        var now = _dateTime.UtcNow;
        var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
        var date = TimeZoneInfo.ConvertTime(now, timezone);

        if (date.Hour < 14 && date.Minute < 30)
        {
            return DateOnly.FromDateTime(date.AddDays(-1));
        }
        
        return DateOnly.FromDateTime(date);
    }
}