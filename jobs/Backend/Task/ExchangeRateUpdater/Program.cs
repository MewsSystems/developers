using ExchangeRateUpdater;
using ExchangeRateUpdater.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
Startup.Configure(app, builder.Environment);

// Run  console-style logic as background task
using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider.GetRequiredService<ExchangeRateUpdater.Domain.Services.Interfaces.IExchangeRateProvider>();

    _ = Task.Run(async () =>
    {
        var currencies = new[]
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

        try
        {
            var rates = await provider.GetExchangeRatesForCurrenciesAsync(currencies, CancellationToken.None);
            var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
            Console.WriteLine($"Successfully retrieved {exchangeRates.Length} exchange rates:");
            foreach (var rate in exchangeRates)
                Console.WriteLine($"{rate.SourceCurrency.Code}/{rate.TargetCurrency.Code} = {rate.Value}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
    });
}

// Run the web host (Swagger + API)
await app.RunAsync();