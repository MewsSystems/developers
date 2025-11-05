using DataLayer.DTOs;

namespace DataLayer.Dapper;

public interface IViewQueryService
{
    // Current and Latest Rates
    Task<IEnumerable<CurrentExchangeRateView>> GetCurrentExchangeRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<LatestExchangeRateView>> GetLatestExchangeRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRateHistoryView>> GetExchangeRateHistoryAsync(int? days = 30, CancellationToken cancellationToken = default);

    // Provider Health and Activity
    Task<IEnumerable<ProviderHealthStatusView>> GetProviderHealthStatusAsync(CancellationToken cancellationToken = default);
    Task<ProviderHealthStatusView?> GetProviderHealthStatusByIdAsync(int providerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RecentFetchActivityView>> GetRecentFetchActivityAsync(int? limit = 50, CancellationToken cancellationToken = default);

    // System Health
    Task<IEnumerable<SystemHealthDashboardView>> GetSystemHealthDashboardAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ErrorSummaryView>> GetErrorSummaryAsync(int? hours = 24, CancellationToken cancellationToken = default);

    // Currency Information
    Task<IEnumerable<CurrencyPairAvailabilityView>> GetCurrencyPairAvailabilityAsync(CancellationToken cancellationToken = default);
}
