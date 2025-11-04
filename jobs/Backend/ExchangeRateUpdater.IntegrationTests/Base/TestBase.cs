using DotNet.Testcontainers.Builders;
using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Data;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace ExchangeRateUpdater.IntegrationTests.Base;

public class TestBase : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15.14-alpine3.22")
        .WithDatabase("exchange_rates_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(5432, true)
        .WithBindMount(Path.Combine(Directory.GetCurrentDirectory(), "Base/database-init.sql"),
            "/docker-entrypoint-initdb.d/database-init.sql")
        .WithEnvironment("POSTGRES_INITDB_ARGS", "--data-checksums")
        .WithEnvironment("TZ", "Europe/Madrid")
        .WithEnvironment("PGTZ", "Europe/Madrid")
        .WithCommand("-c", "password_encryption=scram-sha-256")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(5432))
        .Build();

    protected internal ExchangeRateDbContext DbContext = null!;
    protected internal string DbConnectionString => _postgresContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        var connectionString = _postgresContainer.GetConnectionString();
        var options = new DbContextOptionsBuilder<ExchangeRateDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        
        DbContext = new ExchangeRateDbContext(options,
            new AppConfiguration { DatabaseConnectionString = connectionString });
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }
}