using DataLayer.DTOs;

namespace DataLayer.Dapper;

/// <summary>
/// Service for querying optimized database views using Dapper.
/// This is a DataLayer concern - returns DataLayer DTOs.
/// </summary>
public interface IViewQueryService
{
    Task<IEnumerable<CurrentExchangeRateView>> GetCurrentExchangeRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<LatestExchangeRateView>> GetLatestExchangeRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRateHistoryView>> GetExchangeRateHistoryAsync(int? days = 30, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProviderHealthStatusView>> GetProviderHealthStatusAsync(CancellationToken cancellationToken = default);
    Task<ProviderHealthStatusView?> GetProviderHealthStatusByIdAsync(int providerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RecentFetchActivityView>> GetRecentFetchActivityAsync(int? limit = 50, CancellationToken cancellationToken = default);
    Task<IEnumerable<SystemHealthDashboardView>> GetSystemHealthDashboardAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ErrorSummaryView>> GetErrorSummaryAsync(int? hours = 24, CancellationToken cancellationToken = default);
    Task<IEnumerable<CurrencyPairAvailabilityView>> GetCurrencyPairAvailabilityAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AllLatestExchangeRatesView>> GetAllLatestExchangeRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AllLatestUpdatedExchangeRatesView>> GetAllLatestUpdatedExchangeRatesAsync(CancellationToken cancellationToken = default);
}
