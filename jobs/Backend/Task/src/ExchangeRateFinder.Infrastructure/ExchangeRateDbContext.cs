using ExchangeRateFinder.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateUpdater.Infrastructure
{
    public class ExchangeRateDbContext : DbContext
    {
        public ExchangeRateDbContext(DbContextOptions<ExchangeRateDbContext> options) : base(options)
        {
        }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}