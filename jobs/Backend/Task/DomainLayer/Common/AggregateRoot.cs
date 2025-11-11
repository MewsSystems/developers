using DomainLayer.Events.Common;

namespace DomainLayer.Common;

/// <summary>
/// Base class for aggregate roots.
/// Aggregates are the main entry points for domain operations and manage domain events.
/// </summary>
/// <typeparam name="TId">The type of the aggregate identifier</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvents
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the collection of domain events raised by this aggregate.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// Adds a domain event to be published when the aggregate is persisted.
    /// </summary>
    /// <param name="domainEvent">The domain event to add</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the aggregate.
    /// Called after events have been dispatched.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
