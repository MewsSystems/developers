namespace ExchangeRateUpdater.Models;

public class ExchangeRateResponse
{
    public string SourceCurrency { get; set; }
    public string TargetCurrency { get; set; }
    public decimal ExchangeRate { get; set; }
}