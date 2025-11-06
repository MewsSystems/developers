namespace DomainLayer.Events.Common;

/// <summary>
/// Interface for entities that can raise domain events.
/// Typically implemented by aggregate roots.
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>
    /// Gets the collection of domain events that have been raised.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears all domain events from the entity.
    /// Should be called after events have been dispatched.
    /// </summary>
    void ClearDomainEvents();
}
