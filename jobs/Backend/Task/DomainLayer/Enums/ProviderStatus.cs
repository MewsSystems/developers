namespace DomainLayer.Enums;

/// <summary>
/// Represents the operational status of an exchange rate provider.
/// </summary>
public enum ProviderStatus
{
    /// <summary>
    /// Provider is active and operational.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Provider is temporarily disabled.
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// Provider is quarantined due to consecutive failures.
    /// Requires manual intervention to reactivate.
    /// </summary>
    Quarantined = 3,

    /// <summary>
    /// Provider configuration is pending or incomplete.
    /// </summary>
    Pending = 4
}
