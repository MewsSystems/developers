using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {

                var provider = new ExchangeRateProvider();
                var rates = await provider.GetExchangeRates();

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates of commonly traded currencies:");

                foreach (var rate in rates)
                {
                    foreach (var value in rate)
                    {
                        Console.Write(value.ToString() + " ");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

    }
}
