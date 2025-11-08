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
    Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetCurrentRatesGroupedAsync();
    Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetLatestRateAsync(string sourceCurrency, string targetCurrency, int? providerId = null);
    Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetLatestRatesAsync();
    Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetHistoricalRatesAsync(DateTime from, DateTime to);
    Task<(ConversionResult Data, ApiCallMetrics Metrics)> ConvertCurrencyAsync(string from, string to, decimal amount);

    // Currencies
    Task<(CurrenciesListData Data, ApiCallMetrics Metrics)> GetCurrenciesAsync();
    Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyAsync(int id);
    Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyByCodeAsync(string code);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateCurrencyAsync(string code);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteCurrencyAsync(string code);

    // Providers
    Task<(ProvidersListData Data, ApiCallMetrics Metrics)> GetProvidersAsync();
    Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderAsync(int id);
    Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderByCodeAsync(string code);
    Task<(ProviderHealthData Data, ApiCallMetrics Metrics)> GetProviderHealthAsync(string code);
    Task<(ProviderStatisticsData Data, ApiCallMetrics Metrics)> GetProviderStatisticsAsync(string code);
    Task<(ProviderConfigurationData Data, ApiCallMetrics Metrics)> GetProviderConfigurationAsync(string code);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> ActivateProviderAsync(string code);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> DeactivateProviderAsync(string code);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> ResetProviderHealthAsync(string code);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> TriggerManualFetchAsync(string code);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateProviderAsync(string name, string code, string url, int baseCurrencyId, bool requiresAuth, string? apiKeyRef);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> UpdateProviderConfigurationAsync(string code, string name, string url, bool requiresAuth, string? apiKeyRef);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteProviderAsync(string code, bool force);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> RescheduleProviderAsync(string code, string updateTime, string timeZone);

    // Users (Admin only)
    Task<(UsersListData Data, ApiCallMetrics Metrics)> GetUsersAsync();
    Task<(UserData Data, ApiCallMetrics Metrics)> GetUserAsync(int id);
    Task<(UserData Data, ApiCallMetrics Metrics)> GetUserByEmailAsync(string email);
    Task<(UsersListData Data, ApiCallMetrics Metrics)> GetUsersByRoleAsync(string role);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> CheckEmailExistsAsync(string email);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateUserAsync(string email, string password, string firstName, string lastName, string role);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> UpdateUserAsync(int id, string firstName, string lastName);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> ChangePasswordAsync(int id, string currentPassword, string newPassword);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> ChangeUserRoleAsync(int id, string newRole);
    Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteUserAsync(int id);

    // System Health (Admin only)
    Task<(SystemHealthData Data, ApiCallMetrics Metrics)> GetSystemHealthAsync();
    Task<(ErrorsListData Data, ApiCallMetrics Metrics)> GetRecentErrorsAsync(int count, string? severity);
    Task<(FetchActivityListData Data, ApiCallMetrics Metrics)> GetFetchActivityAsync(int count, int? providerId, bool failedOnly);

    // Streaming (for real-time updates)
    Task StartStreamingAsync(Action<ExchangeRateData> onUpdate, CancellationToken cancellationToken);
    Task StopStreamingAsync();

    // Health Check
    Task<bool> IsApiAvailableAsync();
}
