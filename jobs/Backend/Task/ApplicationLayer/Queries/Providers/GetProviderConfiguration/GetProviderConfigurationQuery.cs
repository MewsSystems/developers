using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;

namespace ApplicationLayer.Queries.Providers.GetProviderConfiguration;

/// <summary>
/// Query to get detailed configuration for a specific provider including all settings.
/// </summary>
public record GetProviderConfigurationQuery(int ProviderId) : IQuery<ExchangeRateProviderDetailDto?>;
