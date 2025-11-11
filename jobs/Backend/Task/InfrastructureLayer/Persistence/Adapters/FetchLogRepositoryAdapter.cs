using DataLayer;
using DataLayer.Dapper;
using DomainLayer.Interfaces.Repositories;

namespace InfrastructureLayer.Persistence.Adapters;

/// <summary>
/// Adapts DataLayer fetch log repository to DomainLayer interface.
/// This bridges the DataLayer and DomainLayer without creating coupling.
/// </summary>
public class FetchLogRepositoryAdapter : IExchangeRateFetchLogRepository
{
    private readonly IUnitOfWork _dataLayerUnitOfWork;
    private readonly IStoredProcedureService _storedProcedureService;

    public FetchLogRepositoryAdapter(
        IUnitOfWork dataLayerUnitOfWork,
        IStoredProcedureService storedProcedureService)
    {
        _dataLayerUnitOfWork = dataLayerUnitOfWork;
        _storedProcedureService = storedProcedureService;
    }

    public async Task<IEnumerable<FetchLogEntry>> GetRecentLogsAsync(
        int count = 50,
        CancellationToken cancellationToken = default)
    {
        var logs = await _dataLayerUnitOfWork.ExchangeRateFetchLogs.GetRecentLogsAsync(count, cancellationToken);
        return logs.Select(MapToFetchLogEntry);
    }

    public async Task<IEnumerable<FetchLogEntry>> GetLogsByProviderAsync(
        int providerId,
        CancellationToken cancellationToken = default)
    {
        var logs = await _dataLayerUnitOfWork.ExchangeRateFetchLogs.GetLogsByProviderAsync(providerId, cancellationToken);
        return logs.Select(MapToFetchLogEntry);
    }

    public async Task<IEnumerable<FetchLogEntry>> GetFailedLogsAsync(
        CancellationToken cancellationToken = default)
    {
        var logs = await _dataLayerUnitOfWork.ExchangeRateFetchLogs.GetFailedLogsAsync(cancellationToken);
        return logs.Select(MapToFetchLogEntry);
    }

    public async Task<IEnumerable<FetchLogEntry>> GetLogsByDateRangeAsync(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default)
    {
        var logs = await _dataLayerUnitOfWork.ExchangeRateFetchLogs.GetLogsByDateRangeAsync(
            startDate,
            endDate,
            cancellationToken);
        return logs.Select(MapToFetchLogEntry);
    }

    public async Task<long> StartFetchLogAsync(
        int providerId,
        int? requestedBy = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _storedProcedureService.StartFetchLogAsync(
            providerId,
            requestedBy,
            cancellationToken);
        return result.LogId;
    }

    public async Task CompleteFetchLogAsync(
        long logId,
        string status,
        int? ratesImported = null,
        int? ratesUpdated = null,
        string? errorMessage = null,
        CancellationToken cancellationToken = default)
    {
        await _storedProcedureService.CompleteFetchLogAsync(
            logId,
            status,
            ratesImported,
            ratesUpdated,
            errorMessage,
            cancellationToken);
    }

    private static FetchLogEntry MapToFetchLogEntry(DataLayer.Entities.ExchangeRateFetchLog log)
    {
        return new FetchLogEntry(
            log.Id,
            log.ProviderId,
            log.Provider.Code,
            log.Provider.Name,
            log.FetchStarted,
            log.FetchCompleted,
            log.Status,
            log.RatesImported,
            log.RatesUpdated,
            log.ErrorMessage,
            log.DurationMs);
    }
}
