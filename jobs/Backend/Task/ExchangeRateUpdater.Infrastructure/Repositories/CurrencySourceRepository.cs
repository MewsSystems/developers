using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Repositories
{
    public class CurrencySourceRepository : ICurrencySourceRepository
    {
        private readonly ApplicationDbContext _db;

        public CurrencySourceRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<CurrencySource>> GetCurrencySourcesAsync()
        {
            return await _db.CurrencySources.ToListAsync();
        }

        public async Task<CurrencySource?> GetCurrencySourceByCurrencyCodeAsync(string currencyCode)
        {
            return await _db.CurrencySources.FirstOrDefaultAsync(x => x.CurrencyCode == currencyCode);
        }


    }
}
