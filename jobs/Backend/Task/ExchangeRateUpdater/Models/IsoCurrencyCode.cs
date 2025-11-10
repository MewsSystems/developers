using System.Runtime.Serialization;

namespace ExchangeRateUpdater.Models;

/// <summary>
/// Three-letter ISO 4217 code of the currency.
/// </summary>
//[JsonConverter(typeof(StringEnumConverter))] ???
public enum IsoCurrencyCode
{
    [EnumMember(Value = "USD")]
    USD,

    [EnumMember(Value = "EUR")]
    EUR,

    [EnumMember(Value = "CZK")]
    CZK,

    [EnumMember(Value = "JPY")]
    JPY,

    [EnumMember(Value = "KES")]
    KES,

    [EnumMember(Value = "RUB")]
    RUB,

    [EnumMember(Value = "THB")]
    THB,

    [EnumMember(Value = "TRY")]
    TRY
}