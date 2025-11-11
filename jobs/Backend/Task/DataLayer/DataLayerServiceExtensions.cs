using Dapper;
using DataLayer.Dapper;
using DataLayer.Repositories;
using DataLayer.Seeding;
using Microsoft.Data.Sqlite;
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

        var useInMemoryDatabase = configuration.GetValue<bool>("Database:UseInMemoryDatabase");

        if (useInMemoryDatabase)
        {
            // Use SQLite in-memory database for demo/showcase purposes
            // SQLite supports SQL queries (so Dapper views will work)
            // Use shared cache to ensure all connections see the same database
            var connectionString = "DataSource=file:memdb1?mode=memory&cache=shared";
            var keepAliveConnection = new SqliteConnection(connectionString);
            keepAliveConnection.Open(); // Keep connection open to maintain in-memory database

            // Register the keep-alive connection as singleton to prevent database from being destroyed
            services.AddSingleton(keepAliveConnection);

            // Register Pooled DbContextFactory with SQLite
            services.AddPooledDbContextFactory<ApplicationDbContext>(
                options => options.UseSqlite(connectionString),
                poolSize: 128);

            // Register scoped DbContext using the factory
            services.AddScoped(sp =>
            {
                var factory = sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
                return factory.CreateDbContext();
            });
        }
        else
        {
            // Use SQL Server for production
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
        }

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

        // Register database seeder for in-memory database
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}
