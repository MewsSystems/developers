using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>(client => {
                        client.Timeout = TimeSpan.FromSeconds(60);
                    });
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>(s=>
                    new ExchangeRateProvider(
                        s.GetRequiredService<ILogger<ExchangeRateProvider>>(),
                        s.GetRequiredService<IHttpClientFactory>()
                    ));
                });     
    }
}
