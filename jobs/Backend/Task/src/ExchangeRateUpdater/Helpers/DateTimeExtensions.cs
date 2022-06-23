namespace ExchangeRateUpdater.Helpers;

public static class DateTimeExtensions
{
    public static DateTime ConvertTimeFromUtcWithTimezoneId(this DateTime datetime, string timezoneId)
    {
        TimeZoneInfo timezone    = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        DateTime     currentTime = TimeZoneInfo.ConvertTimeFromUtc(datetime, timezone);

        return currentTime;
    }

    public static DateTimeOffset GetDateTimeOffsetWithTimezoneId(this DateTime datetime, string timezoneId)
    {
        TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        return new DateTimeOffset(datetime.ToUniversalTime()).ToOffset(timezone.BaseUtcOffset);
    }
}