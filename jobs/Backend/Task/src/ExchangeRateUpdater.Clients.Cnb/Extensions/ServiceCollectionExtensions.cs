using ExchangeRateUpdater.Clients.Cnb.Options;
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
    /// Adds Cnb client(see <see cref="ICnbClient" />)
    /// </summary>
    /// <param name="services">The service container.</param>
    /// <param name="options">The options delegate to configure Cnb client.</param>
    /// <returns>The service container to continue chaining.</returns>
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

        return services
            .AddHttpClient<ICnbClient, CnbClient>()
            .ConfigureHttpClient((s, c) =>
            {
                c.BaseAddress = s.GetRequiredService<IOptions<CnbClientOptions>>().Value.BaseUrl;
            })
            .AddPolicyHandler((s, _) =>
                GetAsyncPolicy(s.GetRequiredService<IOptions<CnbClientOptions>>().Value.RetryOptions));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetAsyncPolicy(RetryOptions retryOptions)
    {
        var delay = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: TimeSpan.FromSeconds(retryOptions.DelayInSeconds), retryCount: retryOptions.Count);

        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>().WaitAndRetryAsync(delay);
    }
}