namespace Mews.Shared.Temporal;

/// <summary>
/// Represents an abstraction for retrieving current time.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Retrieves the current time.
    /// </summary>
    DateTimeOffset Now { get; }
}