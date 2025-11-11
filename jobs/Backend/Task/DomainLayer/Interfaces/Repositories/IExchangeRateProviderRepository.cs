using DomainLayer.Aggregates.ProviderAggregate;

namespace DomainLayer.Interfaces.Repositories;

/// <summary>
/// Repository for managing exchange rate provider aggregates.
/// </summary>
public interface IExchangeRateProviderRepository
{
    /// <summary>
    /// Gets a provider by its unique identifier.
    /// </summary>
    Task<ExchangeRateProvider?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a provider by its unique code.
    /// </summary>
    Task<ExchangeRateProvider?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active providers that can be used for fetching rates.
    /// </summary>
    Task<IEnumerable<ExchangeRateProvider>> GetActiveProvidersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all quarantined providers that have exceeded failure thresholds.
    /// </summary>
    Task<IEnumerable<ExchangeRateProvider>> GetQuarantinedProvidersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all providers.
    /// </summary>
    Task<IEnumerable<ExchangeRateProvider>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a provider with the given code exists.
    /// </summary>
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new provider aggregate.
    /// </summary>
    Task AddAsync(ExchangeRateProvider provider, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing provider aggregate.
    /// </summary>
    Task UpdateAsync(ExchangeRateProvider provider, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a provider aggregate.
    /// </summary>
    Task DeleteAsync(ExchangeRateProvider provider, CancellationToken cancellationToken = default);
}
