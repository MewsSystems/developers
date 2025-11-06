using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;

namespace ApplicationLayer.Queries.Providers.GetProviderHealth;

/// <summary>
/// Query to get detailed health status for a specific provider.
/// </summary>
public record GetProviderHealthQuery(int ProviderId) : IQuery<ProviderHealthDto?>;
