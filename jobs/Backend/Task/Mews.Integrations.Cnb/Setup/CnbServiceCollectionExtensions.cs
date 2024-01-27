using System;
using Mews.Integrations.Cnb.Clients;
using Mews.Integrations.Cnb.Contracts.Configuration;
using Mews.Integrations.Cnb.Contracts.Services;
using Mews.Integrations.Cnb.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.Integrations.Cnb.Setup;

public static class CnbServiceCollectionExtensions
{
    /// <summary>
    /// Registers Cnb integration related service.
    /// </summary>
    public static IServiceCollection AddCnbIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICnbClient, CnbClient>(client =>
        {
            var config = configuration.Get<CnbConfiguration>();
            client.BaseAddress = new Uri(config!.BaseUrl);
        });
        services.Configure<CnbConfiguration>(configuration);
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        
        return services;
    }
}
