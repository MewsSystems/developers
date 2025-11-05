using DataLayer.Dapper;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer;

public static class DataLayerServiceExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)));

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Repositories
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IExchangeRateProviderRepository, ExchangeRateProviderRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddScoped<IExchangeRateProviderConfigurationRepository, ExchangeRateProviderConfigurationRepository>();
        services.AddScoped<ISystemConfigurationRepository, SystemConfigurationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IExchangeRateFetchLogRepository, ExchangeRateFetchLogRepository>();
        services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

        // Register Dapper services
        services.AddSingleton<IDapperContext, DapperContext>();
        services.AddScoped<IStoredProcedureService, StoredProcedureService>();
        services.AddScoped<IViewQueryService, ViewQueryService>();

        return services;
    }
}
