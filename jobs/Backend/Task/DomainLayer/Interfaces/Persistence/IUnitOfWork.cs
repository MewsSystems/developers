using DomainLayer.Interfaces.Repositories;

namespace DomainLayer.Interfaces.Persistence;

/// <summary>
/// Unit of Work pattern for coordinating changes to multiple aggregates.
/// Ensures transactional consistency and dispatches domain events.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the exchange rate provider repository.
    /// </summary>
    IExchangeRateProviderRepository ExchangeRateProviders { get; }

    /// <summary>
    /// Gets the exchange rate repository.
    /// </summary>
    IExchangeRateRepository ExchangeRates { get; }

    /// <summary>
    /// Gets the currency repository.
    /// </summary>
    ICurrencyRepository Currencies { get; }

    /// <summary>
    /// Gets the user repository.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Gets the fetch log repository (read-only monitoring).
    /// </summary>
    Repositories.IExchangeRateFetchLogRepository FetchLogs { get; }

    /// <summary>
    /// Gets the error log repository (read-only monitoring).
    /// </summary>
    Repositories.IErrorLogRepository ErrorLogs { get; }

    /// <summary>
    /// Saves all changes made in this unit of work and dispatches domain events.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
