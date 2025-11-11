using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;
using DomainLayer.Interfaces.Queries;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Providers.GetProviderHealth;

/// <summary>
/// Handler for retrieving provider health status.
/// Uses optimized database view vw_ProviderHealthStatus with 30-day statistics.
/// </summary>
public class GetProviderHealthQueryHandler
    : IQueryHandler<GetProviderHealthQuery, ProviderHealthDto?>
{
    private readonly ISystemViewQueries _systemViewQueries;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<GetProviderHealthQueryHandler> _logger;

    public GetProviderHealthQueryHandler(
        ISystemViewQueries systemViewQueries,
        IDateTimeProvider dateTimeProvider,
        ILogger<GetProviderHealthQueryHandler> logger)
    {
        _systemViewQueries = systemViewQueries;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<ProviderHealthDto?> Handle(
        GetProviderHealthQuery request,
        CancellationToken cancellationToken)
    {
        // Query optimized database view (includes 30-day stats and avg duration)
        var providerHealth = await _systemViewQueries.GetProviderHealthStatusByIdAsync(
            request.ProviderId,
            cancellationToken);

        if (providerHealth == null)
        {
            _logger.LogWarning("Provider {ProviderId} not found", request.ProviderId);
            return null;
        }

        // Calculate time spans from timestamps
        var now = _dateTimeProvider.UtcNow;
        var timeSinceLastSuccess = providerHealth.LastSuccessfulFetch.HasValue
            ? now - providerHealth.LastSuccessfulFetch.Value
            : (TimeSpan?)null;
        var timeSinceLastFailure = providerHealth.LastFailedFetch.HasValue
            ? now - providerHealth.LastFailedFetch.Value
            : (TimeSpan?)null;

        return new ProviderHealthDto
        {
            ProviderId = providerHealth.Id,
            ProviderCode = providerHealth.Code,
            ProviderName = providerHealth.Name,
            Status = providerHealth.HealthStatus,
            IsHealthy = providerHealth.HealthStatus == "Healthy",
            ConsecutiveFailures = providerHealth.ConsecutiveFailures,
            LastSuccessfulFetch = providerHealth.LastSuccessfulFetch,
            LastFailedFetch = providerHealth.LastFailedFetch,
            TimeSinceLastSuccess = timeSinceLastSuccess,
            TimeSinceLastFailure = timeSinceLastFailure
        };
    }
}
