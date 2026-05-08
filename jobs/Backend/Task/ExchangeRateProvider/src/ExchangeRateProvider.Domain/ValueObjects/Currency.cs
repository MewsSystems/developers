using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Domain.ValueObjects;

[ExcludeFromCodeCoverage(Justification = "Simple value object with only auto-generated record members - no custom logic")]
public record Currency
{
    public Currency(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
