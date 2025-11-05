using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<ExchangeRateProvider> ExchangeRateProviders { get; set; }
    public DbSet<ExchangeRate> ExchangeRates { get; set; }
    public DbSet<ExchangeRateProviderConfiguration> ExchangeRateProviderConfigurations { get; set; }
    public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ExchangeRateFetchLog> ExchangeRateFetchLogs { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
