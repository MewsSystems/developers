using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Mews.ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Extensions for <see cref="IHttpClientBuilder"/>.
/// </summary>
public static class HttpClientBuilderExtensions
{
    private const int RetryCountOnCallError = 5;

    /// <summary>
    /// Configures the <see cref="IHttpClientBuilder"/> to add a retry policy to the <see cref="HttpClient"/>. 
    /// This retry policy will retry the operation for a specified number of times with an exponential backoff in case of transient HTTP errors.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="IHttpClientBuilder"/>.</returns>
    public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder builder)
    {
        IAsyncPolicy<HttpResponseMessage> policyHandler = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(RetryCountOnCallError, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        _ = builder.AddPolicyHandler(policyHandler);
        return builder;
    }
}
