using ExchangeRateUpdater.Domain.ApiClients.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Domain.ApiClients;

public sealed class ExchangeRateApiClient(HttpClient httpClient, ILogger<ExchangeRateApiClient> logger)
    : IExchangeRateApiClient
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private const string ErrorMessage = "Request to ExchangeRate source failed.";
    
    public async Task<string> GetExchangeRatesXml(CancellationToken cancellationToken)
    {
        const string endpoint = "/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        var requestUri = new Uri(_httpClient.BaseAddress!, endpoint);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogError(
                "HTTP GET {Url} failed with status {StatusCode}: {Content}",
                requestUri,
                response.StatusCode,
                errorContent
            );

            throw new HttpRequestException(ErrorMessage);
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return content;
    }
}