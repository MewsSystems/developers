using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;

public struct CurrencyDto
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; set; }

    public override string ToString() => Code;
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null
            || obj is not CurrencyDto)
        {
            return false;
        }

        CurrencyDto currObj = (CurrencyDto)obj;
        return string.Equals(Code, currObj.Code);
    }
    public static bool operator ==(CurrencyDto left, CurrencyDto right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CurrencyDto left, CurrencyDto right)
    {
        return !(left == right);
    }
}
