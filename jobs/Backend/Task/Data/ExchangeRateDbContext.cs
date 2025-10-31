using Microsoft.EntityFrameworkCore;

namespace ExchangeRateUpdater.Data;

public class ExchangeRateDbContext : DbContext
{
    public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("");
        base.OnConfiguring(optionsBuilder);
    }
}