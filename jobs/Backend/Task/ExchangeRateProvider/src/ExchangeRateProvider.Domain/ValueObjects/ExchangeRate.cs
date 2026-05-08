using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Domain.ValueObjects;

[ExcludeFromCodeCoverage(Justification = "Simple value object with only auto-generated record members - no custom logic")]
public record ExchangeRate
{
    public ExchangeRate(Currency baseCurrency, Currency quoteCurrency, decimal rate)
    {
        BaseCurrency = baseCurrency;
        QuoteCurrency = quoteCurrency;
        Rate = rate;
    }

    public Currency BaseCurrency { get; }

    public Currency QuoteCurrency { get; }

    public decimal Rate { get; }
}
