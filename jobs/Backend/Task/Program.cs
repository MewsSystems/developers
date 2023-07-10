using ExchangeRateUpdater;
using ExchangeRateUpdater.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, builder) => builder.AddJsonFile("appsettings.json").Build())
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<App>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

    }).Build();

host.Services.GetRequiredService<App>().Run(args);