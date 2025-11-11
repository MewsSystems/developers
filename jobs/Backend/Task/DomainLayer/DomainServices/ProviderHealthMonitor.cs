using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.Enums;

namespace DomainLayer.DomainServices;

/// <summary>
/// Domain service responsible for monitoring and evaluating provider health.
/// </summary>
public class ProviderHealthMonitor
{
    private readonly TimeSpan _staleDataThreshold;
    private readonly int _criticalFailureThreshold;

    public ProviderHealthMonitor(
        TimeSpan? staleDataThreshold = null,
        int criticalFailureThreshold = 3)
    {
        _staleDataThreshold = staleDataThreshold ?? TimeSpan.FromHours(24);
        _criticalFailureThreshold = criticalFailureThreshold;
    }

    /// <summary>
    /// Evaluates the overall health of a provider.
    /// </summary>
    public ProviderHealthStatus EvaluateHealth(ExchangeRateProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        var status = new ProviderHealthStatus
        {
            ProviderId = provider.Id,
            ProviderCode = provider.Code,
            Status = provider.Status,
            IsHealthy = provider.IsHealthy,
            ConsecutiveFailures = provider.ConsecutiveFailures,
            LastSuccessfulFetch = provider.LastSuccessfulFetch,
            LastFailedFetch = provider.LastFailedFetch,
            TimeSinceLastSuccess = provider.TimeSinceLastSuccessfulFetch,
            TimeSinceLastFailure = provider.TimeSinceLastFailedFetch
        };

        // Determine if data is stale
        status.IsDataStale = provider.LastSuccessfulFetch.HasValue &&
                            provider.TimeSinceLastSuccessfulFetch > _staleDataThreshold;

        // Determine health level
        status.HealthLevel = DetermineHealthLevel(provider);

        // Generate recommendations
        status.Recommendations = GenerateRecommendations(provider, status);

        return status;
    }

    /// <summary>
    /// Evaluates health for multiple providers and returns a summary.
    /// </summary>
    public ProviderHealthSummary EvaluateHealthForAll(IEnumerable<ExchangeRateProvider> providers)
    {
        if (providers == null)
            throw new ArgumentNullException(nameof(providers));

        var providerList = providers.ToList();
        var healthStatuses = providerList.Select(EvaluateHealth).ToList();

        return new ProviderHealthSummary
        {
            TotalProviders = providerList.Count,
            HealthyProviders = healthStatuses.Count(s => s.IsHealthy),
            UnhealthyProviders = healthStatuses.Count(s => !s.IsHealthy),
            QuarantinedProviders = healthStatuses.Count(s => s.Status == ProviderStatus.Quarantined),
            ProvidersWithStaleData = healthStatuses.Count(s => s.IsDataStale),
            ProviderStatuses = healthStatuses,
            OverallHealth = CalculateOverallHealth(healthStatuses)
        };
    }

    /// <summary>
    /// Checks if a provider should be automatically recovered from quarantine.
    /// </summary>
    public bool CanAutoRecover(ExchangeRateProvider provider, TimeSpan quarantineDuration)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        if (provider.Status != ProviderStatus.Quarantined)
            return false;

        // Only auto-recover if enough time has passed since the last failure
        if (provider.LastFailedFetch.HasValue)
        {
            var timeSinceFailure = DateTimeOffset.UtcNow - provider.LastFailedFetch.Value;
            return timeSinceFailure >= quarantineDuration;
        }

        return false;
    }

    private HealthLevel DetermineHealthLevel(ExchangeRateProvider provider)
    {
        if (provider.Status == ProviderStatus.Quarantined)
            return HealthLevel.Critical;

        if (!provider.IsActive)
            return HealthLevel.Inactive;

        if (provider.ConsecutiveFailures >= _criticalFailureThreshold)
            return HealthLevel.Critical;

        if (provider.ConsecutiveFailures > 0)
            return HealthLevel.Warning;

        if (provider.LastSuccessfulFetch.HasValue &&
            provider.TimeSinceLastSuccessfulFetch > _staleDataThreshold)
            return HealthLevel.Warning;

        if (provider.LastSuccessfulFetch == null)
            return HealthLevel.Unknown;

        return HealthLevel.Healthy;
    }

    private List<string> GenerateRecommendations(ExchangeRateProvider provider, ProviderHealthStatus status)
    {
        var recommendations = new List<string>();

        if (status.Status == ProviderStatus.Quarantined)
        {
            recommendations.Add("Provider is quarantined. Manual intervention required to reset health status.");
        }

        if (status.IsDataStale)
        {
            recommendations.Add($"Data is stale (last successful fetch: {provider.LastSuccessfulFetch:yyyy-MM-dd HH:mm:ss UTC}). Consider triggering a manual fetch.");
        }

        if (status.ConsecutiveFailures > 0 && status.ConsecutiveFailures < 5)
        {
            recommendations.Add($"Provider has {status.ConsecutiveFailures} consecutive failures. Monitor closely.");
        }

        if (!provider.IsActive && provider.Status != ProviderStatus.Quarantined)
        {
            recommendations.Add("Provider is inactive. Activate to resume fetching exchange rates.");
        }

        if (provider.LastSuccessfulFetch == null && provider.RequiresAuthentication)
        {
            recommendations.Add("Provider has never successfully fetched data. Verify authentication configuration.");
        }

        return recommendations;
    }

    private double CalculateOverallHealth(List<ProviderHealthStatus> statuses)
    {
        if (statuses.Count == 0)
            return 0;

        var healthyCount = statuses.Count(s => s.HealthLevel == HealthLevel.Healthy);
        return (double)healthyCount / statuses.Count * 100;
    }
}

/// <summary>
/// Represents the health status of a provider.
/// </summary>
public class ProviderHealthStatus
{
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public ProviderStatus Status { get; set; }
    public bool IsHealthy { get; set; }
    public int ConsecutiveFailures { get; set; }
    public DateTimeOffset? LastSuccessfulFetch { get; set; }
    public DateTimeOffset? LastFailedFetch { get; set; }
    public TimeSpan? TimeSinceLastSuccess { get; set; }
    public TimeSpan? TimeSinceLastFailure { get; set; }
    public bool IsDataStale { get; set; }
    public HealthLevel HealthLevel { get; set; }
    public List<string> Recommendations { get; set; } = new();
}

/// <summary>
/// Represents a summary of health across all providers.
/// </summary>
public class ProviderHealthSummary
{
    public int TotalProviders { get; set; }
    public int HealthyProviders { get; set; }
    public int UnhealthyProviders { get; set; }
    public int QuarantinedProviders { get; set; }
    public int ProvidersWithStaleData { get; set; }
    public double OverallHealth { get; set; }
    public List<ProviderHealthStatus> ProviderStatuses { get; set; } = new();
}

/// <summary>
/// Represents the health level of a provider.
/// </summary>
public enum HealthLevel
{
    Unknown = 0,
    Healthy = 1,
    Warning = 2,
    Critical = 3,
    Inactive = 4
}
