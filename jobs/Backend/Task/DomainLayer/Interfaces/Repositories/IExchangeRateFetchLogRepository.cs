namespace DomainLayer.Interfaces.Repositories;

/// <summary>
/// Repository for accessing and managing exchange rate fetch logs.
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

    /// <summary>
    /// Starts a new fetch log entry for tracking provider fetch operations.
    /// </summary>
    /// <param name="providerId">The ID of the exchange rate provider</param>
    /// <param name="requestedBy">Optional ID of the entity that requested the fetch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created fetch log entry</returns>
    Task<long> StartFetchLogAsync(
        int providerId,
        int? requestedBy = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Completes a fetch log entry with the results of the fetch operation.
    /// </summary>
    /// <param name="logId">The ID of the fetch log entry to complete</param>
    /// <param name="status">The status of the fetch operation (e.g., "Success", "Failed")</param>
    /// <param name="ratesImported">Number of new rates imported</param>
    /// <param name="ratesUpdated">Number of existing rates updated</param>
    /// <param name="errorMessage">Error message if the fetch failed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task CompleteFetchLogAsync(
        long logId,
        string status,
        int? ratesImported = null,
        int? ratesUpdated = null,
        string? errorMessage = null,
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
