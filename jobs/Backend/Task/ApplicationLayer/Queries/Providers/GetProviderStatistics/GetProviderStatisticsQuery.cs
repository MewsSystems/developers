using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;

namespace ApplicationLayer.Queries.Providers.GetProviderStatistics;

/// <summary>
/// Query to get performance statistics and metrics for a specific provider.
/// </summary>
public record GetProviderStatisticsQuery(int ProviderId) : IQuery<ProviderStatisticsDto?>;
