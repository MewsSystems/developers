namespace Mews.Shared.Temporal;

/// <summary>
/// <see cref="IClock"/> implementation that retrieves current system UTC time.
/// </summary>
public class SystemUtcClock : IClock
{
    /// <inheritdoc/>
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
