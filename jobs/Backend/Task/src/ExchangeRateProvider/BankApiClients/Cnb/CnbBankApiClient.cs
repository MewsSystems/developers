using System.Text.Json;
using ExchangeRateProvider.BankApiClients.Cnb.Models;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.BankApiClients.Cnb;

// API specification: https://api.cnb.cz/cnbapi/api-docs (copied to .\src\ExchangeRateProvider\BankApiClients\Cnb\spec.json for easy reference)
public sealed class CnbBankApiClient(HttpClient httpClient) : IBankApiClient
{
    public async Task<IEnumerable<BankCurrencyRate>> GetDailyExchangeRatesAsync(DateTimeOffset? validFor = null, CancellationToken cancellationToken = default)
    {
        var apiRatesResponse = await GetRatesFromBankApiAsync(validFor, cancellationToken);

        return TransformToCurrencyRates(apiRatesResponse);
    }

    private async Task<IEnumerable<CnbBankCurrencyRate>> GetRatesFromBankApiAsync(DateTimeOffset? validFor = null, CancellationToken cancellationToken = default)
    {
        var requestUri = "cnbapi/exrates/daily";

        if (validFor.HasValue)
        {
            requestUri += "?date=" + validFor.Value.ToString("yyyy-MM-dd");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        var bankResponse = await JsonSerializer.DeserializeAsync<CnbBankResponse>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            },
            cancellationToken).ConfigureAwait(false);

        if (bankResponse?.Rates is null) return [];

        return bankResponse.Rates;
    }

    private static IEnumerable<BankCurrencyRate> TransformToCurrencyRates(IEnumerable<CnbBankCurrencyRate> apiRateResponse)
    {
        return apiRateResponse
            .Where(r =>
                r is { Amount: not null, CurrencyCode: not null, Rate: not null })
            .Select(r =>
                new BankCurrencyRate(
                    r.Amount!.Value,
                    r.CurrencyCode!,
                    r.Rate!.Value
                ));
    }

}