using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.ExchangeRateProviders;

namespace ApplicationLayer.Queries.Providers.GetAllProviders;

/// <summary>
/// Query to get all exchange rate providers with optional pagination and filtering.
/// </summary>
public record GetAllProvidersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    bool? IsActive = null,
    string? SearchTerm = null) : IQuery<PagedResult<ExchangeRateProviderDto>>;
