using ExchangeRateUpdater;
using ExchangeRateUpdater.Domain.Configurations;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;



var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
var host = new HostBuilder()
        .ConfigureServices((services) =>
        {

            var settings = new ExchangeRateProviderSettings();
            config.GetSection("ExchangeRateProviderSettings").Bind(settings);
            services.AddHttpClient<ExchangeRateService>("exchange",
                (serviceProvider, client) =>
                {
                    client.BaseAddress = new Uri($"{settings.UrlBaseAPI}/{settings.UrlExchangeRate}");
                })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new SocketsHttpHandler()
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15)
            };
        })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

            services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddTransient<IExchangeRateService, ExchangeRateService>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
        })
        .UseSerilog()
        .Build();
         
var exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();

for (int i = 0; i < 3; i++)
{
    var rates = await exchangeRateProvider.GetExchangeRatesAsync(TestingData.currencies);
    foreach (var rate in rates)
    {
        Console.WriteLine(rate.ToString());
    }
    Console.WriteLine("-----------------");

}



