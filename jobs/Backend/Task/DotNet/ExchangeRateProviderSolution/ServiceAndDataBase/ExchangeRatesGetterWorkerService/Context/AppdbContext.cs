using ExchangeRatesGetterWorkerService.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesGetterWorkerService.Context
{
    public class AppDbContext : DbContext
    {
        public const string TABLE_NAME = "ExchangeRateData";
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ExchangeRateData> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRateData>().ToTable(TABLE_NAME);

        }
    }
}
