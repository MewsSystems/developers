namespace ExchangeRateUpdater.Infrastructure.CNB.Entities;

public record CnbExchangeResponseEntity
{
    public required CnbExchangeRateEntity[] Rates { get; init; }
}