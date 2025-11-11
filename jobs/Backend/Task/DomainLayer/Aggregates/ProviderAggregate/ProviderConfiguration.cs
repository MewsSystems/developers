using DomainLayer.Common;

namespace DomainLayer.Aggregates.ProviderAggregate;

/// <summary>
/// Represents a configuration setting for an exchange rate provider.
/// Part of the ExchangeRateProvider aggregate.
/// </summary>
public class ProviderConfiguration : Entity<int>
{
    public int ProviderId { get; private set; }
    public string SettingKey { get; private set; }
    public string SettingValue { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset Created { get; private set; }
    public DateTimeOffset? Modified { get; private set; }

    // EF Core constructor
    private ProviderConfiguration() : base()
    {
        SettingKey = string.Empty;
        SettingValue = string.Empty;
    }

    private ProviderConfiguration(
        int providerId,
        string settingKey,
        string settingValue,
        string? description = null) : base()
    {
        if (string.IsNullOrWhiteSpace(settingKey))
            throw new ArgumentException("Setting key cannot be null or empty.", nameof(settingKey));

        if (string.IsNullOrWhiteSpace(settingValue))
            throw new ArgumentException("Setting value cannot be null or empty.", nameof(settingValue));

        ProviderId = providerId;
        SettingKey = settingKey.Trim();
        SettingValue = settingValue;
        Description = description;
        Created = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new provider configuration.
    /// </summary>
    public static ProviderConfiguration Create(
        int providerId,
        string settingKey,
        string settingValue,
        string? description = null)
    {
        if (providerId <= 0)
            throw new ArgumentException("Provider ID must be positive.", nameof(providerId));

        return new ProviderConfiguration(providerId, settingKey, settingValue, description);
    }

    /// <summary>
    /// Updates the configuration value.
    /// </summary>
    public void UpdateValue(string newValue, string? newDescription = null)
    {
        if (string.IsNullOrWhiteSpace(newValue))
            throw new ArgumentException("Setting value cannot be null or empty.", nameof(newValue));

        SettingValue = newValue;

        if (newDescription != null)
            Description = newDescription;

        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the value as a specific type.
    /// </summary>
    public T GetValueAs<T>()
    {
        try
        {
            return (T)Convert.ChangeType(SettingValue, typeof(T));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Cannot convert setting '{SettingKey}' value '{SettingValue}' to type {typeof(T).Name}.",
                ex);
        }
    }

    /// <summary>
    /// Tries to get the value as a specific type.
    /// </summary>
    public bool TryGetValueAs<T>(out T? value)
    {
        try
        {
            value = GetValueAs<T>();
            return true;
        }
        catch
        {
            value = default;
            return false;
        }
    }
}
