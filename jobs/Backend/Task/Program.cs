using CsvHelper;
using ExchangeRateUpdater.CsvParser;
using ExchangeRateUpdater.DAL;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public static class Program
{
    public static void Main(string[] args)
    {
        // setup dependency injection
        var serviceProvider = new ServiceCollection()
            // configure logging
            .AddLogging(configure => configure.AddConsole())
            // configure our services
            .AddSingleton<IEndpointDal, EndpointDal>()
            .AddSingleton<IExchangeRateProvider, CnbExchangeRateProvider>()
            .AddSingleton<App>()
            .AddSingleton<ICsvFactory, CsvFactory>()
            .AddTransient<IHttpHandler, HttpHandler>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<ICnbCsvReader, CnbCsvReader>()
            .BuildServiceProvider();

        var app = serviceProvider.GetService<App>();
        app.Run();
    }
}