using CzechNationalBankClient;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExchangeRateUpdater.Extensions
{
    public static class ConfigureClientsExtensions
    {
        public static IServiceCollection ConfigureClients(this IServiceCollection services)
        {
            services.AddHttpClient<ICurrencyExchangeRateClient, CurrencyExchangeRateClient>(c =>
                {
                    c.BaseAddress = new Uri("https://api.cnb.cz/");
                });
            return services;
        }
    }
}
