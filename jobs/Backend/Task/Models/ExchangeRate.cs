namespace ExchangeRateUpdater.Models;

public class ExchangeRate
{
    public string SourceCurrencyCode { get; set; }
    public string TargetCurrencyCode { get; set; }
    public decimal CurrencyValue { get; set; }

    public override string ToString()
    {
        return $"{SourceCurrencyCode}/{TargetCurrencyCode}={CurrencyValue}";
    }
}