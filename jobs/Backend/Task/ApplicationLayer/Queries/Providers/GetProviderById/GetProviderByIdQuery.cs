using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;

namespace ApplicationLayer.Queries.Providers.GetProviderById;

/// <summary>
/// Query to get an exchange rate provider by ID.
/// </summary>
public record GetProviderByIdQuery(int ProviderId) : IQuery<ExchangeRateProviderDetailDto?>;
