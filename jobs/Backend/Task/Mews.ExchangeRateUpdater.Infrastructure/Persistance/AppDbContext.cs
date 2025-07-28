using Mews.ExchangeRateUpdater.Infrastructure.Persistance.Models;
using Microsoft.EntityFrameworkCore;

namespace Mews.ExchangeRateUpdater.Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<ExchangeRateEntity> ExchangeRates => Set<ExchangeRateEntity>();
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ExchangeRateEntity>(entity =>
        {
            // Primary key is a composite of SourceCurrency, and TargetCurrency
            entity.HasKey(e => new { e.SourceCurrency, e.TargetCurrency });

            // Index for quick lookups by Date and SourceCurrency
            entity.HasIndex(e => new { e.Date, e.SourceCurrency });

            // Properties constraints
            entity.Property(e => e.Date)
                .IsRequired();
            
            entity.Property(e => e.SourceCurrency)
                .HasMaxLength(3)
                .IsRequired();

            entity.Property(e => e.TargetCurrency)
                .HasMaxLength(3)
                .IsRequired();

            // Precision for Value to handle currency values
            entity.Property(e => e.Value)
                .IsRequired()
                .HasPrecision(18, 6);
        });
    }
}
