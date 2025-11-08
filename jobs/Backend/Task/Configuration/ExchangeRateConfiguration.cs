namespace ExchangeRateUpdater.Configuration;

public sealed record ExchangeRateConfiguration
{
    public IEnumerable<string> CurrencyCodes { get; init; } = [];
}