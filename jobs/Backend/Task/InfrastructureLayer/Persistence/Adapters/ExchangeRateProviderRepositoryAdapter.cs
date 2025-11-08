using DataLayer;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.Interfaces.Repositories;

namespace InfrastructureLayer.Persistence.Adapters;

/// <summary>
/// Adapts DataLayer provider repository to DomainLayer interface.
/// Uses the ExchangeRateProvider.Reconstruct factory method for proper aggregate hydration.
/// </summary>
public class ExchangeRateProviderRepositoryAdapter : IExchangeRateProviderRepository
{
    private readonly IUnitOfWork _dataLayerUnitOfWork;

    public ExchangeRateProviderRepositoryAdapter(IUnitOfWork dataLayerUnitOfWork)
    {
        _dataLayerUnitOfWork = dataLayerUnitOfWork;
    }

    public async Task<ExchangeRateProvider?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.ExchangeRateProviders.GetByIdAsync(id, cancellationToken);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<ExchangeRateProvider?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.ExchangeRateProviders.GetByCodeAsync(code, cancellationToken);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<IEnumerable<ExchangeRateProvider>> GetActiveProvidersAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.ExchangeRateProviders.GetActiveProvidersAsync(cancellationToken);
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<ExchangeRateProvider>> GetQuarantinedProvidersAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.ExchangeRateProviders.GetAllAsync(cancellationToken);
        return entities.Where(e => e.ConsecutiveFailures >= 5).Select(MapToDomain);
    }

    public async Task<IEnumerable<ExchangeRateProvider>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.ExchangeRateProviders.GetAllAsync(cancellationToken);
        return entities.Select(MapToDomain);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.ExchangeRateProviders.GetByCodeAsync(code, cancellationToken);
        return entity != null;
    }

    public async Task AddAsync(ExchangeRateProvider provider, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(provider);
        await _dataLayerUnitOfWork.ExchangeRateProviders.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(ExchangeRateProvider provider, CancellationToken cancellationToken = default)
    {
        // Get the tracked entity and update its properties instead of creating a new entity
        // This avoids EF Core tracking conflicts
        var existingEntity = await _dataLayerUnitOfWork.ExchangeRateProviders.GetByIdAsync(provider.Id, cancellationToken);
        if (existingEntity != null)
        {
            existingEntity.Name = provider.Name;
            existingEntity.Code = provider.Code;
            existingEntity.Url = provider.Url;
            existingEntity.BaseCurrencyId = provider.BaseCurrencyId;
            existingEntity.RequiresAuthentication = provider.RequiresAuthentication;
            existingEntity.ApiKeyVaultReference = provider.ApiKeyVaultReference;
            existingEntity.IsActive = provider.IsActive;
            existingEntity.LastSuccessfulFetch = provider.LastSuccessfulFetch;
            existingEntity.LastFailedFetch = provider.LastFailedFetch;
            existingEntity.ConsecutiveFailures = provider.ConsecutiveFailures;
            existingEntity.Modified = provider.Modified;

            // Sync configurations from domain to data layer
            SyncConfigurations(provider, existingEntity);

            await _dataLayerUnitOfWork.ExchangeRateProviders.UpdateAsync(existingEntity, cancellationToken);
        }
    }

    /// <summary>
    /// Syncs configuration settings from domain aggregate to DataLayer entity.
    /// Adds new configurations, updates existing ones, and removes deleted ones.
    /// </summary>
    private static void SyncConfigurations(ExchangeRateProvider domain, DataLayer.Entities.ExchangeRateProvider entity)
    {
        // Get all domain configurations
        var domainConfigs = domain.Configurations.ToList();

        // Remove configurations that no longer exist in domain
        var configsToRemove = entity.Configurations
            .Where(ec => !domainConfigs.Any(dc => dc.SettingKey.Equals(ec.SettingKey, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var config in configsToRemove)
        {
            entity.Configurations.Remove(config);
        }

        // Add or update configurations
        foreach (var domainConfig in domainConfigs)
        {
            var existingConfig = entity.Configurations
                .FirstOrDefault(ec => ec.SettingKey.Equals(domainConfig.SettingKey, StringComparison.OrdinalIgnoreCase));

            if (existingConfig != null)
            {
                // Update existing
                existingConfig.SettingValue = domainConfig.SettingValue;
                existingConfig.Description = domainConfig.Description;
                existingConfig.Modified = domainConfig.Modified;
            }
            else
            {
                // Add new
                entity.Configurations.Add(new DataLayer.Entities.ExchangeRateProviderConfiguration
                {
                    ProviderId = entity.Id,
                    SettingKey = domainConfig.SettingKey,
                    SettingValue = domainConfig.SettingValue,
                    Description = domainConfig.Description,
                    Created = domainConfig.Created,
                    Modified = domainConfig.Modified
                });
            }
        }
    }

    public async Task DeleteAsync(ExchangeRateProvider provider, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.ExchangeRateProviders.GetByIdAsync(provider.Id, cancellationToken);
        if (entity != null)
        {
            await _dataLayerUnitOfWork.ExchangeRateProviders.DeleteAsync(entity, cancellationToken);
        }
    }

    /// <summary>
    /// Maps DataLayer entity to Domain aggregate.
    /// Uses the Reconstruct factory method for proper aggregate hydration.
    /// </summary>
    private static ExchangeRateProvider MapToDomain(DataLayer.Entities.ExchangeRateProvider entity)
    {
        return ExchangeRateProvider.Reconstruct(
            id: entity.Id,
            name: entity.Name,
            code: entity.Code,
            url: entity.Url,
            baseCurrencyId: entity.BaseCurrencyId,
            requiresAuthentication: entity.RequiresAuthentication,
            apiKeyVaultReference: entity.ApiKeyVaultReference,
            isActive: entity.IsActive,
            lastSuccessfulFetch: entity.LastSuccessfulFetch,
            lastFailedFetch: entity.LastFailedFetch,
            consecutiveFailures: entity.ConsecutiveFailures,
            created: entity.Created,
            modified: entity.Modified);
    }

    private static DataLayer.Entities.ExchangeRateProvider MapToEntity(ExchangeRateProvider domain)
    {
        return new DataLayer.Entities.ExchangeRateProvider
        {
            Id = domain.Id,
            Name = domain.Name,
            Code = domain.Code,
            Url = domain.Url,
            BaseCurrencyId = domain.BaseCurrencyId,
            RequiresAuthentication = domain.RequiresAuthentication,
            ApiKeyVaultReference = domain.ApiKeyVaultReference,
            IsActive = domain.IsActive,
            LastSuccessfulFetch = domain.LastSuccessfulFetch,
            LastFailedFetch = domain.LastFailedFetch,
            ConsecutiveFailures = domain.ConsecutiveFailures,
            Created = domain.Created,
            Modified = domain.Modified
        };
    }
}
