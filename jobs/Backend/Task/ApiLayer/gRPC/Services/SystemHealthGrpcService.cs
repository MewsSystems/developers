using ApplicationLayer.Queries.SystemHealth.GetFetchActivity;
using ApplicationLayer.Queries.SystemHealth.GetRecentErrors;
using ApplicationLayer.Queries.SystemHealth.GetSystemHealth;
using gRPC.Protos.SystemHealth;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace gRPC.Services;

/// <summary>
/// gRPC service for system health monitoring operations.
/// </summary>
[Authorize(Roles = "Admin")]
public class SystemHealthGrpcService : SystemHealthService.SystemHealthServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SystemHealthGrpcService> _logger;

    public SystemHealthGrpcService(
        IMediator mediator,
        ILogger<SystemHealthGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<GetSystemHealthResponse> GetSystemHealth(
        GetSystemHealthRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetSystemHealth request received");

        var query = new GetSystemHealthQuery();
        var health = await _mediator.Send(query, context.CancellationToken);

        var response = new GetSystemHealthResponse
        {
            Health = new SystemHealthInfo
            {
                TotalProviders = health.TotalProviders,
                ActiveProviders = health.ActiveProviders,
                QuarantinedProviders = health.QuarantinedProviders,
                TotalCurrencies = health.TotalCurrencies,
                TotalExchangeRates = health.TotalExchangeRates,
                TotalFetchesToday = health.TotalFetchesToday,
                SuccessfulFetchesToday = health.SuccessfulFetchesToday,
                FailedFetchesToday = health.FailedFetchesToday,
                SuccessRateToday = (double)health.SuccessRateToday,
                LastUpdated = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(health.LastUpdated)
            },
            Message = "System health retrieved successfully"
        };

        if (health.LatestRateDate.HasValue)
        {
            response.Health.LatestRateDate = new gRPC.Protos.Common.Date
            {
                Year = health.LatestRateDate.Value.Year,
                Month = health.LatestRateDate.Value.Month,
                Day = health.LatestRateDate.Value.Day
            };
        }

        if (health.OldestRateDate.HasValue)
        {
            response.Health.OldestRateDate = new gRPC.Protos.Common.Date
            {
                Year = health.OldestRateDate.Value.Year,
                Month = health.OldestRateDate.Value.Month,
                Day = health.OldestRateDate.Value.Day
            };
        }

        return response;
    }

    public override async Task<GetRecentErrorsResponse> GetRecentErrors(
        GetRecentErrorsRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetRecentErrors request: Limit={Limit}, ProviderId={ProviderId}",
            request.Limit, request.HasProviderId ? request.ProviderId : null);

        // Note: GetRecentErrorsQuery takes Severity (string?), not ProviderId
        // ProviderId filtering is not supported by this query
        var query = new GetRecentErrorsQuery(request.Limit);

        var errors = await _mediator.Send(query, context.CancellationToken);

        var response = new GetRecentErrorsResponse
        {
            Message = "Recent errors retrieved successfully"
        };

        foreach (var error in errors.Items)
        {
            response.Errors.Add(new ErrorLogInfo
            {
                Id = 0, // ErrorSummaryDto doesn't have an ID
                ProviderId = 0, // ErrorSummaryDto doesn't track individual provider IDs
                ProviderCode = error.AffectedProviders.Count > 0 ? string.Join(", ", error.AffectedProviders) : "",
                ErrorMessage = error.ErrorMessage,
                ErrorType = error.ErrorType,
                StackTrace = "",
                OccurredAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(error.LastOccurrence)
            });
        }

        return response;
    }

    public override async Task<GetFetchActivityResponse> GetFetchActivity(
        GetFetchActivityRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetFetchActivity request: Limit={Limit}, ProviderId={ProviderId}",
            request.Limit, request.HasProviderId ? request.ProviderId : null);

        int? providerId = request.HasProviderId ? request.ProviderId : null;
        var query = new GetFetchActivityQuery(request.Limit, providerId);

        var fetchLogs = await _mediator.Send(query, context.CancellationToken);

        var response = new GetFetchActivityResponse
        {
            Message = "Fetch activity retrieved successfully"
        };

        foreach (var log in fetchLogs.Items)
        {
            response.FetchLogs.Add(new FetchLogInfo
            {
                Id = log.LogId,
                ProviderId = log.ProviderId,
                ProviderCode = log.ProviderCode,
                StartedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(log.StartedAt),
                CompletedAt = log.CompletedAt.HasValue
                    ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(log.CompletedAt.Value)
                    : null,
                Status = log.Status,
                ErrorMessage = log.ErrorMessage ?? "",
                RecordsFetched = (log.RatesImported ?? 0) + (log.RatesUpdated ?? 0),
                RequestedBy = ""
            });
        }

        return response;
    }
}
