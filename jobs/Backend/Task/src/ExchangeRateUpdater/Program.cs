using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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

var basePath = System.IO.Directory.GetCurrentDirectory();
var config = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
var host = new HostBuilder()
         .ConfigureServices((configuration, services) =>
         {
             var appsettings = config.GetSection("ExchangeRateProviderSettings");
             var settings = new ExchangeRateProviderSettings();
             appsettings.Bind(settings);
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
        .Build();
        var exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();
        IEnumerable<Currency> currencies = new[]
       {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
        var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);
        foreach(var rate in rates)
        {
            Console.WriteLine(rate.Rate);
        }


