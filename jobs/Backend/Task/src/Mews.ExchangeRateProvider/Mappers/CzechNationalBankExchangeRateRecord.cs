using FileHelpers;

namespace Mews.ExchangeRateProvider.Mappers;

[DelimitedRecord("|")]
[IgnoreFirst(2)]
internal sealed class CzechNationalBankExchangeRateRecord
{
    public string? Country;

    public string? Currency;

    public decimal Amount;

    public string? Code;

    public decimal Rate;
}