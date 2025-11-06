using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;

public class BulkUpsertExchangeRatesCommandHandler
    : ICommandHandler<BulkUpsertExchangeRatesCommand, Result<BulkUpsertResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BulkUpsertExchangeRatesCommandHandler> _logger;

    public BulkUpsertExchangeRatesCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<BulkUpsertExchangeRatesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<BulkUpsertResult>> Handle(
        BulkUpsertExchangeRatesCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Verify provider exists
            var provider = await _unitOfWork.ExchangeRateProviders
                .GetByIdAsync(request.ProviderId, cancellationToken);

            if (provider == null)
            {
                _logger.LogError("Provider {ProviderId} not found", request.ProviderId);
                return Result.Failure<BulkUpsertResult>($"Provider with ID {request.ProviderId} not found.");
            }

            // Convert ApplicationLayer DTOs to DomainLayer DTOs for bulk operation
            // The stored procedure expects target currency codes relative to the provider's base currency
            var bulkRates = request.Rates
                .Select(r => new DomainLayer.Interfaces.Repositories.BulkExchangeRateItem(
                    CurrencyCode: r.TargetCurrencyCode,
                    Rate: r.Rate,
                    Multiplier: (int)r.Multiplier))
                .ToList();

            // Use the optimized bulk upsert stored procedure
            var result = await _unitOfWork.ExchangeRates.BulkUpsertAsync(
                request.ProviderId,
                request.ValidDate,
                bulkRates,
                cancellationToken);

            // Map domain result to application result
            var appResult = new BulkUpsertResult(
                RatesInserted: result.InsertedCount,
                RatesUpdated: result.UpdatedCount,
                RatesUnchanged: result.ProcessedCount - result.InsertedCount - result.UpdatedCount,
                TotalProcessed: result.ProcessedCount);

            _logger.LogInformation(
                "Bulk upsert completed for provider {ProviderId}, date {ValidDate}: " +
                "{Inserted} inserted, {Updated} updated, {Skipped} skipped, Status: {Status}",
                request.ProviderId,
                request.ValidDate,
                result.InsertedCount,
                result.UpdatedCount,
                result.SkippedCount,
                result.Status);

            return Result.Success(appResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error during bulk upsert for provider {ProviderId}, date {ValidDate}",
                request.ProviderId,
                request.ValidDate);

            return Result.Failure<BulkUpsertResult>($"Error processing exchange rates: {ex.Message}");
        }
    }
}
