namespace ExchangeRateUpdater.Core.Entities;

public class SupportedCurrenciesOptions
{
    public IReadOnlyList<string> Currencies { get; set; } = default!;
}