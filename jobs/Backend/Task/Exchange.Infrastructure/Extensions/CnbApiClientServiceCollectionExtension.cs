using Exchange.Application.Abstractions.ApiClients;
using Exchange.Infrastructure.ApiClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Exchange.Infrastructure.Extensions;

public static class CnbApiClientServiceCollectionExtension
{
    public static IServiceCollection AddCnbApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<CnbApiOptions>()
            .Bind(configuration.GetSection(CnbApiOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient<ICnbApiClient, CnbApiClient>(ConfigureCnbApiClient())
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static Action<IServiceProvider, HttpClient> ConfigureCnbApiClient()
    {
        return (provider, client) =>
        {
            var cnbApiOptions = provider.GetRequiredService<IOptions<CnbApiOptions>>().Value;
            client.BaseAddress = new Uri(cnbApiOptions.BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(cnbApiOptions.TimeoutInSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        };
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );
}