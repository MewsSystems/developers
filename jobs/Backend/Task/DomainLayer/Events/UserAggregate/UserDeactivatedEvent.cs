using DomainLayer.Events.Common;

namespace DomainLayer.Events.UserAggregate;

/// <summary>
/// Event raised when a user account is deactivated.
/// </summary>
public sealed record UserDeactivatedEvent(
    int UserId,
    string Email,
    DateTimeOffset DeactivatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
