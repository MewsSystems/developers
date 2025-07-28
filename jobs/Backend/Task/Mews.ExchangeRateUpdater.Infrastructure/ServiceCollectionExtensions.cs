using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Mews.ExchangeRateUpdater.Infrastructure.Interfaces;
using Mews.ExchangeRateUpdater.Infrastructure.Persistance;
using Mews.ExchangeRateUpdater.Infrastructure.Persistance.Repositories;
using Mews.ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ExchangeRateUpdater.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString, string cnbBaseUrl)
    {
        services.AddDbContext<AppDbContext>(o => o.UseSqlite(connectionString));
        services.AddScoped<IExchangeRateRepository, EfExchangeRateRepository>();
        services.AddScoped<ICnbParser, CnbParser>();

        services.AddHttpClient<ICnbService, CnbService>(client =>
        {
            client.BaseAddress = new Uri(cnbBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}