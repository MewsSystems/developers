using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CnbApiOptions>(configuration.GetSection(CnbApiOptions.SectionName));
        services.AddScoped<ICnbResponseParser, CnbResponseParser>();

        services.AddHttpClient<ICnbExchangeRateService, CnbExchangeRateService>()
            .AddPolicyHandler((serviceProvider, _) => GetRetryPolicy(serviceProvider))
            .AddPolicyHandler((serviceProvider, _) => GetCircuitBreakerPolicy(serviceProvider));

        services.AddScoped<ExchangeRateProvider>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<CnbApiOptions>>().Value;

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(options.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<CnbApiOptions>>().Value;

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(options.CircuitBreakerFailureThreshold, TimeSpan.FromSeconds(options.CircuitBreakerDurationSeconds));
    }
}
