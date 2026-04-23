using System.Net.Mime;
using ExchangeRateUpdater.Api.Infrastructure.Interfaces;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Api.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CnbApiOptions>(
            configuration.GetSection(CnbApiOptions.SectionName));

        services.AddMemoryCache();
        services.AddCnbExchangeRateProvider();

        return services;
    }

    private static IServiceCollection AddCnbExchangeRateProvider(this IServiceCollection services)
    {
        services
            .AddHttpClient<IExternalExchangeRateClient, CnbApiClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<CnbApiOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            })
            .AddStandardResilienceHandler();

        return services;
    }
}
