using ExchangeRateUpdater;
using ExchangeRateUpdater.Application.Components.Consumers;
using ExchangeRateUpdater.Application.Configurations;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;
using System;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<AppConfigurations>(configuration.GetSection("AppConfigurations"),
            options => options.BindNonPublicProperties = true);
        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
        services.AddScoped<IExchangeRateProviderService, CzechNationalBankExchangeRateProviderService>();
        services.AddHostedService<ConsoleApplication>();
        services.AddHttpClient("CzechNationalBankApi", cfg => { cfg.BaseAddress = new Uri(configuration["CzechNationalBankApi:BaseAddress"]); })
            .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3)));
        services.AddMediator(cfg => { cfg.AddConsumersFromNamespaceContaining<GetExchangeRatesQueryConsumer>(); });
    })
    .UseSerilog((hostingContext, logging) => { logging.WriteTo.Console().MinimumLevel.Error(); })
    .Build();

await host.RunAsync();