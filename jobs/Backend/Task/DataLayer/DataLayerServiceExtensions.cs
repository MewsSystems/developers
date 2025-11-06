using Dapper;
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
        // Register Dapper TypeHandlers
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Register Pooled DbContextFactory for use in both Singleton and Scoped services
        // This uses connection pooling and is safe for all lifetimes
        services.AddPooledDbContextFactory<ApplicationDbContext>(
            options => options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)),
            poolSize: 128); // Configure pool size

        // Register scoped DbContext using the factory
        // This allows repositories to receive ApplicationDbContext directly
        services.AddScoped(sp =>
        {
            var factory = sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            return factory.CreateDbContext();
        });

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
