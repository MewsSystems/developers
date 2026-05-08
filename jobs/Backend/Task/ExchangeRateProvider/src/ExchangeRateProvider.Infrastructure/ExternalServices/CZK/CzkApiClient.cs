using ExchangeRateProvider.Infrastructure.Policies;
using LazyCache;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using System.Text.Json;

namespace ExchangeRateProvider.Infrastructure.ExternalServices.CZK;

public class CzkApiClient(
    HttpClient httpClient,
    IReadOnlyPolicyRegistry<string> policyRegistry,
    IAppCache cache,
    ILogger<CzkApiClient> logger) : ICzkApiClient
{
    private const string ApiEndpoint = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";
    private const string CacheKey = "CzkExchangeRates";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    public async Task<CzkExchangeRateResponse?> GetExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        return await cache.GetOrAddAsync(
            CacheKey,
            async () => await FetchFromApiAsync(cancellationToken),
            CacheDuration);
    }

    private async Task<CzkExchangeRateResponse?> FetchFromApiAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching exchange rates from CNB API");

        var retryPolicy = policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.WaitAndRetry) 
            ?? Policy.NoOpAsync<HttpResponseMessage>();
        
        var context = new Context($"{nameof(GetExchangeRatesAsync)}-{Guid.NewGuid()}", new Dictionary<string, object>
        {
            { PolicyContextItems.Logger, logger }
        });

        var response = await retryPolicy.ExecuteAsync(
            ctx => httpClient.GetAsync(new Uri(ApiEndpoint, UriKind.Absolute), cancellationToken), 
            context);
        
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var result = await JsonSerializer.DeserializeAsync<CzkExchangeRateResponse>(
            stream, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, 
            cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Successfully fetched and cached exchange rates from CNB API");

        return result;
    }
}
