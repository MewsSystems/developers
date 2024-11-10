using ExchangeRate.Domain.Extensions;
using ExchangeRate.Domain.Providers;
using ExchangeRate.Domain.Providers.CzechNationalBank;

namespace ExchangeRate.Api.Clients;

public sealed class CzechNationalBankClient(HttpClient httpClient, ILogger<CzechNationalBankClient> logger)
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

        LogRequestTrace(requestUri);

        if (!httpResponse.IsSuccessStatusCode)
            return Results.Problem(
                type: httpResponse.ReasonPhrase,
                detail: $"Failed with status code {httpResponse.StatusCode}",
                statusCode: (int)httpResponse.StatusCode
            );

        var providerResponse = await httpResponse.Content
            .ReadFromJsonAsync<CzechNationalBankProviderResponse>(cancellationToken);

        return TypedResults.Ok(providerResponse);
    }

    private void LogRequestTrace(Uri requestUri)
    {
        logger.LogTrace("Request to {ExchangeRateProvider} with query parameters '{QueryParameters}'",
            Name,
            requestUri.Query);
    }
}