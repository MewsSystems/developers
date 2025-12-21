namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;

/// <summary>
/// Implementation of date and time calculation services for CNB (Czech National Bank) exchange rate operations.
/// </summary>
/// <param name="timeProvider">The time provider to use for current time calculations.</param>
internal class CnbCacheDateTimeProvider(TimeProvider timeProvider) : ICacheDateTimeProvider
{
    /// <summary>
    /// Default buffer time added after the CNB fixing time before cache expiration.
    /// </summary>
    private static readonly TimeSpan DefaultBuffer = TimeSpan.FromMinutes(5);

    /// <summary>
    /// The time when CNB publishes exchange rates (14:30 Prague time on business days).
    /// </summary>
    private static readonly TimeOnly FixingTime = new(14, 30, 0);

    /// <summary>
    /// Calculates the UTC expiration time for cached exchange rates based on CNB's fixing schedule.
    /// CNB publishes exchange rates at 14:30 Prague time on business days.
    /// </summary>
    /// <param name="buffer">Additional buffer time after the CNB fixing time. Defaults to 5 minutes if not specified.</param>
    /// <returns>The UTC DateTime when the cached exchange rates should expire.</returns>
    public DateTimeOffset GetNextFixingExpirationUtc(TimeSpan? buffer = null)
    {
        buffer ??= DefaultBuffer;

        var pragueTz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");

        var nowUtc = timeProvider.GetUtcNow();
        var nowPrague = TimeZoneInfo.ConvertTime(nowUtc, pragueTz);

        var todayPrague = DateOnly.FromDateTime(nowPrague.DateTime);

        var fixingTodayPrague = new DateTimeOffset(
            todayPrague.Year, todayPrague.Month, todayPrague.Day,
            FixingTime.Hour, FixingTime.Minute, FixingTime.Second,
            nowPrague.Offset);

        DateTimeOffset nextFixingPrague;

        if (ShouldUseNextBusinessDayFixing(todayPrague, nowPrague, fixingTodayPrague))
        {
            var nextBusinessDay = GetNextBusinessDay(todayPrague);

            nextFixingPrague = new DateTimeOffset(
                nextBusinessDay.Year, nextBusinessDay.Month, nextBusinessDay.Day,
                FixingTime.Hour, FixingTime.Minute, FixingTime.Second,
                nowPrague.Offset);
        }
        else
        {
            nextFixingPrague = fixingTodayPrague;
        }

        var expirePrague = nextFixingPrague.Add(buffer.Value);
        return TimeZoneInfo.ConvertTime(expirePrague, TimeZoneInfo.Utc);
    }

    /// <summary>
    /// Determines if the calculation should use the next business day fixing time.
    /// This occurs when it's currently a weekend or when today's fixing time has already passed.
    /// </summary>
    /// <param name="date">The current date in Prague timezone.</param>
    /// <param name="currentTime">The current time in Prague timezone.</param>
    /// <param name="todayFixingTime">Today's fixing time in Prague timezone.</param>
    /// <returns>True if the next business day fixing should be used; otherwise, false.</returns>
    private static bool ShouldUseNextBusinessDayFixing(DateOnly date, DateTimeOffset currentTime, DateTimeOffset todayFixingTime)
    {
        return IsWeekend(date) || currentTime >= todayFixingTime;
    }

    /// <summary>
    /// Determines if the specified date falls on a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>True if the date is a Saturday or Sunday; otherwise, false.</returns>
    private static bool IsWeekend(DateOnly date) =>
        date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    /// Finds the next business day (Monday through Friday) after the specified date.
    /// </summary>
    /// <param name="date">The starting date to find the next business day from.</param>
    /// <returns>The next business day after the specified date.</returns>
    private static DateOnly GetNextBusinessDay(DateOnly date)
    {
        var nextBusinessDay = date.AddDays(1);

        while (IsWeekend(nextBusinessDay))
        {
            nextBusinessDay = nextBusinessDay.AddDays(1);
        }

        return nextBusinessDay;
    }
}