using System.Text.Json;
using ExchangeRateProvider.BankApiClients.Cnb.Models;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.BankApiClients.Cnb;

public sealed class CnbBankApiClient(HttpClient httpClient) : IBankApiClient
{
    public async Task<IEnumerable<BankCurrencyRate>> GetDailyExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var apiRatesResponse = await GetRatesFromBankApiAsync(cancellationToken);

        return TransformToCurrencyRates(apiRatesResponse);
    }

    private async Task<IEnumerable<CnbBankCurrencyRate>> GetRatesFromBankApiAsync(CancellationToken cancellationToken)
    {
	    var request = new HttpRequestMessage(HttpMethod.Get, "cnbapi/exrates/daily");
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
			    new BankCurrencyRate
			    {
				    Amount = r.Amount!.Value,
				    CurrencyCode = r.CurrencyCode!,
				    Rate = r.Rate!.Value
			    });
    }

}