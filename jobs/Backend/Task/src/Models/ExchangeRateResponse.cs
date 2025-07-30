namespace ExchangeRateUpdater.Models;

public class ExchangeRateResponse
{
    public string SourceCurrency { get; init; }
    public string TargetCurrency { get; init; }
    public decimal ExchangeRate { get; init; }
}