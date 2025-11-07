namespace DomainLayer.Interfaces.Repositories;

/// <summary>
/// Repository for accessing and managing error logs.
/// </summary>
public interface IErrorLogRepository
{
    /// <summary>
    /// Gets the most recent errors.
    /// </summary>
    Task<IEnumerable<ErrorLogEntry>> GetRecentErrorsAsync(
        int count = 100,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets errors by severity level.
    /// </summary>
    Task<IEnumerable<ErrorLogEntry>> GetErrorsBySeverityAsync(
        string severity,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets errors within a date range.
    /// </summary>
    Task<IEnumerable<ErrorLogEntry>> GetErrorsByDateRangeAsync(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs an error to the database.
    /// </summary>
    /// <param name="severity">The severity level (e.g., "Error", "Warning", "Critical")</param>
    /// <param name="source">The source of the error (e.g., component or class name)</param>
    /// <param name="message">The error message</param>
    /// <param name="exception">Optional exception details</param>
    /// <param name="stackTrace">Optional stack trace</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created error log entry</returns>
    Task<long> LogErrorAsync(
        string severity,
        string source,
        string message,
        string? exception = null,
        string? stackTrace = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents an error log entry (read-only).
/// </summary>
public record ErrorLogEntry(
    long Id,
    DateTimeOffset Timestamp,
    string Severity,
    string Source,
    string Message,
    string? Exception,
    string? StackTrace);
