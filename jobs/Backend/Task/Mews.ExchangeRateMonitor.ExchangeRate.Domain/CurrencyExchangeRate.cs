namespace Mews.ExchangeRateMonitor.ExchangeRate.Domain;

public class CurrencyExchangeRate
{
    public CurrencyExchangeRate(
        Currency sourceCurrency,
        Currency targetCurrency,
        decimal sourceCurrencyAmount,
        decimal targetCurrencyRate)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        SourceCurrencyAmount = sourceCurrencyAmount;
        TargetCurrencyRate = targetCurrencyRate;
    }

    public Currency SourceCurrency { get; }
    public decimal SourceCurrencyAmount { get; }

    public Currency TargetCurrency { get; }
    public decimal TargetCurrencyRate { get; }

    public override string ToString() =>
        $"{SourceCurrencyAmount} {SourceCurrency}  = {TargetCurrencyRate} {TargetCurrency} ";
}