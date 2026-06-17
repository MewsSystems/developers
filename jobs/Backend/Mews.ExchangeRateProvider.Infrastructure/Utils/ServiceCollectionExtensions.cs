using Mews.ExchangeRateProvider.Infrastructure.Abstractions;
using Mews.ExchangeRateProvider.Infrastructure.Caching;
using Mews.ExchangeRateProvider.Infrastructure.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;

namespace Mews.ExchangeRateProvider.Infrastructure.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection InfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<CNBClientOptions>(options => configuration.GetSection("CNBClientOptions").Bind(options));
            var cnbSettings = services.BuildServiceProvider().GetRequiredService<IOptions<CNBClientOptions>>().Value;
            services.AddHttpClient("CNBClient",httpClient =>
            {
                httpClient.BaseAddress = new Uri(cnbSettings.CnbDailyRatesUrl);
                httpClient.Timeout = new TimeSpan(0, 0, 20);
            })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5)
                }));
            services.AddMemoryCache();
            services.AddScoped<ICNBCacheProvider, CNBCacheProvider>();
            services.AddScoped<ICNBClient, CNBClient>();
            return services;
        }
    }
}
