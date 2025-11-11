using DomainLayer.ValueObjects;

namespace DomainLayer.Interfaces.Repositories;

/// <summary>
/// Repository for managing currencies.
/// Note: Currency is a value object, but we still need repository operations.
/// </summary>
public interface ICurrencyRepository
{
    /// <summary>
    /// Gets a currency by its unique identifier.
    /// </summary>
    Task<Currency?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a currency by its ISO code.
    /// </summary>
    Task<Currency?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all currencies.
    /// </summary>
    Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active currencies.
    /// </summary>
    Task<IEnumerable<Currency>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a currency with the given code exists.
    /// </summary>
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new currency.
    /// </summary>
    Task AddAsync(Currency currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing currency.
    /// </summary>
    Task UpdateAsync(Currency currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a currency.
    /// </summary>
    Task DeleteAsync(Currency currency, CancellationToken cancellationToken = default);
}
