using ExchangeRates.Api.Infrastructure.Clients.Cnb;
using ExchangeRates.Api.Options;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net.Sockets;

namespace Microsoft.Extensions.DependencyInjection;

public static class HttpClientServiceCollectionExtesions
{
    private const int _defaultMaxAttempts = 5;
    private const int _defaultBaseRetryInMs = 1000;

    public static IServiceCollection AddExchangeRatesApiHttpClients(this IServiceCollection services)
    {
        return services
                .AddCnbHttpClient();
    }

    private static IServiceCollection AddCnbHttpClient(this IServiceCollection services)
    {
        services
            .AddHttpClient<ICnbHttpClient, CnbHttpClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<CnbHttpClientOptions>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(settings.DefaultTimeoutInMilliseconds);

            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddRequestTimingDelegatingHandler();

        return services;
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<SocketException>()
            .OrTransientHttpStatusCode()
            .WaitAndRetryAsync(_defaultMaxAttempts, attempt => TimeSpan.FromMilliseconds(_defaultBaseRetryInMs));
    }
}
