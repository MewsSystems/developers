using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Application;

public class ExchangeRateApp(IExchangeRateProvider provider, ILogger<ExchangeRateApp> logger)
{
    public async Task RunAsync(IEnumerable<Currency> currencies)
    {
        try
        {
            var rates = await provider.GetExchangeRatesAsync(currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");

            foreach (var rate in rates)
            {
                Console.WriteLine(rate);
            }

            logger.LogInformation(
                "Application completed successfully. Retrieved {Count} exchange rates",
                rates.Count()
            );
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Network error while retrieving exchange rates");
            Console.WriteLine("Unable to retrieve exchange rates due to network error. Please check your connection.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving exchange rates");
            Console.WriteLine("Unable to retrieve exchange rates at this time. Please try again later.");
        }
    }
}