using ExchangeRateUpdater;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IExchangeRateProviderService, CzechNationalBankExchangeRateProviderService>();
        services.AddHostedService<ConsoleApplication>();
        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
    })
    .Build();

await host.RunAsync();