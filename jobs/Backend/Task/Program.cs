using System;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Application.Components.Consumers;
using ExchangeRateUpdater.Application.Configurations;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<AppConfigurations>(configuration.GetSection("AppConfigurations"));
        services.AddScoped<IExchangeRateProviderService, CzechNationalBankExchangeRateProviderService>();
        services.AddHttpClient("CzechNationalBankApi", cfg =>
        {
            cfg.BaseAddress = new Uri(configuration["CzechNationalBankApi:BaseAddress"]);
        });
        services.AddMediator(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<GetExchangeRatesQueryConsumer>();
        });
        services.AddHostedService<ConsoleApplication>();
        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
    })
    .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Error))
    .Build();

await host.RunAsync();