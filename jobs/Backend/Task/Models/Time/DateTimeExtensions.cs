using System;

namespace ExchangeRateUpdater.Models.Time;

/// <summary>
/// Contains extension methods for handling <see cref="DateTime"/> objects.
/// </summary>
internal static class DateTimeExtensions
{
    /// <summary>
    /// Gets the string representation of the year and month of the specified <see cref="DateTime"/> object in the format "yyyy-MM".
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> object.</param>
    /// <returns>A string representing the year and month in the format "yyyy-MM".</returns>
    public static string GetYearMonthString(this DateTime dateTime)
        => dateTime.AddMonths(-1).ToString("yyyy-MM");
}