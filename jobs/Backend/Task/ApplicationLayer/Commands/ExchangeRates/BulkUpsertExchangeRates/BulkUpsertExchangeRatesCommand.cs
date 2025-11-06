using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;

/// <summary>
/// Command to bulk insert or update exchange rates from a provider.
/// Used by background jobs to process fetched rates efficiently.
/// </summary>
public record BulkUpsertExchangeRatesCommand(
    int ProviderId,
    DateOnly ValidDate,
    List<ExchangeRateItemDto> Rates) : ICommand<Result<BulkUpsertResult>>;

/// <summary>
/// DTO for individual exchange rate items in the bulk operation.
/// </summary>
public record ExchangeRateItemDto(
    string SourceCurrencyCode,
    string TargetCurrencyCode,
    decimal Rate,
    decimal Multiplier);

/// <summary>
/// Result of the bulk upsert operation.
/// </summary>
public record BulkUpsertResult(
    int RatesInserted,
    int RatesUpdated,
    int RatesUnchanged,
    int TotalProcessed);
