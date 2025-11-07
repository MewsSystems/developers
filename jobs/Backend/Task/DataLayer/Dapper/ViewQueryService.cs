using Dapper;
using DataLayer.DTOs;

namespace DataLayer.Dapper;

/// <summary>
/// Implementation of IViewQueryService using Dapper for efficient view queries.
/// Maps database views to DataLayer DTOs.
/// </summary>
public class ViewQueryService : IViewQueryService
{
    private readonly IDapperContext _dapperContext;

    public ViewQueryService(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<CurrentExchangeRateView>> GetCurrentExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<CurrentExchangeRateView>(
            "SELECT * FROM [dbo].[vw_CurrentExchangeRates] ORDER BY ProviderCode, TargetCurrencyCode");
    }

    public async Task<IEnumerable<LatestExchangeRateView>> GetLatestExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<LatestExchangeRateView>(
            "SELECT * FROM [dbo].[vw_LatestExchangeRates] ORDER BY ProviderCode, TargetCurrencyCode");
    }

    public async Task<IEnumerable<ExchangeRateHistoryView>> GetExchangeRateHistoryAsync(int? days = 30, CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);

        var sql = days.HasValue
            ? "SELECT * FROM [dbo].[vw_ExchangeRateHistory] WHERE DaysOld <= @Days ORDER BY ValidDate DESC, ProviderCode, TargetCurrencyCode"
            : "SELECT * FROM [dbo].[vw_ExchangeRateHistory] ORDER BY ValidDate DESC, ProviderCode, TargetCurrencyCode";

        return await connection.QueryAsync<ExchangeRateHistoryView>(sql, new { Days = days });
    }

    public async Task<IEnumerable<ProviderHealthStatusView>> GetProviderHealthStatusAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<ProviderHealthStatusView>(
            "SELECT * FROM [dbo].[vw_ProviderHealthStatus] ORDER BY Code");
    }

    public async Task<ProviderHealthStatusView?> GetProviderHealthStatusByIdAsync(int providerId, CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<ProviderHealthStatusView>(
            "SELECT * FROM [dbo].[vw_ProviderHealthStatus] WHERE Id = @ProviderId",
            new { ProviderId = providerId });
    }

    public async Task<IEnumerable<RecentFetchActivityView>> GetRecentFetchActivityAsync(int? limit = 50, CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);

        var sql = limit.HasValue
            ? "SELECT TOP (@Limit) * FROM [dbo].[vw_RecentFetchActivity] ORDER BY FetchStarted DESC"
            : "SELECT * FROM [dbo].[vw_RecentFetchActivity] ORDER BY FetchStarted DESC";

        return await connection.QueryAsync<RecentFetchActivityView>(sql, new { Limit = limit });
    }

    public async Task<IEnumerable<SystemHealthDashboardView>> GetSystemHealthDashboardAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<SystemHealthDashboardView>(
            "SELECT * FROM [dbo].[vw_SystemHealthDashboard] ORDER BY Metric");
    }

    public async Task<IEnumerable<ErrorSummaryView>> GetErrorSummaryAsync(int? hours = 24, CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);

        var sql = hours.HasValue
            ? "SELECT * FROM [dbo].[vw_ErrorSummary] WHERE MinutesAgo <= @Minutes ORDER BY Timestamp DESC"
            : "SELECT * FROM [dbo].[vw_ErrorSummary] ORDER BY Timestamp DESC";

        return await connection.QueryAsync<ErrorSummaryView>(sql, new { Minutes = hours * 60 });
    }

    public async Task<IEnumerable<CurrencyPairAvailabilityView>> GetCurrencyPairAvailabilityAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<CurrencyPairAvailabilityView>(
            "SELECT * FROM [dbo].[vw_CurrencyPairAvailability] ORDER BY BaseCurrencyCode, TargetCurrencyCode");
    }

    public async Task<IEnumerable<AllLatestExchangeRatesView>> GetAllLatestExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<AllLatestExchangeRatesView>(
            "SELECT * FROM [dbo].[vw_AllLatestExchangeRates] ORDER BY BaseCurrencyCode, TargetCurrencyCode");
    }

    public async Task<IEnumerable<AllLatestUpdatedExchangeRatesView>> GetAllLatestUpdatedExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dapperContext.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<AllLatestUpdatedExchangeRatesView>(
            "SELECT * FROM [dbo].[vw_AllLatestUpdatedExchangeRates] ORDER BY BaseCurrencyCode, TargetCurrencyCode");
    }
}
