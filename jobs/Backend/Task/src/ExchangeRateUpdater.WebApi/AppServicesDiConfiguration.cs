using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Queries;
using ExchangeRateUpdater.Infrastructure.ExternalServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace ExchangeRateUpdater.WebApi
{
    public static class AppServicesDiConfiguration
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<GetExchangeRatesQuery>();

            services.AddHttpClient("ExchangeRatesApiClient", c => c.BaseAddress = new Uri(appSettings.ExchangeRateProviderUrl));

            services.AddTransient<IExchangeRateProvider>(ctx =>
            {
                var clientFactory = ctx.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient("ExchangeRatesApiClient");

                return new CnbExchangeRateProvider(httpClient, appSettings.SourceCurrency);
            });

            return services;
        }

    }
}
