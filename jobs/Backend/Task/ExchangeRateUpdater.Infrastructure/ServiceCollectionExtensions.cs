using ExchangeRateUpdater.ApplicationServices.Interfaces;
using ExchangeRateUpdater.Infrastructure.Data.Repositories;
using ExchangeRateUpdater.Infrastructure.HttpClients;
using ExchangeRateUpdater.Infrastructure.HttpClients.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IExchangeRateRepository, CnbExchangeRateRepository>();

        services.Configure<ExchangeRatesSettings>(configuration.GetSection(ExchangeRatesSettings.SectionName));

        var cnbApi = configuration.GetSection(ExchangeRatesSettings.SectionName).GetValue<string>("CnbApi")!;

        services.AddHttpClient<ICnbApiClient, CnbApiClient>(client => client.BaseAddress = new Uri(cnbApi));

        return services;
    }
}