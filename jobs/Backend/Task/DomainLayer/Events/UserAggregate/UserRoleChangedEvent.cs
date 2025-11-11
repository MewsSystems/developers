using DomainLayer.Enums;
using DomainLayer.Events.Common;

namespace DomainLayer.Events.UserAggregate;

/// <summary>
/// Event raised when a user's role is changed.
/// </summary>
public sealed record UserRoleChangedEvent(
    int UserId,
    string Email,
    UserRole OldRole,
    UserRole NewRole,
    DateTimeOffset ChangedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
