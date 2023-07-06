using System.Threading.Tasks;
using ExchangeRateTool.Extensions;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var sp = CreateServiceProvider();

            var mainService = sp.GetRequiredService<MainService>();

            await mainService.Run();
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection()
                .AddExchangeRateTool(configuration)
                .AddScoped<ICurrenciesProvider, CurrenciesProvider>()
                .AddScoped<IExchangeRatePrinter, ExchangeRatePrinter>()
                .AddScoped<MainService>()
                .AddSingleton<IConfiguration>(configuration);
           
            return serviceCollection.BuildServiceProvider();
        }
    }
}
