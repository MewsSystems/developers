using ExchangeRateUpdater.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Polly;

namespace ExchangeRateUpdater.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiClients(this IServiceCollection services, IConfiguration configuration, string settingsSectionName, string clientName)
        {
            var random = new Random();
            var apiClientSettings = new ApiClientSettings();
            configuration.GetSection(settingsSectionName).Bind(apiClientSettings);

            services.AddHttpClient(clientName,
                client => { client.BaseAddress = new Uri(apiClientSettings.BaseAddress); })
                .AddTransientHttpErrorPolicy(x =>
                x.WaitAndRetryAsync(apiClientSettings.RetrySettings.MaxRetries,
                times => TimeSpan.FromSeconds(Math.Pow(2, times)) + TimeSpan.FromMilliseconds(random.Next(0, 1000))));
        }
    }
}
