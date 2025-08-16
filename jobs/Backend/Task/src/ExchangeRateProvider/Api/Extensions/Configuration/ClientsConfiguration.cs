using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Options.Clients;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Refit;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ClientsConfiguration
{
    public static IServiceCollection ConfigureClients(this IServiceCollection services)
    {
        services.AddRefitClient<ICzechNationalBankApiClient>(GetNewtonsoftJsonRefitSettings())
            .ConfigureHttpClient((sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptions<CzechNationalBankApiClientOptions>>().Value;

                httpClient.BaseAddress = new Uri(options.BaseUrl);
                httpClient.Timeout = TimeSpan.FromMilliseconds(options.TimeoutInMs!.Value);
            })
            .AddPolicyHandler((sp, _) =>
            {
                var options = sp.GetRequiredService<IOptions<CzechNationalBankApiClientOptions>>().Value;
                return GetRetryPolicy(options.MaxRetries!.Value);
            });

        return services;
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(int retries)
    {
        return Policy
            .Handle<TaskCanceledException>()
            .OrTransientHttpStatusCode()
            .WaitAndRetryAsync(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static RefitSettings GetNewtonsoftJsonRefitSettings() => new(
        new NewtonsoftJsonContentSerializer(
            new JsonSerializerSettings()));
}
