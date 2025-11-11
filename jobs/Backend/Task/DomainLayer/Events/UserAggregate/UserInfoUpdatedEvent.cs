using DomainLayer.Events.Common;

namespace DomainLayer.Events.UserAggregate;

/// <summary>
/// Event raised when a user's information is updated.
/// </summary>
public sealed record UserInfoUpdatedEvent(
    int UserId,
    string Email,
    string FullName,
    DateTimeOffset UpdatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
