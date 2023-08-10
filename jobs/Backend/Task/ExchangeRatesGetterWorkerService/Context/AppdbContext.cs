using ExchangeRatesGetterWorkerService.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesGetterWorkerService.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ExchangeRateData> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRateData>().ToTable("ExchangeRateData");

        }
    }
}
