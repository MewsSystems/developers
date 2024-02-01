using System.Collections.Generic;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Application.Components.Consumers;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<List<Currency>>(configuration.GetSection("Currencies"));
        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);

        services.AddExchangeRatesProvider(configuration);
        services.AddHostedService<ConsoleApplication>();
        services.AddMediator(cfg => { cfg.AddConsumersFromNamespaceContaining<GetExchangeRatesForCurrenciesQueryConsumer>(); });
    })
    .UseSerilog((_, logging) => { logging.WriteTo.File("log.txt").MinimumLevel.Error(); })
    .Build();

await host.RunAsync();