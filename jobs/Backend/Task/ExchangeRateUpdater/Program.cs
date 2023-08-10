using ExchangeRateUpdater;

// Todo here could be some DI container - Host.CreateDefaultBuilder(args)
// with configuration, logging, etc.

// Todo it could be completly separate project exposing only interface and service builder. 
// Something like public static IServiceCollection ConfigureExchangeRateUpdater(this IServiceCollection services)
// {
//     services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>(); //And others registration
// }
// Then others application would easily use it no matter if it is console app, web app, etc.
// Host.CreateDefaultBuilder(args)
//     .ConfigureServices(services => services.ConfigureExchangeRateUpdater())
// It would make sense because all interfaces we have to register are coupled to ExchangeRateProvider 
// If we want to replace Czech with different national bank, it would have to comply with same API contract according to some standard
// otherwise different input/output mappers would have to be implemented for each national bank.

try
{
    // Todo CzechExchangeRateProvider

    //Todo GetExchangeRatesAsync, but I dont want to change task interface
    var rates = await new ExchangeRateProvider(new CurrencyValidator(), new CzechExchangeRateFetcher())
        .GetExchangeRates(new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"), // Todo this is not part of czech national bank api. If we want to support it, we have to find another source.
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ"),
            new Currency("INR")
        });

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