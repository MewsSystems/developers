namespace ExchangeRateUpdater.Helpers;

public static class DateTimeExtensions
{
    public static DateTime WithTimezone(this DateTime datetime, string timezoneId)
    {
        TimeZoneInfo timezone    = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        DateTime     currentTime = TimeZoneInfo.ConvertTimeFromUtc(datetime, timezone);

        return currentTime;
    }
}