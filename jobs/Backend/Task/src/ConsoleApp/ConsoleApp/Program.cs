using API.Models;
using ConsoleClient;

namespace ConsoleApp
{
    public static class Program
    {
        private static readonly IEnumerable<Currency> currencies =
        [
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        ];

        public static async Task Main()
        {
            try
            {
                var callLocalAPI = new APIClient(HttpClientFactory.Create());
                var rates = await callLocalAPI.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while retrieving exchange rates: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
