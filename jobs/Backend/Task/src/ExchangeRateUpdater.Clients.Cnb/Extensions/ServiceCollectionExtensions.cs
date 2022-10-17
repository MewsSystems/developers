using ExchangeRateUpdater.Clients.Cnb.Options;
using ExchangeRateUpdater.Clients.Cnb.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace ExchangeRateUpdater.Clients.Cnb.Extensions;

/// <summary>
/// Registration extensions for Cnb client.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Cnb client.
    /// </summary>
    /// <param name="services">The service container.</param>
    /// <param name="options">The options delegate to configure Cnb client.</param>
    public static IHttpClientBuilder AddCnbClient(this IServiceCollection services, Action<CnbClientOptions> options)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        services
            .AddOptions<CnbClientOptions>()
            .Configure(options)
            .Validate(opts => opts.BaseUrl != null, "Base URL missing.");

        services.AddTransient<ICnbClientResponseParser, CnbClientResponseParser>();

        return services
            .AddHttpClient<ICnbClient, CnbClient>()
            .ConfigureHttpClient((s, c) =>
            {
                c.BaseAddress = s.GetRequiredService<IOptions<CnbClientOptions>>().Value.BaseUrl;
            })
            .AddPolicyHandler((s, _) =>
                GetAsyncPolicy(s.GetRequiredService<IOptions<CnbClientOptions>>().Value.RetryOptions));
    }

    /// <summary>
    /// Gets retry policy for cbn client, works with jitter strategy
    /// </summary>
    /// <param name="retryOptions">The options delegate to configure retry.</param>
    private static IAsyncPolicy<HttpResponseMessage> GetAsyncPolicy(RetryOptions retryOptions)
    {
        var delay = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: TimeSpan.FromSeconds(retryOptions.DelayInSeconds),
            retryCount: retryOptions.Count);

        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>().WaitAndRetryAsync(delay);
    }
}