using System;
using ExchangeRateUpdater.Config;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateUpdater.Data;

/// <summary>
///     Database context for managing exchange rate data in PostgreSQL.
///     Configures the connection using settings from <see cref="IAppConfiguration" />.
/// </summary>
public class ExchangeRateDbContext : DbContext
{
    private readonly IAppConfiguration _appConfiguration;

    public ExchangeRateDbContext(DbContextOptions<ExchangeRateDbContext> options, IAppConfiguration appConfiguration)
        : base(options)
    {
        _appConfiguration = appConfiguration;
    }

    public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(_appConfiguration.DatabaseConnectionString))
            throw new InvalidOperationException("Database connection string is not configured.");

        optionsBuilder.UseNpgsql(_appConfiguration.DatabaseConnectionString);
        base.OnConfiguring(optionsBuilder);
    }
}