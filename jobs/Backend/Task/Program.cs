using ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;


var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHttpClient<IExchangeRateService, ExchangeRateService>(
            client => 
            {
                client.BaseAddress = new Uri("https://api.cnb.cz/cnbapi/");
            });
        services.AddLogging();
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .Build();
