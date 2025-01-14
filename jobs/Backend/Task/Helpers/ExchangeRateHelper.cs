using System;

namespace ExchangeRateUpdater.Helpers;

public static class ExchangeRateHelper
{
    public static TimeProvider TimeProvider = TimeProvider.System;

    public static string GetTimeUntilNextExchangeRateData()
    {
        var now = TimeProvider.GetUtcNow().UtcDateTime;
        
        var nextUpdateTime = now.DayOfWeek switch
        {
            DayOfWeek.Saturday => now.Date.AddDays(2).AddHours(14).AddMinutes(31),
            DayOfWeek.Sunday => now.Date.AddDays(1).AddHours(14).AddMinutes(31),
            _ => new DateTime(now.Year, now.Month, now.Day, 14, 31, 0, DateTimeKind.Utc)
        };

        if (now >= nextUpdateTime)
        {
            nextUpdateTime = nextUpdateTime.AddDays(1);
            
            while (nextUpdateTime.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                nextUpdateTime = nextUpdateTime.AddDays(1);
            }
        }

        var timeUntilNextUpdate = nextUpdateTime - now;

        return $"Time until next exchange rate update: {(int)timeUntilNextUpdate.TotalHours} hours and {timeUntilNextUpdate.Minutes} minutes.";
    }
}