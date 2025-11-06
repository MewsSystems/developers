using ApplicationLayer.Common.Abstractions;
using DomainLayer.Aggregates.ExchangeRateAggregate;
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

            // Get all unique currency codes from the rates
            var currencyCodes = request.Rates
                .SelectMany(r => new[] { r.SourceCurrencyCode, r.TargetCurrencyCode })
                .Distinct()
                .ToList();

            // Load all currencies and create a lookup map
            var currencyMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var code in currencyCodes)
            {
                var currency = await _unitOfWork.Currencies.GetByCodeAsync(code, cancellationToken);
                if (currency == null)
                {
                    _logger.LogWarning(
                        "Currency {CurrencyCode} not found in database. Skipping rates with this currency.",
                        code);
                    continue;
                }
                currencyMap[code] = currency.Id;
            }

            // Get existing rates for this provider and date
            var existingRates = (await _unitOfWork.ExchangeRates
                .GetByProviderAndDateAsync(request.ProviderId, request.ValidDate, cancellationToken))
                .ToList();

            // Create a lookup for existing rates
            var existingRatesMap = existingRates
                .ToDictionary(r => (r.BaseCurrencyId, r.TargetCurrencyId));

            int inserted = 0;
            int updated = 0;
            int unchanged = 0;
            int skipped = 0;

            // Process each rate
            var ratesToAdd = new List<ExchangeRate>();

            foreach (var rateDto in request.Rates)
            {
                // Skip if currencies not found
                if (!currencyMap.TryGetValue(rateDto.SourceCurrencyCode, out var baseCurrencyId) ||
                    !currencyMap.TryGetValue(rateDto.TargetCurrencyCode, out var targetCurrencyId))
                {
                    skipped++;
                    continue;
                }

                // Check if rate exists
                if (existingRatesMap.TryGetValue((baseCurrencyId, targetCurrencyId), out var existingRate))
                {
                    // Compare values to see if update is needed
                    if (existingRate.Rate != rateDto.Rate || existingRate.Multiplier != (int)rateDto.Multiplier)
                    {
                        existingRate.UpdateRate(rateDto.Rate, (int)rateDto.Multiplier);
                        await _unitOfWork.ExchangeRates.UpdateAsync(existingRate, cancellationToken);
                        updated++;

                        _logger.LogDebug(
                            "Updated rate for {Base}/{Target}: {OldRate}/{OldMultiplier} -> {NewRate}/{NewMultiplier}",
                            rateDto.SourceCurrencyCode,
                            rateDto.TargetCurrencyCode,
                            existingRate.Rate,
                            existingRate.Multiplier,
                            rateDto.Rate,
                            rateDto.Multiplier);
                    }
                    else
                    {
                        unchanged++;
                    }
                }
                else
                {
                    // Create new rate
                    var newRate = ExchangeRate.Create(
                        request.ProviderId,
                        baseCurrencyId,
                        targetCurrencyId,
                        (int)rateDto.Multiplier,
                        rateDto.Rate,
                        request.ValidDate);

                    ratesToAdd.Add(newRate);
                    inserted++;

                    _logger.LogDebug(
                        "Creating new rate for {Base}/{Target}: {Rate}/{Multiplier}",
                        rateDto.SourceCurrencyCode,
                        rateDto.TargetCurrencyCode,
                        rateDto.Rate,
                        rateDto.Multiplier);
                }
            }

            // Bulk insert new rates
            if (ratesToAdd.Any())
            {
                await _unitOfWork.ExchangeRates.AddRangeAsync(ratesToAdd, cancellationToken);
            }

            // Save all changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new BulkUpsertResult(inserted, updated, unchanged, request.Rates.Count);

            _logger.LogInformation(
                "Bulk upsert completed for provider {ProviderId}, date {ValidDate}: " +
                "{Inserted} inserted, {Updated} updated, {Unchanged} unchanged, {Skipped} skipped",
                request.ProviderId,
                request.ValidDate,
                inserted,
                updated,
                unchanged,
                skipped);

            return Result.Success(result);
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
