using ExchangeRateUpdater.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ExchangeRateUpdater.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureExchangeRateServices(this IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddConsole(options =>
            {
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
            }));
            services.AddTransient<CnbExchangeRateProvider>();
            services.AddHttpClient(nameof(CnbExchangeRateProvider));
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();
            services.AddSingleton(config);
        }
    }
}
