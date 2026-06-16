using Domain.ValueTypes;

namespace Domain.Entities;

public class ExchangeRateRequest
{
    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }

    public ExchangeRateRequest(Currency sourceCurrency, Currency targetCurrency)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
    }

    public string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}";
    }
}