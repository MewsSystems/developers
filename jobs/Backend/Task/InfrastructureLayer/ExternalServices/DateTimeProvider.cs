using DomainLayer.Interfaces.Services;

namespace InfrastructureLayer.ExternalServices;

/// <summary>
/// Default implementation of IDateTimeProvider.
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);
}
