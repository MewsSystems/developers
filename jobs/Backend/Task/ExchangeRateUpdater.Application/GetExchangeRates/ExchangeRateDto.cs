namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class ExchangeRateDto
{
    public string SourceCurrency { get; init; }
    public string TargetCurrency { get; init; }
    public decimal Rate { get; init; }
}