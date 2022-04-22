using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExchangeRateUpdater.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IBankClient, CzechNationalBankClient>(client =>
            {
                client.BaseAddress = new Uri(config
                    .GetSection(CzechNationalBankOptions.SectionName)
                    .Get<CzechNationalBankOptions>()
                    .Url);
            });
            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

            return services;
        }
    }
}
