using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.json"));
builder.ConfigureServices((context, services) => {
    services.AddApplicationServices();
    services.AddInfrastructureServices(context.Configuration);
});

using IHost host = builder.Build();

RunProgram(host.Services);

host.Run();

static async void RunProgram(IServiceProvider hostProvider)
{
    try
    {
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

        var provider = hostProvider.GetRequiredService<IExchangeRateProvider>();
        var rates = await provider.GetExchangeRatesAsync(currencies, DateOnly.FromDateTime(DateTime.Now), Language.EN);

        Console.WriteLine($"\n\nSuccessfully retrieved {rates.Count()} exchange rates:");
        foreach (var rate in rates)
        {
            Console.WriteLine(rate.ToString());
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
    }

    Console.ReadLine();
    Environment.Exit(0);
}
