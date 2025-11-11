using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.Currencies;

namespace ApplicationLayer.Queries.Currencies.GetAllCurrencies;

/// <summary>
/// Query to get all currencies with optional pagination and search.
/// </summary>
public record GetAllCurrenciesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool IncludePagination = true) : IQuery<PagedResult<CurrencyDto>>;
