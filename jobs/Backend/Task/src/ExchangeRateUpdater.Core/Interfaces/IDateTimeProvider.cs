using NodaTime;

namespace ExchangeRateUpdater.Core.Interfaces;

/// <summary>
/// Interface for date/time operations.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current date in UTC.
    /// </summary>
    /// <returns>The current date.</returns>
    LocalDate GetCurrentDate();

    /// <summary>
    /// Gets the current local date and time in UTC.
    /// </summary>
    /// <returns>The current local date and time.</returns>
    LocalDateTime GetCurrentDateTime();

    /// <summary>
    /// Determines whether the specified date is a working day.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is a working day; otherwise, <c>false</c>.</returns>
    bool IsWorkingDay(LocalDate date);

    /// <summary>
    /// Determines whether the publication time has passed for the specified date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the publication time has passed; otherwise, <c>false</c>.</returns>
    bool IsPublicationTimePassedForDate(LocalDate date);

    /// <summary>
    /// Gets the next business day after the specified date.
    /// </summary>
    /// <param name="date">The reference date.</param>
    /// <returns>The next business day.</returns>
    LocalDate GetNextBusinessDay(LocalDate date);

    /// <summary>
    /// Gets the last working day before or on the specified date.
    /// </summary>
    /// <param name="date">The reference date.</param>
    /// <returns>The last working day.</returns>
    LocalDate GetLastWorkingDay(LocalDate date);
}
