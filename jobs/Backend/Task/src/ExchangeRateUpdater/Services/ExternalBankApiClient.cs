namespace ExchangeRateUpdater.Services;

/// <summary>
///     An implementation of the external bank API client.
/// </summary>
public class ExternalBankApiClient : IExternalBankApiClient
{
    private readonly RestClient _restClient;

    public ExternalBankApiClient(RestClient restClient)
    {
        _restClient = restClient;
        // TODO set url from settings
    }

    /// <inheritdoc />
    public async Task<GetDailyExchangeRatesResponse> GetDailyExchangeRatesAsync(DateOnly? date = null,
        string? lang = null, CancellationToken cancellationToken = default)
    {
        const string uri = "exrates/daily";

        var restRequest = new RestRequest(uri);

        if (date is not null)
            restRequest.AddQueryParameter("date", date.Value);

        if (lang is not null)
            // TODO probably worth checking if lang is one of the enumeration [CZ, EN]
            restRequest.AddQueryParameter("lang", lang);

        var response = await _restClient.ExecuteAsync<GetDailyExchangeRatesResponse>(restRequest, cancellationToken);
        response.ThrowIfError();

        return response.Data!;
    }
}