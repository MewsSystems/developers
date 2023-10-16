using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ExchangeRateUpdater
{
    public static class Program
    {
        public static List<CurrencyDto> currencies = new List<CurrencyDto>
        {
            new CurrencyDto("USD"),
            new CurrencyDto("EUR"),
            new CurrencyDto("CZK"),
            new CurrencyDto("jpy"),
            new CurrencyDto("KES"),
            new CurrencyDto("RUB"),
            new CurrencyDto("THB"),
            new CurrencyDto("TRY"),
            new CurrencyDto("XYZ")
        };

        private static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            var services = Ioc.Services.Register(serviceCollection);

            Console.WriteLine("------------------------------------------------------------------------------");

            Console.WriteLine($"Do you want to fetch the exchange rates for the currencies [\"{string.Join(", ", currencies.Select(c => c.Code))}\"] ?: yes/no");

            while (Console.ReadLine()?.ToLowerInvariant() == "yes")
            {
                Console.WriteLine($"If you want to fetch the exchange rates for some other currencies, please enter them as a comma seperated values Ex: GBP,INR");

                var extraCurrencies = Console.ReadLine()?.Split(',').ToList();
                if (extraCurrencies != null && extraCurrencies.Any()) currencies.AddRange(extraCurrencies.Select(ec => new CurrencyDto(ec.Trim())).ToList());
                currencies = currencies.DistinctBy(c => c.Code, StringComparer.OrdinalIgnoreCase).ToList();
                
                try
                {
                    var provider = new ExchangeRateProvider(services.GetService<IExchangeRateProviderService>()!);
                    var rates = await provider.GetExchangeRates(currencies);

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

                Console.WriteLine("------------------------------------------------------------------------------");

                Console.WriteLine($"Do you want to fetch the exchange rates for these currencies [\"{string.Join(", ", currencies.Select(c => c.Code))}\"] again ?: yes/no");
            }
        }
    }
}
