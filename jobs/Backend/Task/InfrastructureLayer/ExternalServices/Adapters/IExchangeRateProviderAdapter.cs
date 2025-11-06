using InfrastructureLayer.ExternalServices.Adapters.Models;

namespace InfrastructureLayer.ExternalServices.Adapters;

/// <summary>
/// Adapter that wraps the existing IExchangeRateProvider and adapts it to domain interfaces.
/// Bridges between ExchangeRateProviderLayer and DomainLayer.
/// </summary>
public interface IExchangeRateProviderAdapter
{
    /// <summary>
    /// The provider code identifying this adapter (e.g., "ECB", "CNB").
    /// </summary>
    string ProviderCode { get; }

    /// <summary>
    /// Fetches the latest exchange rates.
    /// </summary>
    Task<ProviderRateResponse> FetchLatestRatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetches historical exchange rates.
    /// </summary>
    Task<ProviderRateResponse> FetchHistoricalRatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the provider is healthy.
    /// </summary>
    Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default);
}
