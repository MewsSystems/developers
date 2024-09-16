using ExchangeRateFinder.Infrastructure.Models;
using ExchangeRateFinder.Infrastructure.Services;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateFinder.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddTransient<IHttpClientService, HttpClientService>();
            
            services.AddSingleton<ICachingService<ExchangeRate>, CachingService<ExchangeRate>>();
            services.AddDbContext<ExchangeRateDbContext>(options =>
                    options.UseInMemoryDatabase("ExchangeRates"), ServiceLifetime.Singleton);
        }
    }
}
