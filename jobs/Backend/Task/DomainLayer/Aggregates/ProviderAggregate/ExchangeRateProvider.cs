using DomainLayer.Enums;
using DomainLayer.Exceptions;
using DomainLayer.ValueObjects;

namespace DomainLayer.Aggregates.ProviderAggregate;

/// <summary>
/// Aggregate root representing an exchange rate provider.
/// Encapsulates provider health monitoring, configuration, and lifecycle management.
/// </summary>
public class ExchangeRateProvider
{
    private const int MaxConsecutiveFailuresBeforeQuarantine = 5;
    private readonly List<ProviderConfiguration> _configurations = new();

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string Url { get; private set; }
    public int BaseCurrencyId { get; private set; }
    public bool RequiresAuthentication { get; private set; }
    public string? ApiKeyVaultReference { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset? LastSuccessfulFetch { get; private set; }
    public DateTimeOffset? LastFailedFetch { get; private set; }
    public int ConsecutiveFailures { get; private set; }
    public DateTimeOffset Created { get; private set; }
    public DateTimeOffset? Modified { get; private set; }

    /// <summary>
    /// Read-only collection of provider configurations.
    /// </summary>
    public IReadOnlyCollection<ProviderConfiguration> Configurations => _configurations.AsReadOnly();

    /// <summary>
    /// Gets the current provider status based on its state.
    /// </summary>
    public ProviderStatus Status
    {
        get
        {
            if (ConsecutiveFailures >= MaxConsecutiveFailuresBeforeQuarantine)
                return ProviderStatus.Quarantined;

            if (!IsActive)
                return ProviderStatus.Inactive;

            if (LastSuccessfulFetch == null && RequiresAuthentication && string.IsNullOrEmpty(ApiKeyVaultReference))
                return ProviderStatus.Pending;

            return ProviderStatus.Active;
        }
    }

    /// <summary>
    /// Checks if the provider is healthy and can be used for fetching rates.
    /// </summary>
    public bool IsHealthy => Status == ProviderStatus.Active && ConsecutiveFailures == 0;

    /// <summary>
    /// Checks if the provider should be quarantined due to failures.
    /// </summary>
    public bool ShouldBeQuarantined => ConsecutiveFailures >= MaxConsecutiveFailuresBeforeQuarantine;

    // EF Core constructor
    private ExchangeRateProvider()
    {
        Name = string.Empty;
        Code = string.Empty;
        Url = string.Empty;
    }

    private ExchangeRateProvider(
        string name,
        string code,
        string url,
        int baseCurrencyId,
        bool requiresAuthentication = false,
        string? apiKeyVaultReference = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Provider name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Provider code cannot be null or empty.", nameof(code));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Provider URL cannot be null or empty.", nameof(url));

        if (baseCurrencyId <= 0)
            throw new ArgumentException("Base currency ID must be positive.", nameof(baseCurrencyId));

        if (requiresAuthentication && string.IsNullOrWhiteSpace(apiKeyVaultReference))
            throw new ArgumentException("API key vault reference is required when authentication is enabled.", nameof(apiKeyVaultReference));

        Name = name.Trim();
        Code = code.Trim().ToUpperInvariant();
        Url = url.Trim();
        BaseCurrencyId = baseCurrencyId;
        RequiresAuthentication = requiresAuthentication;
        ApiKeyVaultReference = apiKeyVaultReference?.Trim();
        IsActive = true;
        ConsecutiveFailures = 0;
        Created = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new exchange rate provider.
    /// </summary>
    public static ExchangeRateProvider Create(
        string name,
        string code,
        string url,
        int baseCurrencyId,
        bool requiresAuthentication = false,
        string? apiKeyVaultReference = null)
    {
        return new ExchangeRateProvider(
            name,
            code,
            url,
            baseCurrencyId,
            requiresAuthentication,
            apiKeyVaultReference);
    }

    /// <summary>
    /// Activates the provider, making it available for fetching rates.
    /// </summary>
    public void Activate()
    {
        if (Status == ProviderStatus.Quarantined)
            throw new ProviderQuarantinedException(Code, ConsecutiveFailures);

        IsActive = true;
        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Deactivates the provider, preventing it from being used for fetching rates.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Records a successful fetch operation.
    /// Resets consecutive failures counter.
    /// </summary>
    public void RecordSuccessfulFetch()
    {
        LastSuccessfulFetch = DateTimeOffset.UtcNow;
        ConsecutiveFailures = 0;
        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Records a failed fetch operation.
    /// Increments consecutive failures counter and may quarantine the provider.
    /// </summary>
    public void RecordFailedFetch()
    {
        LastFailedFetch = DateTimeOffset.UtcNow;
        ConsecutiveFailures++;
        Modified = DateTimeOffset.UtcNow;

        if (ShouldBeQuarantined)
        {
            IsActive = false;
        }
    }

    /// <summary>
    /// Resets the provider health status after manual intervention.
    /// </summary>
    public void ResetHealthStatus()
    {
        ConsecutiveFailures = 0;
        LastFailedFetch = null;
        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Updates the provider's basic information.
    /// </summary>
    public void UpdateInfo(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Provider name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Provider URL cannot be null or empty.", nameof(url));

        Name = name.Trim();
        Url = url.Trim();
        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Updates the authentication configuration.
    /// </summary>
    public void UpdateAuthentication(bool requiresAuthentication, string? apiKeyVaultReference)
    {
        if (requiresAuthentication && string.IsNullOrWhiteSpace(apiKeyVaultReference))
            throw new ArgumentException("API key vault reference is required when authentication is enabled.", nameof(apiKeyVaultReference));

        RequiresAuthentication = requiresAuthentication;
        ApiKeyVaultReference = apiKeyVaultReference?.Trim();
        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Adds or updates a configuration setting.
    /// </summary>
    public void SetConfiguration(string key, string value, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Configuration key cannot be null or empty.", nameof(key));

        var existingConfig = _configurations.FirstOrDefault(c => c.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));

        if (existingConfig != null)
        {
            existingConfig.UpdateValue(value, description);
        }
        else
        {
            var newConfig = ProviderConfiguration.Create(Id, key, value, description);
            _configurations.Add(newConfig);
        }

        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets a configuration value by key.
    /// </summary>
    public string? GetConfigurationValue(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Configuration key cannot be null or empty.", nameof(key));

        return _configurations
            .FirstOrDefault(c => c.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase))
            ?.SettingValue;
    }

    /// <summary>
    /// Gets a configuration value as a specific type.
    /// </summary>
    public T? GetConfigurationValueAs<T>(string key)
    {
        var config = _configurations.FirstOrDefault(c => c.SettingKey.Equals(key, StringComparison.OrdinalIgnoreCase));
        return config != null ? config.GetValueAs<T>() : default;
    }

    /// <summary>
    /// Checks if the provider can be used for fetching rates.
    /// </summary>
    public void EnsureCanFetch()
    {
        if (!IsActive)
            throw new ProviderNotActiveException(Code, Status.ToString());

        if (Status == ProviderStatus.Quarantined)
            throw new ProviderQuarantinedException(Code, ConsecutiveFailures);
    }

    /// <summary>
    /// Gets the time elapsed since the last successful fetch.
    /// </summary>
    public TimeSpan? TimeSinceLastSuccessfulFetch =>
        LastSuccessfulFetch.HasValue ? DateTimeOffset.UtcNow - LastSuccessfulFetch.Value : null;

    /// <summary>
    /// Gets the time elapsed since the last failed fetch.
    /// </summary>
    public TimeSpan? TimeSinceLastFailedFetch =>
        LastFailedFetch.HasValue ? DateTimeOffset.UtcNow - LastFailedFetch.Value : null;
}
