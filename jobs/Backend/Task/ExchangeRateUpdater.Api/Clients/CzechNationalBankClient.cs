using ExchangeRate.Domain.Extensions;
using ExchangeRate.Domain.Providers;
using ExchangeRate.Domain.Providers.CzechNationalBank;

namespace ExchangeRate.Api.Clients;

public sealed class CzechNationalBankClient(HttpClient httpClient)
    : IExchangeRateClient
{
    public string Name => "Czech National Bank";
    public Uri BaseAddress => new("https://api.cnb.cz/cnbapi/exrates/daily");

    public async Task<IResult> GetExchangeRatesAsync(
        IExchangeRateProviderRequest request,
        CancellationToken cancellationToken)
    {
        var requestUri = request.ConstructRequestUri(BaseAddress);
        var httpResponse = await httpClient.GetAsync(requestUri, cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            var providerResponse = await httpResponse.Content
                .ReadFromJsonAsync<CzechNationalBankProviderResponse>(cancellationToken);

            return TypedResults.Ok(providerResponse);
        }

        return Results.Problem(
            type: httpResponse.ReasonPhrase,
            detail: $"Failed with status code {httpResponse.StatusCode}",
            statusCode: (int)httpResponse.StatusCode
        );
    }
}