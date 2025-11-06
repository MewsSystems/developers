namespace DomainLayer.Interfaces.Repositories;

/// <summary>
/// Repository for accessing error logs.
/// Note: This is primarily for read-only queries and monitoring.
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
