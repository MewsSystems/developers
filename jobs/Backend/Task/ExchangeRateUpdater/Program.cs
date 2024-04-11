using MewsFinance.Application.Interfaces;
using MewsFinance.Application.UseCases.ExchangeRates.Queries;
using MewsFinance.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> currencies = new[]
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

        public static async Task Main(string[] args)
        {
            try
            {
                var serviceCollection = new ServiceCollection();
                Startup.RegisterServices(serviceCollection);

                var serviceProvider = serviceCollection.BuildServiceProvider();

                var getExchangeRatesUseCase = serviceProvider.GetService<IGetExchangeRatesUseCase>();
                var request = CreateRequest(currencies);
                var rates = await getExchangeRatesUseCase.GetExchangeRates(request);

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
        }

        private static ExchangeRateRequest CreateRequest(IEnumerable<Currency> currencies)
        {
            return new ExchangeRateRequest
            {
                CurrencyCodes = currencies.Select(c => c.Code)
            };
        }
    }
}