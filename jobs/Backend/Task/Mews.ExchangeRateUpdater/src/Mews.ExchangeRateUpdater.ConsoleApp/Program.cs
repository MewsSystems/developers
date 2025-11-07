using Mews.ExchangeRateUpdater.Application;
using Mews.ExchangeRateUpdater.Application.ExchangeRates;
using Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;
using Mews.ExchangeRateUpdater.Infrastructure;

var builtConfig = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddEnvironmentVariables()
    .Build();

await Host.CreateDefaultBuilder()
    .ConfigureServices(ConfigureServices)
    .ConfigureServices(services => services.AddSingleton<Executor>())
    .Build()
    .Services
    .GetService<Executor>()!
    .Execute();

return;

void ConfigureServices(IServiceCollection services)
{
    _ = services
        .AddApplicationServices()
        .AddInfrastructureServices(builtConfig);
}

/// <summary>
/// Executor class for the console application.
/// </summary>
public class Executor
{
    private readonly IExchangeRateAppService _exchangeRateAppService;

    private static readonly List<Currency> Currencies = new()
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

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="exchangeRateAppService"></param>
    public Executor(IExchangeRateAppService exchangeRateAppService)
    {
        _exchangeRateAppService = exchangeRateAppService;
    }

    /// <summary>
    /// Main method for the executor that gets the today's exchange rates and prints them to the console.
    /// </summary>
    public async Task Execute()
    {
        try
        {
            var rates = (await _exchangeRateAppService.GetTodayExchangeRatesAsync(Currencies)).ToList();

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
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
    }
}
