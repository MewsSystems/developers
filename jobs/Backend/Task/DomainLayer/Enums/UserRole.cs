namespace DomainLayer.Enums;

/// <summary>
/// Represents the role of a user in the system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Consumer role - can view exchange rates and use the API.
    /// </summary>
    Consumer = 1,

    /// <summary>
    /// Administrator role - full access to manage providers, view logs, and system configuration.
    /// </summary>
    Admin = 2
}
