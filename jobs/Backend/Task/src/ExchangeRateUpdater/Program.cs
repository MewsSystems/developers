using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using ExchangeRateUpdater.Domain.Configurations;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


    
      var host = new HostBuilder()
         .ConfigureServices((services) =>
         {
             

             services.AddHttpClient<ExchangeRateService>(
                 (serviceProvider, client) =>
                 {
                     var settings = serviceProvider.GetRequiredService<IOptions<ExchangeRateProviderSettings>>().Value;
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
        services.AddSingleton(TimeProvider.System);
         })
        .Build();
        var exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();


