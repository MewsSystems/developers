namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> _currencies = new[]
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
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var provider = serviceProvider.GetService<ExchangeRateProvider>();

            try
            {
                var rates = await provider.GetExchangeRates(_currencies);

                var ratesList = rates.ToList();
                Console.WriteLine($"Successfully retrieved {ratesList.Count} exchange rates:");
                foreach (var rate in ratesList)
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
        
        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddTransient<ExchangeRateProvider>();
        }
    }
}