using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Configuration
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Bind configuration
            services.Configure<CzechBankSettings>(Configuration.GetSection("CzechBankSettings"));

            // Add dependencies
            services.AddMemoryCache();
            services.AddHttpClient<IExchangeRateProviderService, ExchangeRateProviderService>();

            // Add logging
            services.AddLogging(config =>
            {
                config.ClearProviders();
                config.AddConsole();
            });
        }
    }
}
