using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<CurrencyCzechRate> CurrencyCzechRate { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyCzechRate>()
            .HasKey(m => new { m.Code, m.CreatedDate });

        modelBuilder.Entity<CurrencyCzechRate>()
            .HasIndex(m => new { m.CreatedDate });
    }
}
