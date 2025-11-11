using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Currencies;

namespace ApplicationLayer.Queries.Currencies.GetCurrencyByCode;

/// <summary>
/// Query to get a currency by its ISO 4217 code.
/// </summary>
public record GetCurrencyByCodeQuery(string Code) : IQuery<CurrencyDto?>;
