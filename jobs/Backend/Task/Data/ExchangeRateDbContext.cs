using ExchangeRateUpdater.Config;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateUpdater.Data;

public class ExchangeRateDbContext : DbContext
{
    private readonly AppConfiguration _appConfiguration;

    public ExchangeRateDbContext(DbContextOptions<ExchangeRateDbContext> options, AppConfiguration appConfiguration)
        : base(options)
    {
        _appConfiguration = appConfiguration;
    }

    public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_appConfiguration.DatabaseConnectionString);
        base.OnConfiguring(optionsBuilder);
    }
}