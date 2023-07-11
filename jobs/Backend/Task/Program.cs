﻿using ExchangeRateUpdater;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, builder) => builder.AddJsonFile("appsettings.json").Build())
    .ConfigureServices((context, services) =>
    {
        services.AddExchangeRateUpdaterServices(context.Configuration);
        services.AddScoped<App>();
    }).Build();

await host.Services.GetRequiredService<App>().Run(args);