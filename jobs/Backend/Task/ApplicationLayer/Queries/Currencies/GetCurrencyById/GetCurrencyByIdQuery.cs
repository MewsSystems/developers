using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Currencies;

namespace ApplicationLayer.Queries.Currencies.GetCurrencyById;

/// <summary>
/// Query to get a currency by its unique identifier.
/// </summary>
public record GetCurrencyByIdQuery(int CurrencyId) : IQuery<CurrencyDto?>;
