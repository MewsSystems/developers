using DataLayer.Dapper;
using DomainLayer.Interfaces.Queries;
using DomainLayer.Models.Views;

namespace InfrastructureLayer.Persistence.Adapters;

/// <summary>
/// Adapts DataLayer view query service to DomainLayer interface.
/// Maps DataLayer DTOs to Domain view models.
/// </summary>
public class ViewQueryRepositoryAdapter : ISystemViewQueries
{
    private readonly IViewQueryService _viewQueryService;

    public ViewQueryRepositoryAdapter(IViewQueryService viewQueryService)
    {
        _viewQueryService = viewQueryService;
    }

    public async Task<IEnumerable<CurrentExchangeRateView>> GetCurrentExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetCurrentExchangeRatesAsync(cancellationToken);
        return dataLayerViews.Select(MapCurrentExchangeRate);
    }

    public async Task<IEnumerable<LatestExchangeRateView>> GetLatestExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetLatestExchangeRatesAsync(cancellationToken);
        return dataLayerViews.Select(MapLatestExchangeRate);
    }

    public async Task<IEnumerable<ExchangeRateHistoryView>> GetExchangeRateHistoryAsync(int? days = 30, CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetExchangeRateHistoryAsync(days, cancellationToken);
        return dataLayerViews.Select(MapExchangeRateHistory);
    }

    public async Task<IEnumerable<ProviderHealthStatusView>> GetProviderHealthStatusAsync(CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetProviderHealthStatusAsync(cancellationToken);
        return dataLayerViews.Select(MapProviderHealthStatus);
    }

    public async Task<ProviderHealthStatusView?> GetProviderHealthStatusByIdAsync(int providerId, CancellationToken cancellationToken = default)
    {
        var dataLayerView = await _viewQueryService.GetProviderHealthStatusByIdAsync(providerId, cancellationToken);
        return dataLayerView != null ? MapProviderHealthStatus(dataLayerView) : null;
    }

    public async Task<IEnumerable<RecentFetchActivityView>> GetRecentFetchActivityAsync(int? limit = 50, CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetRecentFetchActivityAsync(limit, cancellationToken);
        return dataLayerViews.Select(MapRecentFetchActivity);
    }

    public async Task<IEnumerable<SystemHealthDashboardView>> GetSystemHealthDashboardAsync(CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetSystemHealthDashboardAsync(cancellationToken);
        return dataLayerViews.Select(MapSystemHealthDashboard);
    }

    public async Task<IEnumerable<ErrorSummaryView>> GetErrorSummaryAsync(int? hours = 24, CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetErrorSummaryAsync(hours, cancellationToken);
        return dataLayerViews.Select(MapErrorSummary);
    }

    public async Task<IEnumerable<CurrencyPairAvailabilityView>> GetCurrencyPairAvailabilityAsync(CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetCurrencyPairAvailabilityAsync(cancellationToken);
        return dataLayerViews.Select(MapCurrencyPairAvailability);
    }

    public async Task<IEnumerable<AllLatestExchangeRatesView>> GetAllLatestExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetAllLatestExchangeRatesAsync(cancellationToken);
        return dataLayerViews.Select(MapAllLatestExchangeRates);
    }

    public async Task<IEnumerable<AllLatestUpdatedExchangeRatesView>> GetAllLatestUpdatedExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var dataLayerViews = await _viewQueryService.GetAllLatestUpdatedExchangeRatesAsync(cancellationToken);
        return dataLayerViews.Select(MapAllLatestUpdatedExchangeRates);
    }

    // Mapping methods from DataLayer DTOs to Domain view models
    private static CurrentExchangeRateView MapCurrentExchangeRate(DataLayer.DTOs.CurrentExchangeRateView dto)
    {
        return new CurrentExchangeRateView
        {
            Id = dto.Id,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            TargetCurrencyId = dto.TargetCurrencyId,
            Rate = dto.Rate,
            Multiplier = dto.Multiplier,
            RatePerUnit = dto.RatePerUnit,
            ValidDate = dto.ValidDate,
            Created = dto.Created
        };
    }

    private static LatestExchangeRateView MapLatestExchangeRate(DataLayer.DTOs.LatestExchangeRateView dto)
    {
        return new LatestExchangeRateView
        {
            Id = dto.Id,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            Rate = dto.Rate,
            Multiplier = dto.Multiplier,
            RatePerUnit = dto.RatePerUnit,
            ValidDate = dto.ValidDate,
            Created = dto.Created,
            RowNum = dto.RowNum
        };
    }

    private static ExchangeRateHistoryView MapExchangeRateHistory(DataLayer.DTOs.ExchangeRateHistoryView dto)
    {
        return new ExchangeRateHistoryView
        {
            Id = dto.Id,
            ProviderCode = dto.ProviderCode,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            Rate = dto.Rate,
            Multiplier = dto.Multiplier,
            RatePerUnit = dto.RatePerUnit,
            ValidDate = dto.ValidDate,
            Created = dto.Created,
            Modified = dto.Modified,
            DaysOld = dto.DaysOld
        };
    }

    private static ProviderHealthStatusView MapProviderHealthStatus(DataLayer.DTOs.ProviderHealthStatusView dto)
    {
        return new ProviderHealthStatusView
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            IsActive = dto.IsActive,
            BaseCurrencyId = dto.BaseCurrencyId,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            RequiresAuthentication = dto.RequiresAuthentication,
            LastSuccessfulFetch = dto.LastSuccessfulFetch,
            LastFailedFetch = dto.LastFailedFetch,
            ConsecutiveFailures = dto.ConsecutiveFailures,
            HoursSinceLastSuccess = dto.HoursSinceLastSuccess,
            TotalFetches30Days = dto.TotalFetches30Days,
            SuccessfulFetches30Days = dto.SuccessfulFetches30Days,
            FailedFetches30Days = dto.FailedFetches30Days,
            AvgFetchDurationMs = dto.AvgFetchDurationMs,
            HealthStatus = dto.HealthStatus
        };
    }

    private static RecentFetchActivityView MapRecentFetchActivity(DataLayer.DTOs.RecentFetchActivityView dto)
    {
        return new RecentFetchActivityView
        {
            Id = dto.Id,
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            FetchStarted = dto.FetchStarted,
            FetchCompleted = dto.FetchCompleted,
            Status = dto.Status,
            RatesImported = dto.RatesImported,
            RatesUpdated = dto.RatesUpdated,
            DurationMs = dto.DurationMs,
            ErrorMessage = dto.ErrorMessage,
            RequestedByEmail = dto.RequestedByEmail
        };
    }

    private static SystemHealthDashboardView MapSystemHealthDashboard(DataLayer.DTOs.SystemHealthDashboardView dto)
    {
        return new SystemHealthDashboardView
        {
            Metric = dto.Metric,
            Value = dto.Value,
            Status = dto.Status,
            Details = dto.Details
        };
    }

    private static ErrorSummaryView MapErrorSummary(DataLayer.DTOs.ErrorSummaryView dto)
    {
        return new ErrorSummaryView
        {
            Id = dto.Id,
            Timestamp = dto.Timestamp,
            Severity = dto.Severity,
            Source = dto.Source,
            Message = dto.Message,
            UserEmail = dto.UserEmail,
            MinutesAgo = dto.MinutesAgo
        };
    }

    private static CurrencyPairAvailabilityView MapCurrencyPairAvailability(DataLayer.DTOs.CurrencyPairAvailabilityView dto)
    {
        return new CurrencyPairAvailabilityView
        {
            BaseCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            ProviderCount = dto.ProviderCount,
            AvailableProviders = dto.AvailableProviders,
            LatestDate = dto.LatestDate,
            DaysSinceUpdate = dto.DaysSinceUpdate
        };
    }

    private static AllLatestExchangeRatesView MapAllLatestExchangeRates(DataLayer.DTOs.AllLatestExchangeRatesView dto)
    {
        return new AllLatestExchangeRatesView
        {
            Id = dto.Id,
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            BaseCurrencyId = dto.BaseCurrencyId,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            TargetCurrencyId = dto.TargetCurrencyId,
            Rate = dto.Rate,
            Multiplier = dto.Multiplier,
            RatePerUnit = dto.RatePerUnit,
            ValidDate = dto.ValidDate,
            Created = dto.Created,
            Modified = dto.Modified,
            DaysOld = dto.DaysOld,
            FreshnessStatus = dto.FreshnessStatus
        };
    }

    private static AllLatestUpdatedExchangeRatesView MapAllLatestUpdatedExchangeRates(DataLayer.DTOs.AllLatestUpdatedExchangeRatesView dto)
    {
        return new AllLatestUpdatedExchangeRatesView
        {
            Id = dto.Id,
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            BaseCurrencyId = dto.BaseCurrencyId,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            TargetCurrencyId = dto.TargetCurrencyId,
            Rate = dto.Rate,
            Multiplier = dto.Multiplier,
            RatePerUnit = dto.RatePerUnit,
            ValidDate = dto.ValidDate,
            Created = dto.Created,
            Modified = dto.Modified,
            DaysOld = dto.DaysOld,
            MinutesSinceUpdate = dto.MinutesSinceUpdate,
            UpdateFreshness = dto.UpdateFreshness
        };
    }
}
