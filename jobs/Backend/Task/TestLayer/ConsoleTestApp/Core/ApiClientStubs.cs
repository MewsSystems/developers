using ConsoleTestApp.Models;

namespace ConsoleTestApp.Core;

/// <summary>
/// Stub implementations for API methods not yet implemented.
/// Returns "Not Implemented" errors with zero latency.
/// </summary>
public static class ApiClientStubs
{
    public static Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetCurrentRatesStub()
    {
        return Task.FromResult((
            new ExchangeRateData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(ConversionResult Data, ApiCallMetrics Metrics)> ConvertCurrencyStub()
    {
        return Task.FromResult((
            new ConversionResult(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(CurrenciesListData Data, ApiCallMetrics Metrics)> GetCurrenciesStub()
    {
        return Task.FromResult((
            new CurrenciesListData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyStub()
    {
        return Task.FromResult((
            new CurrencyData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(ProvidersListData Data, ApiCallMetrics Metrics)> GetProvidersStub()
    {
        return Task.FromResult((
            new ProvidersListData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderStub()
    {
        return Task.FromResult((
            new ProviderData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(ProviderHealthData Data, ApiCallMetrics Metrics)> GetProviderHealthStub()
    {
        return Task.FromResult((
            new ProviderHealthData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(ProviderStatisticsData Data, ApiCallMetrics Metrics)> GetProviderStatisticsStub()
    {
        return Task.FromResult((
            new ProviderStatisticsData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(UsersListData Data, ApiCallMetrics Metrics)> GetUsersStub()
    {
        return Task.FromResult((
            new UsersListData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }

    public static Task<(UserData Data, ApiCallMetrics Metrics)> GetUserStub()
    {
        return Task.FromResult((
            new UserData(),
            new ApiCallMetrics
            {
                Success = false,
                ErrorMessage = "Not implemented yet",
                ResponseTimeMs = 0
            }
        ));
    }
}
