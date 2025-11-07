using ConsoleTestApp.Models;

namespace ConsoleTestApp.Core;

/// <summary>
/// Common interface for all API clients (REST, SOAP, gRPC).
/// Provides unified testing interface regardless of protocol.
/// </summary>
public interface IApiClient
{
    string Protocol { get; }
    bool IsAuthenticated { get; }
    bool SupportsStreaming { get; }

    // Authentication
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task LogoutAsync();

    // Exchange Rates
    Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetCurrentRatesAsync();
    Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetLatestRatesAsync();
    Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetHistoricalRatesAsync(DateTime from, DateTime to);
    Task<(ConversionResult Data, ApiCallMetrics Metrics)> ConvertCurrencyAsync(string from, string to, decimal amount);

    // Currencies
    Task<(CurrenciesListData Data, ApiCallMetrics Metrics)> GetCurrenciesAsync();
    Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyAsync(string code);

    // Providers
    Task<(ProvidersListData Data, ApiCallMetrics Metrics)> GetProvidersAsync();
    Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderAsync(string code);
    Task<(ProviderHealthData Data, ApiCallMetrics Metrics)> GetProviderHealthAsync(string code);
    Task<(ProviderStatisticsData Data, ApiCallMetrics Metrics)> GetProviderStatisticsAsync(string code);

    // Users (Admin only)
    Task<(UsersListData Data, ApiCallMetrics Metrics)> GetUsersAsync();
    Task<(UserData Data, ApiCallMetrics Metrics)> GetUserAsync(int id);

    // Streaming (for real-time updates)
    Task StartStreamingAsync(Action<ExchangeRateData> onUpdate, CancellationToken cancellationToken);
    Task StopStreamingAsync();

    // Health Check
    Task<bool> IsApiAvailableAsync();
}
