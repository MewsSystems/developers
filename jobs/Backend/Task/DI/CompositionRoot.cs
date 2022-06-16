namespace ExchangeRateUpdater.DI
{
    using ExchangeRateUpdater.Code.Application;
    using ExchangeRateUpdater.Code.Observability;
    using ExchangeRateUpdater.Data;
    using ExchangeRateUpdater.Domain;
    using Microsoft.Extensions.DependencyInjection;

    public class CompositionRoot
    {
        public static IServiceCollection ConfigureServices()
        {
            var collection = new ServiceCollection();

            collection.AddScoped<ILogger, ConsoleLoggier>();
            collection.AddScoped<IRetryPolicy<BankDetails>, ExchangeRateRetryPolicy>();
            collection.AddScoped<IExchangeRateData>(provider => 
                new ExchangeRateData(
                    Configuration.BankUrl, 
                    provider.GetService<ILogger>(), 
                    provider.GetService<IRetryPolicy<BankDetails>>()));

            collection.AddScoped(provider => 
                new ExchangeRateProvider(
                    provider.GetService<ILogger>(),
                    provider.GetService<IExchangeRateData>(), 
                    new Domain.Currency("CZY")));

            return collection;
        }
    }
}
