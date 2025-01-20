using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Domain.Configurations;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;

var basePath = System.IO.Directory.GetCurrentDirectory();
var config = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

var host = new HostBuilder()
        .ConfigureServices((services) =>
        {

            var settings = new ExchangeRateProviderSettings();
            config.GetSection("ExchangeRateProviderSettings").Bind(settings);
            services.AddHttpClient<ExchangeRateService>("exchange",
                (serviceProvider, client) =>
                {
                    client.BaseAddress = new Uri($"{settings.UrlBaseAPI}/{settings.UrlExchangeRate}");
                })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new SocketsHttpHandler()
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(15)
                };
        })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddTransient<IExchangeRateService, ExchangeRateService>();
        services.AddSingleton<IMemoryCache, MemoryCache>();
        })
        .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(config))
        .Build();
         
var exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();
        
var rates = await exchangeRateProvider.GetExchangeRatesAsync(TestingData.currencies);
foreach(var rate in rates)
{
    Console.WriteLine(rate.ToString());
}


