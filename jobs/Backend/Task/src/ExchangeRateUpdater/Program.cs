using System.Threading.Tasks;
using ExchangeRateUpdater.DependencyInjection;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.Configure<CurrencySettings>(context.Configuration.GetSection("CurrencySettings"));
                services.AddApiClients(context.Configuration, "CNBExchangeRatesApiSettings", "CNBExchangeRatesApi");
                services.AddTransient<IFxRateService, CNBFxRateService>();
                services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
                services.AddHostedService<AppService>();
            });
    }
}
