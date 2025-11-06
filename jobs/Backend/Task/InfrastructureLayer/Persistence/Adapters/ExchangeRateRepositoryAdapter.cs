using DataLayer;
using DataLayer.Dapper;
using DomainLayer.Aggregates.ExchangeRateAggregate;
using DomainLayer.Interfaces.Repositories;

namespace InfrastructureLayer.Persistence.Adapters;

/// <summary>
/// Adapts DataLayer exchange rate repository to DomainLayer interface.
/// Uses the ExchangeRate.Reconstruct factory method for proper aggregate hydration.
/// </summary>
public class ExchangeRateRepositoryAdapter : IExchangeRateRepository
{
    private readonly IUnitOfWork _dataLayerUnitOfWork;
    private readonly IStoredProcedureService _storedProcedureService;

    public ExchangeRateRepositoryAdapter(
        IUnitOfWork dataLayerUnitOfWork,
        IStoredProcedureService storedProcedureService)
    {
        _dataLayerUnitOfWork = dataLayerUnitOfWork;
        _storedProcedureService = storedProcedureService;
    }

    public async Task<ExchangeRate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.ExchangeRates.GetByIdAsync(id, cancellationToken);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<IEnumerable<ExchangeRate>> GetByProviderAndDateAsync(
        int providerId,
        DateOnly validDate,
        CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.ExchangeRates.GetAllAsync(cancellationToken);
        return entities
            .Where(e => e.ProviderId == providerId && e.ValidDate == validDate)
            .Select(MapToDomain);
    }

    public async Task<ExchangeRate?> GetLatestRateAsync(
        int baseCurrencyId,
        int targetCurrencyId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.ExchangeRates.GetRatesByCurrencyPairAsync(
            baseCurrencyId,
            targetCurrencyId,
            cancellationToken);

        var latest = entities.OrderByDescending(e => e.ValidDate).ThenByDescending(e => e.Created).FirstOrDefault();
        return latest != null ? MapToDomain(latest) : null;
    }

    public async Task<IEnumerable<ExchangeRate>> GetHistoryAsync(
        int baseCurrencyId,
        int targetCurrencyId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.ExchangeRates.GetRatesByCurrencyPairAsync(
            baseCurrencyId,
            targetCurrencyId,
            cancellationToken);

        return entities
            .Where(e => e.ValidDate >= startDate && e.ValidDate <= endDate)
            .OrderBy(e => e.ValidDate)
            .Select(MapToDomain);
    }

    public async Task<IEnumerable<ExchangeRate>> GetByProviderAndDateRangeAsync(
        int providerId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.ExchangeRates.GetRatesByProviderAndDateRangeAsync(
            providerId,
            startDate,
            endDate,
            cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task AddAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(exchangeRate);
        await _dataLayerUnitOfWork.ExchangeRates.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<ExchangeRate> exchangeRates, CancellationToken cancellationToken = default)
    {
        var entities = exchangeRates.Select(MapToEntity);
        await _dataLayerUnitOfWork.ExchangeRates.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(exchangeRate);
        await _dataLayerUnitOfWork.ExchangeRates.UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.ExchangeRates.GetByIdAsync(exchangeRate.Id, cancellationToken);
        if (entity != null)
        {
            await _dataLayerUnitOfWork.ExchangeRates.DeleteAsync(entity, cancellationToken);
        }
    }

    public async Task<BulkExchangeRateResult> BulkUpsertAsync(
        int providerId,
        DateOnly validDate,
        IEnumerable<BulkExchangeRateItem> rates,
        CancellationToken cancellationToken = default)
    {
        // Convert domain DTOs to DataLayer DTOs
        var dataLayerRates = rates.Select(r => new DataLayer.DTOs.ExchangeRateInput
        {
            CurrencyCode = r.CurrencyCode,
            Rate = r.Rate,
            Multiplier = r.Multiplier
        });

        // Call the stored procedure via DataLayer service
        var result = await _storedProcedureService.BulkUpsertExchangeRatesAsync(
            providerId,
            validDate,
            dataLayerRates,
            cancellationToken);

        // Convert DataLayer result to Domain result
        return new BulkExchangeRateResult(
            InsertedCount: result.InsertedCount,
            UpdatedCount: result.UpdatedCount,
            SkippedCount: result.SkippedCount,
            ProcessedCount: result.ProcessedCount,
            TotalInJson: result.TotalInJson,
            Status: result.Status);
    }

    /// <summary>
    /// Maps DataLayer entity to Domain aggregate.
    /// Uses the Reconstruct factory method for proper aggregate hydration.
    /// </summary>
    private static ExchangeRate MapToDomain(DataLayer.Entities.ExchangeRate entity)
    {
        return ExchangeRate.Reconstruct(
            id: entity.Id,
            providerId: entity.ProviderId,
            baseCurrencyId: entity.BaseCurrencyId,
            targetCurrencyId: entity.TargetCurrencyId,
            multiplier: entity.Multiplier,
            rate: entity.Rate,
            validDate: entity.ValidDate,
            created: entity.Created,
            modified: entity.Modified);
    }

    private static DataLayer.Entities.ExchangeRate MapToEntity(ExchangeRate domain)
    {
        return new DataLayer.Entities.ExchangeRate
        {
            Id = domain.Id,
            ProviderId = domain.ProviderId,
            BaseCurrencyId = domain.BaseCurrencyId,
            TargetCurrencyId = domain.TargetCurrencyId,
            Multiplier = domain.Multiplier,
            Rate = domain.Rate,
            ValidDate = domain.ValidDate,
            Created = domain.Created,
            Modified = domain.Modified
        };
    }
}
