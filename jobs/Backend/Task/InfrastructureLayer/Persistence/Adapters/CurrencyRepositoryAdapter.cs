using DataLayer;
using DomainLayer.Interfaces.Repositories;
using DomainLayer.ValueObjects;

namespace InfrastructureLayer.Persistence.Adapters;

/// <summary>
/// Adapts DataLayer currency repository to DomainLayer interface.
/// Handles mapping between DataLayer.Entities.Currency and DomainLayer.ValueObjects.Currency.
/// </summary>
public class CurrencyRepositoryAdapter : ICurrencyRepository
{
    private readonly IUnitOfWork _dataLayerUnitOfWork;

    public CurrencyRepositoryAdapter(IUnitOfWork dataLayerUnitOfWork)
    {
        _dataLayerUnitOfWork = dataLayerUnitOfWork;
    }

    public async Task<Currency?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.Currencies.GetByIdAsync(id, cancellationToken);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<Currency?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.Currencies.GetByCodeAsync(code, cancellationToken);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.Currencies.GetAllAsync(cancellationToken);
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Currency>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.Currencies.GetActiveCurrenciesAsync(cancellationToken);
        return entities.Select(MapToDomain);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.Currencies.GetByCodeAsync(code, cancellationToken);
        return entity != null;
    }

    public async Task AddAsync(Currency currency, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(currency);
        await _dataLayerUnitOfWork.Currencies.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Currency currency, CancellationToken cancellationToken = default)
    {
        // Get the existing entity
        var existingEntity = await _dataLayerUnitOfWork.Currencies.GetByIdAsync(currency.Id, cancellationToken);
        if (existingEntity == null)
        {
            throw new InvalidOperationException($"Cannot update currency with Id {currency.Id}: entity not found.");
        }

        var entity = MapToEntity(currency);
        await _dataLayerUnitOfWork.Currencies.UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(Currency currency, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.Currencies.GetByIdAsync(currency.Id, cancellationToken);
        if (entity != null)
        {
            await _dataLayerUnitOfWork.Currencies.DeleteAsync(entity, cancellationToken);
        }
    }

    private static Currency MapToDomain(DataLayer.Entities.Currency entity)
    {
        return Currency.FromCode(entity.Code, entity.Id);
    }

    private static DataLayer.Entities.Currency MapToEntity(Currency domain)
    {
        return new DataLayer.Entities.Currency
        {
            Id = domain.Id,
            Code = domain.Code
        };
    }
}
