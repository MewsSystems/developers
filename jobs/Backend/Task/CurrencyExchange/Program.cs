using CurrencyExchange;
using CurrencyExchange.Clients;
using CurrencyExchange.Model;
using CurrencyExchange.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<ICurrencyExchangeClient, CnbApiClient>(c => c.BaseAddress = new Uri(builder.Configuration["CNBEndpoint"]!));

builder.Services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();

// Clear all default providers and send logs to a file to keep the console clear
builder.Logging.ClearProviders();
builder.Logging.AddFile("application.log", append: false);

using IHost host = builder.Build();

var hostTask = host.RunAsync();

await ReadConsoleInputAsync(host.Services);

await hostTask;

static async Task ReadConsoleInputAsync(IServiceProvider services)
{
    var applicationLifetime = services.GetService<IHostApplicationLifetime>();
    while (!applicationLifetime!.ApplicationStopping.IsCancellationRequested)
    {
        try
        {
            var input = Console.ReadLine();
            if (input?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
            {
                break;
            }
    
            if (input?.StartsWith("rates", StringComparison.OrdinalIgnoreCase) == true)
            {
                // Handling user input like this is naive. It could be improved by using a third party library for user console commands 
                // that is designed to handle all quirks of user input.
                var ratesInput = input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    
                if (ratesInput.Length < 2)
                {
                    Console.WriteLine("Please input requested currency codes separated by a comma (Example: rates AUD,USD)");
                    continue;
                }
    
                var currencies = ratesInput[1].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(cc => new Currency(cc));
    
                if (!currencies.Any())
                {
                    Console.WriteLine("List of currency codes cannot be empty.");
                    continue;
                }
    
                var retrievedRates = await services.GetRequiredService<IExchangeRateProvider>().GetExchangeRates(currencies, applicationLifetime.ApplicationStopping);
                Console.WriteLine($"Rates for {retrievedRates.First().ValidFor.Date.ToShortDateString()}:");
                foreach (var rate in retrievedRates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred. See log for details.");
            services.GetRequiredService<ILogger<Program>>().LogError(ex, "An error occurred");
        }
    }
    services.GetRequiredService<IHostApplicationLifetime>().StopApplication();
}