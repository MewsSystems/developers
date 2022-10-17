namespace ExchangeRateUpdater.Domain.Extensions;

/// <summary>
/// Extension for datetime.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Finds previous work day.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns></returns>
    public static DateTime PreviousWorkDay(this DateTime date)
    {
        var prevDay = date.AddDays(-1);
        while (prevDay.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            prevDay = prevDay.AddDays(-1);
        }

        return prevDay;
    }
}