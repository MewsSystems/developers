using ExchangeRateUpdater.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public ApplicationDbContext() { }

        public virtual DbSet<CurrencySource> CurrencySources { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CurrencySource>().HasData(
                new CurrencySource() { Id = Guid.NewGuid(), CurrencyCode = "CZK", SourceUrl = "https://api.cnb.cz/cnbapi/exrates/daily" }
            );

        }
    }
}
