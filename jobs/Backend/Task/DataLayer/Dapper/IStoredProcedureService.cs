using DataLayer.DTOs;

namespace DataLayer.Dapper;

public interface IStoredProcedureService
{
    Task<BulkUpsertResult> BulkUpsertExchangeRatesAsync(
        int providerId,
        DateOnly validDate,
        IEnumerable<ExchangeRateInput> rates,
        CancellationToken cancellationToken = default);

    Task<StartFetchLogResult> StartFetchLogAsync(
        int providerId,
        int? requestedBy = null,
        CancellationToken cancellationToken = default);

    Task<CompleteFetchLogResult> CompleteFetchLogAsync(
        long logId,
        string status,
        int? ratesImported = null,
        int? ratesUpdated = null,
        string? errorMessage = null,
        CancellationToken cancellationToken = default);
}
