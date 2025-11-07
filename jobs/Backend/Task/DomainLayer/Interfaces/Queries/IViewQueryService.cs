using DomainLayer.Models.Views;

namespace DomainLayer.Interfaces.Queries;

/// <summary>
/// Domain interface for querying system views and aggregated data.
/// Provides read-only access to pre-aggregated data for performance.
/// Implemented by InfrastructureLayer adapter.
/// </summary>
public interface ISystemViewQueries
{
    /// <summary>
    /// Gets current exchange rates.
    /// Returns rates for today from all active providers.
    /// </summary>
    Task<IEnumerable<CurrentExchangeRateView>> GetCurrentExchangeRatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets latest exchange rates.
    /// Returns the most recent rate for each currency pair.
    /// </summary>
    Task<IEnumerable<LatestExchangeRateView>> GetLatestExchangeRatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets exchange rate history.
    /// </summary>
    /// <param name="days">Optional filter for rates within last N days</param>
    Task<IEnumerable<ExchangeRateHistoryView>> GetExchangeRateHistoryAsync(int? days = 30, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets provider health status for all providers.
    /// Includes 30-day statistics and average fetch duration.
    /// </summary>
    Task<IEnumerable<ProviderHealthStatusView>> GetProviderHealthStatusAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets provider health status by provider ID.
    /// </summary>
    Task<ProviderHealthStatusView?> GetProviderHealthStatusByIdAsync(int providerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets recent fetch activity logs.
    /// </summary>
    /// <param name="limit">Maximum number of records to return</param>
    Task<IEnumerable<RecentFetchActivityView>> GetRecentFetchActivityAsync(int? limit = 50, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets system health dashboard metrics.
    /// Returns key-value metrics for system-wide statistics.
    /// </summary>
    Task<IEnumerable<SystemHealthDashboardView>> GetSystemHealthDashboardAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets error summary.
    /// </summary>
    /// <param name="hours">Filter for errors within last N hours</param>
    Task<IEnumerable<ErrorSummaryView>> GetErrorSummaryAsync(int? hours = 24, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets currency pair availability.
    /// Shows which currency pairs are available from which providers.
    /// </summary>
    Task<IEnumerable<CurrencyPairAvailabilityView>> GetCurrencyPairAvailabilityAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all latest exchange rates across all providers.
    /// Returns the most recent rate (by ValidDate) for each currency pair, regardless of provider.
    /// </summary>
    Task<IEnumerable<AllLatestExchangeRatesView>> GetAllLatestExchangeRatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all latest updated exchange rates across all providers.
    /// Returns the most recently updated rate (by Created timestamp) for each currency pair.
    /// Useful when multiple providers publish rates for the same ValidDate at different times.
    /// </summary>
    Task<IEnumerable<AllLatestUpdatedExchangeRatesView>> GetAllLatestUpdatedExchangeRatesAsync(CancellationToken cancellationToken = default);
}
