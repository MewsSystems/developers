using DomainLayer.Aggregates.ExchangeRateAggregate;

namespace DomainLayer.Interfaces.Repositories;

/// <summary>
/// Repository for managing exchange rate aggregates.
/// </summary>
public interface IExchangeRateRepository
{
    /// <summary>
    /// Gets an exchange rate by its unique identifier.
    /// </summary>
    Task<ExchangeRate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets exchange rates for a specific provider and date.
    /// </summary>
    Task<IEnumerable<ExchangeRate>> GetByProviderAndDateAsync(
        int providerId,
        DateOnly validDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest exchange rate for a specific currency pair.
    /// </summary>
    Task<ExchangeRate?> GetLatestRateAsync(
        int baseCurrencyId,
        int targetCurrencyId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets exchange rate history for a currency pair within a date range.
    /// </summary>
    Task<IEnumerable<ExchangeRate>> GetHistoryAsync(
        int baseCurrencyId,
        int targetCurrencyId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all exchange rates for a specific provider within a date range.
    /// Optimized method to avoid N+1 queries when getting provider statistics.
    /// </summary>
    Task<IEnumerable<ExchangeRate>> GetByProviderAndDateRangeAsync(
        int providerId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new exchange rate aggregate.
    /// </summary>
    Task AddAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds multiple exchange rate aggregates.
    /// </summary>
    Task AddRangeAsync(IEnumerable<ExchangeRate> exchangeRates, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing exchange rate aggregate.
    /// </summary>
    Task UpdateAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an exchange rate aggregate.
    /// </summary>
    Task DeleteAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs bulk upsert of exchange rates using optimized database operations.
    /// This method uses stored procedures for high-performance bulk operations.
    /// </summary>
    /// <param name="providerId">The exchange rate provider ID</param>
    /// <param name="validDate">The date for which the rates are valid</param>
    /// <param name="rates">Collection of exchange rates to upsert</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing counts of inserted, updated, and skipped records</returns>
    Task<BulkExchangeRateResult> BulkUpsertAsync(
        int providerId,
        DateOnly validDate,
        IEnumerable<BulkExchangeRateItem> rates,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents a single exchange rate item for bulk operations.
/// </summary>
public record BulkExchangeRateItem(
    string CurrencyCode,
    decimal Rate,
    int Multiplier);

/// <summary>
/// Result of a bulk exchange rate operation.
/// </summary>
public record BulkExchangeRateResult(
    int InsertedCount,
    int UpdatedCount,
    int SkippedCount,
    int ProcessedCount,
    int TotalInJson,
    string Status);
