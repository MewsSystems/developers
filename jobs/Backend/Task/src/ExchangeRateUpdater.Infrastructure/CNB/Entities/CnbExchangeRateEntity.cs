namespace ExchangeRateUpdater.Infrastructure.CNB.Entities;

public class CnbExchangeRateEntity
{
    public required DateOnly ValidFor { get; init; }
    public required int Order { get; init; }
    public required string Country { get; init; }
    public required string Currency { get; init; }
    public required int Amount { get; init; }
    public required string CurrencyCode { get; init; }
    public decimal Rate { get; init; }
}