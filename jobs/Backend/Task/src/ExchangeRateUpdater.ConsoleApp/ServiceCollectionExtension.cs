using ExchangeRateUpdater.Application.Abstractions;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.ConsoleApp;
using ExchangeRateUpdater.ConsoleApp.Abstractions;
using ExchangeRateUpdater.ConsoleApp.Controllers;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureCustomServices(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            services.AddSingleton(configuration);

            services.AddTransient<IView, View>();
            services.AddTransient<ICommandReader, CommandReader>();
            services.AddTransient<IExchangeRateProviderService, ExchangeRateProviderService>();
            services.AddTransient<ExchangeRateProviderController>();
            services.AddExchangeRateUpdaterApplicationServices();
            services.AddExchangeRateUpdaterInfrastuctureServices();
        }
    }
}
