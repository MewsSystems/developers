namespace DomainLayer.Interfaces.Repositories;

/// <summary>
/// Repository for accessing exchange rate fetch logs.
/// Note: This is primarily for read-only queries and monitoring.
/// </summary>
public interface IExchangeRateFetchLogRepository
{
    /// <summary>
    /// Gets the most recent fetch logs.
    /// </summary>
    Task<IEnumerable<FetchLogEntry>> GetRecentLogsAsync(
        int count = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets fetch logs for a specific provider.
    /// </summary>
    Task<IEnumerable<FetchLogEntry>> GetLogsByProviderAsync(
        int providerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all failed fetch logs.
    /// </summary>
    Task<IEnumerable<FetchLogEntry>> GetFailedLogsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets fetch logs for a date range.
    /// </summary>
    Task<IEnumerable<FetchLogEntry>> GetLogsByDateRangeAsync(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents a fetch log entry (read-only).
/// </summary>
public record FetchLogEntry(
    long Id,
    int ProviderId,
    string ProviderCode,
    string ProviderName,
    DateTimeOffset FetchStarted,
    DateTimeOffset? FetchCompleted,
    string Status,
    int? RatesImported,
    int? RatesUpdated,
    string? ErrorMessage,
    int? DurationMs);
