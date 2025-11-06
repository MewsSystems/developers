using ApplicationLayer;
using Dapper;
using DataLayer;
using DataLayer.Dapper;
using InfrastructureLayer;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respawn;

namespace Integration.Infrastructure;

/// <summary>
/// Base class for integration tests providing common functionality.
/// </summary>
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IServiceScope Scope;
    protected readonly IMediator Mediator;
    protected readonly DomainLayer.Interfaces.Persistence.IUnitOfWork UnitOfWork;
    protected readonly ApplicationDbContext DbContext;
    private static Respawner? _respawner;
    private static readonly object _lock = new();
    private const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ExchangeRateUpdaterTest;Integrated Security=true;Connection Timeout=30;MultipleActiveResultSets=true;";

    protected IntegrationTestBase()
    {
        // Register Dapper TypeHandlers before any database operations
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        // Build service collection manually
        var services = new ServiceCollection();

        // Register logging
        services.AddLogging(configure => configure.AddConsole());

        // Register DbContext WITHOUT retry logic (conflicts with manual transactions)
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ConnectionString));

        // Register DataLayer services
        services.AddScoped<DataLayer.IUnitOfWork, DataLayer.UnitOfWork>();
        services.AddScoped<DataLayer.Repositories.IUserRepository, DataLayer.Repositories.UserRepository>();
        services.AddScoped<DataLayer.Repositories.ICurrencyRepository, DataLayer.Repositories.CurrencyRepository>();
        services.AddScoped<DataLayer.Repositories.IExchangeRateProviderRepository, DataLayer.Repositories.ExchangeRateProviderRepository>();
        services.AddScoped<DataLayer.Repositories.IExchangeRateRepository, DataLayer.Repositories.ExchangeRateRepository>();

        // Register Dapper services
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = ConnectionString
            })
            .Build();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<DataLayer.Dapper.IDapperContext, DataLayer.Dapper.DapperContext>();
        services.AddScoped<DataLayer.Dapper.IStoredProcedureService, DataLayer.Dapper.StoredProcedureService>();
        services.AddScoped<DataLayer.Dapper.IViewQueryService, DataLayer.Dapper.ViewQueryService>();

        // Register InfrastructureLayer services
        services.AddScoped<DomainLayer.Interfaces.Persistence.IUnitOfWork, InfrastructureLayer.Persistence.DomainUnitOfWork>();
        services.AddScoped<DomainLayer.Interfaces.Repositories.IUserRepository, InfrastructureLayer.Persistence.Adapters.UserRepositoryAdapter>();
        services.AddScoped<DomainLayer.Interfaces.Repositories.ICurrencyRepository, InfrastructureLayer.Persistence.Adapters.CurrencyRepositoryAdapter>();
        services.AddScoped<DomainLayer.Interfaces.Repositories.IExchangeRateProviderRepository, InfrastructureLayer.Persistence.Adapters.ExchangeRateProviderRepositoryAdapter>();
        services.AddScoped<DomainLayer.Interfaces.Repositories.IExchangeRateRepository, InfrastructureLayer.Persistence.Adapters.ExchangeRateRepositoryAdapter>();
        services.AddScoped<DomainLayer.Interfaces.Services.IPasswordHasher, InfrastructureLayer.Services.BCryptPasswordHasher>();
        services.AddSingleton<DomainLayer.Interfaces.Services.IDateTimeProvider, InfrastructureLayer.ExternalServices.DateTimeProvider>();
        services.AddScoped<DomainLayer.Interfaces.Queries.ISystemViewQueries, InfrastructureLayer.Persistence.Adapters.ViewQueryRepositoryAdapter>();

        // Register Fake BackgroundJobService for testing (avoids Hangfire dependency)
        services.AddScoped<ApplicationLayer.Common.Interfaces.IBackgroundJobService, FakeBackgroundJobService>();

        // Register ApplicationLayer with MediatR
        services.AddApplicationLayer();

        // Build service provider
        ServiceProvider = services.BuildServiceProvider();
        Scope = ServiceProvider.CreateScope();

        // Get services
        Mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();
        UnitOfWork = Scope.ServiceProvider.GetRequiredService<DomainLayer.Interfaces.Persistence.IUnitOfWork>();
        DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    /// <summary>
    /// Called before each test. Ensures database is created and clean.
    /// </summary>
    public virtual async Task InitializeAsync()
    {
        // Ensure database is created
        await DbContext.Database.EnsureCreatedAsync();

        // Initialize Respawn once
        if (_respawner == null)
        {
            lock (_lock)
            {
                if (_respawner == null)
                {
                    using var connection = new SqlConnection(ConnectionString);
                    connection.Open();

                    _respawner = Respawner.CreateAsync(connection, new RespawnerOptions
                    {
                        TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
                        DbAdapter = DbAdapter.SqlServer
                    }).GetAwaiter().GetResult();
                }
            }
        }

        // Reset database to clean state
        using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();
        await _respawner!.ResetAsync(conn);
    }

    /// <summary>
    /// Called after each test. Cleans up resources.
    /// </summary>
    public virtual async Task DisposeAsync()
    {
        Scope.Dispose();
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
