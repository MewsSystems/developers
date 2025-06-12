using NodaTime;

namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;

public record ExchangeRateData
{
    public LocalDate Date { get; init; }
    public LocalDate PublishedDate { get; init; }
    public IReadOnlyList<CurrencyRate> Rates { get; init; } = new List<CurrencyRate>();
}