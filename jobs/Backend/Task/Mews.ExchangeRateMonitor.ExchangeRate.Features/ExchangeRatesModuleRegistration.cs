using FluentValidation;
using Mews.ExchangeRateMonitor.Common.API.Extensions;
using Mews.ExchangeRateMonitor.Common.Application.Extensions;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Features;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Options;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Shared.HttpClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mews.ExchangeRateMonitor.ExchangeRate.Features;

public static class ExchangeRatesModuleRegistration
{
    public static IServiceCollection AddExchangeRatesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomAppOptions(configuration);
        services.AddHttpClients();
        services.AddCustomServices();
        services.AddExchangeRatesModuleApi();
        return services;
    }

    private static IServiceCollection AddCustomAppOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var msg = $"{nameof(ExchangeRateModuleOptions)} is not configured properly.";
        services.AddOptions<ExchangeRateModuleOptions>()
            .Bind(configuration.GetSection(nameof(ExchangeRateModuleOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Validate(x => !string.IsNullOrEmpty(x.CnbExratesOptions.BaseCnbApiUri), $"{msg} Base url shouldn't been empty or null");

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
        return services;
    }

    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientConsts.HttpCnbClient, (serviceProvider, httpClient) =>
        {
            var cnbExrateOptions = serviceProvider
                .GetRequiredService<IOptions<ExchangeRateModuleOptions>>().Value.CnbExratesOptions;

            var baseUri = cnbExrateOptions.BaseCnbApiUri.EndsWith("/")
                ? cnbExrateOptions.BaseCnbApiUri
                : cnbExrateOptions.BaseCnbApiUri + "/";

            httpClient.BaseAddress = new Uri(cnbExrateOptions.BaseCnbApiUri);
        })
            //recycle each individual TCP connection every 5 minutes
            .ConfigurePrimaryHttpMessageHandler(() =>
                new SocketsHttpHandler
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                })
            //keep the HttpClient handler alive forever, so its connection pool isn’t thrown away every 2 minutes
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        return services;
    }

    private static IServiceCollection AddExchangeRatesModuleApi(this IServiceCollection services)
    {
        services.RegisterApiEndpointsFromAssemblyContaining(typeof(ExchangeRatesModuleRegistration));
        services.RegisterHandlersFromAssemblyContaining(typeof(ExchangeRatesModuleRegistration));
        services.AddValidatorsFromAssembly(typeof(ExchangeRatesModuleRegistration).Assembly, includeInternalTypes: true);

        return services;
    }
}
