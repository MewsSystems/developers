using CommandLine;
using ExchangeRateUpdater.Application.GetExchangeRates;
using ExchangeRateUpdater.Application.GetExchangeRatesForDate;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater.Presentation.Cli;

public static class Program
{
    private static readonly CancellationTokenSource _cts = new();

    public static async Task Main(string[] args)
    {
        Console.CancelKeyPress += (_, _) => _cts.Cancel();

        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureServices((_, services) => ConfigureServices(services));
        using var host = builder.Build();

        var argsHandler = Parser.Default.ParseArguments<CommandLineOptions>(args);

        await argsHandler.WithParsedAsync(o => RunWithOptions(host, o));
        argsHandler.WithNotParsed(PrintErrors);
    }

    private static async Task RunWithOptions(IHost host, CommandLineOptions options)
    {
        var providedCurrencies = options.Currencies.Select(c => new Currency(c.Trim()));

        if (options.Date.HasValue)
        {
            var query = host.Services.GetRequiredService<GetExchangeRatesForDateQuery>();
            await GetRates(() => query.Execute(options.Date.Value, providedCurrencies, _cts.Token));
        }
        else
        {
            var query = host.Services.GetRequiredService<GetExchangeRatesQuery>();
            await GetRates(() => query.Execute(providedCurrencies, _cts.Token));
        }
    }

    private static async Task GetRates(Func<Task<IEnumerable<ExchangeRate>>> query)
    {
        try
        {
            var rates = (await query()).ToArray();

            Console.WriteLine($"Successfully retrieved {rates.Length} exchange rates:");
            ExchangeRatePrinter.Print(rates);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
    }

    private static void PrintErrors(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            if (error.Tag == ErrorType.MissingRequiredOptionError)
            {
                Console.WriteLine("The Currencies option is required to specify at least one currency");
            }
            else if(error.Tag != ErrorType.HelpRequestedError && error.Tag != ErrorType.VersionRequestedError)
            {
                Console.WriteLine(error);
            }
        }
    }


    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddExternalApiInfrastructure(configuration);
        services.AddScoped<GetExchangeRatesQuery>();
        services.AddScoped<GetExchangeRatesForDateQuery>();
        services.AddSingleton(TimeProvider.System);
    }
}