using System;
using ExchangeRateUpdater.Domain.ApiClients;
using ExchangeRateUpdater.Domain.ApiClients.Interfaces;
using ExchangeRateUpdater.Domain.Services.Implementations;
using ExchangeRateUpdater.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Configuration;

public static class DependencyInjection
{
     public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureServices();
            services.ConfigureParsers();
            services.ConfigureHttpClients();
        }
     
        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IExchangeRateProvider, ExchangeRateUpdaterProvider>();
        }

        private static void ConfigureParsers(this IServiceCollection services)
        {
            services.AddSingleton<IExchangeRateParser, XmlExchangeRatesParser>();
        }
    
        private static void ConfigureHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IExchangeRateApiClient, ExchangeRateApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://www.cnb.cz/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });
        }
}