using ExchangeRateUpdater.Configurations;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Startup>();
                    services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                            .AddSingleton<IExchangeRateSource, CzechNationalBank>()
                            .AddSingleton<ICzechNationalBankExchangeRateParser, CzechNationalBankExchangeRateParser>()
                            .AddSingleton<ICzechNationalBankConfig, CzechNationalBankConfig>()
                            .AddSingleton<IHttpClientLineReader, HttpClientLineReader>();
                });
        }

    }
}
