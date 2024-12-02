using ExchangeRateUpdater.Application.Services.Models;
using System.Text.Json;

namespace ExchangeRateUpdater.Application.Services;

internal interface IExternalExchangeRateProvider
{
    ValueTask<ExchangeRateProviderResponse> GetDailyExchangeRates(DateTime? forDate);
}

internal class ExternalExchangeRateProvider : IExternalExchangeRateProvider
{
    private readonly ExternalExchangeRateProviderHttpClient _httpClient;

    public ExternalExchangeRateProvider(ExternalExchangeRateProviderHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async ValueTask<ExchangeRateProviderResponse> GetDailyExchangeRates(DateTime? forDate)
    {
        var date = forDate ?? DateTime.UtcNow;
        var response = await _httpClient.Client.GetAsync($"exrates/daily?date={date.ToString("yyyy-MM-dd")}&lang=EN");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var exchangeRateServiceResponse = JsonSerializer.Deserialize<ExchangeRateProviderResponse>(content);

        if (exchangeRateServiceResponse is null) return new ExchangeRateProviderResponse(Rates: new List<ExchangeRateProviderRate>());

        return exchangeRateServiceResponse;
    }
}
