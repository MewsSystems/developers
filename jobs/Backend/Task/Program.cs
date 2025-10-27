
using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddServices(builder.Configuration);

            var currencies = GetCurrencies(builder.Configuration);
            GetExchangeRate(currencies);
        }

        private static void GetExchangeRate(
            IEnumerable<Currency> currencies)
        {
            try
            {
                var provider = new ExchangeRateProvider();

                var rates = provider.GetExchangeRates(currencies);

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
        
        private static IEnumerable<Currency> GetCurrencies(IConfiguration configuration)
        {
            var section  = configuration
                .GetSection(Constants.ExchangeRateConfiguration)
                .Get<ExchangeRateConfiguration>();

            return section!.CurrencyCodes.Select(code => new Currency(code));
        }
    }
}
