using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ExchangeRateUpdater
{
    public static class ServicesInstaller
    {
        public static IHost InstallServices()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<CacheConfig>(context.Configuration.GetSection(nameof(CacheConfig)));
                    services.Configure<CnbApiConfig>(context.Configuration.GetSection(nameof(CnbApiConfig)));
                    services.Configure<PollyConfig>(context.Configuration.GetSection(nameof(PollyConfig)));

                    var cnbApiConfig = new CnbApiConfig();
                    context.Configuration.Bind(nameof(cnbApiConfig), cnbApiConfig);

                    services.AddHttpClient(cnbApiConfig.ClientName, client =>
                    {
                        client.BaseAddress = new Uri(cnbApiConfig.BaseUrl);
                        client.Timeout = TimeSpan.FromSeconds(cnbApiConfig.TimeOut);
                    });

                    services.AddSingleton<IHttpClientService, HttpClientService>();
                    services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
                })
                .Build();
        }
    }
}
