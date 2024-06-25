using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public static class ServiceProviderBuilder
    {
        public static ServiceProvider Build()
        {
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();

            return new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<HttpClient>()
                .AddTransient<IHttpClient, HttpClientWrapper>()
                .AddTransient< IExchangeRateParser, CnbExchangeRateParser>()
                .AddTransient<IExchangeRateProvider, ExchangeRateProvider>()
                .AddLogging()
                .BuildServiceProvider();
        }
    }
}
