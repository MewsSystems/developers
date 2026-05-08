using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace ExchangeRateUpdaterTests.StartupTests
{
    public class StartupForTest : Startup
    {
        public new IConfiguration Configuration { get; }

        public StartupForTest(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public new void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CzechBankSettings>(Configuration.GetSection("CzechBankSettings"));

            services.AddMemoryCache();
            services.AddSingleton<IExchangeRateParser>(sp =>
                new CzechNationalBankTextRateParser(5, sp.GetRequiredService<ILogger<CzechNationalBankTextRateParser>>()));

            services.AddHttpClient<DailyExchangeRateFetcher>()
                .ConfigureHttpClient((provider, client) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>().Value;
                    client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                })
                .AddPolicyHandler((provider, request) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>().Value;
                    return Polly.Extensions.Http.HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(settings.RetryCount,
                            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
                });

            services.AddHttpClient<OtherCurrencyExchangeRateFetcher>()
                .ConfigureHttpClient((provider, client) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>().Value;
                    client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                })
                .AddPolicyHandler((provider, request) =>
                {
                    var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>().Value;
                    return Polly.Extensions.Http.HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(settings.RetryCount,
                            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
                });

            services.AddSingleton<IExhangeRateFetcher, DailyExchangeRateFetcher>();
            services.AddSingleton<IExhangeRateFetcher, OtherCurrencyExchangeRateFetcher>();
            services.AddSingleton<IExchangeRateProviderService, ExchangeRateProviderService>();

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(Configuration.GetSection("Logging"));
                logging.AddConsole();
            });
        }
    }
}
