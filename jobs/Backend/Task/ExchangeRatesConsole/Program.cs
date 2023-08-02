using Application;
using Application.Providers;
using ExchangeRateUpdater;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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

var builder = new HostBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfrastructure()
            .AddApplication();

    }).UseConsoleLifetime();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.UseSerilog();

var host = builder.Build();

try
{
    var provider = host.Services.GetRequiredService<ExchangeRateProvider>();
    var rates = (await provider.GetExchangeRatesAsync(currencies)).Select(r => new ExchangeRate(
        new Currency("CZK"),
        new Currency(r.CurrencyCode),
        r.Rate / r.Amount));

    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
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