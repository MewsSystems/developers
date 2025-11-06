namespace DomainLayer.Interfaces.Services;

/// <summary>
/// Abstraction for getting current date/time.
/// Allows for testability and consistent time handling across the application.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTimeOffset UtcNow { get; }

    /// <summary>
    /// Gets the current UTC date (without time component).
    /// </summary>
    DateOnly Today { get; }
}
