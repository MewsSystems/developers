namespace ExchangeRateUpdater.Core.Models;

public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateTime? dateValid)
{
    public Currency SourceCurrency => sourceCurrency;

    public Currency TargetCurrency => targetCurrency;

    public decimal Value => value;

    public DateTime? DateValid => dateValid;

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}