using Microsoft.EntityFrameworkCore;
using ExchangeRateFinder.Infrastructure.Models;


namespace ExchangeRateUpdater.Infrastructure.Repositories
{
    public interface IExchangeRateRepository
    {
        Task<List<ExchangeRate>> GetAllAsync();
        Task<ExchangeRate> GetByCodeAsync(string code);
        Task UpdateAllAsync(List<ExchangeRate> newData);
    }

    public class ExchangeRateRepository : IExchangeRateRepository

    {
        private readonly ExchangeRateDbContext _context;

        public ExchangeRateRepository(ExchangeRateDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExchangeRate>> GetAllAsync()
        {
            return await _context.ExchangeRates.ToListAsync();
        }

        public async Task<ExchangeRate> GetByCodeAsync(string code)
        {
            return await _context.ExchangeRates.FirstOrDefaultAsync(e => e.Code == code);
        }

        public async Task UpdateAllAsync(List<ExchangeRate> newData)
        {
            var existingData = await GetAllAsync();

            foreach(var newDataItem in newData)
            {
                var existingItem = await GetByCodeAsync(newDataItem.Code);
                if(existingItem != null)
                {
                    await UpdateAsync(newDataItem);
                }
                else
                {
                    await AddAsync(newDataItem);
                }
            }

            foreach (var existingItem in existingData)
            {
                if (!newData.Any(n => n.Code == existingItem.Code))
                {
                    await DeleteAsync(existingItem.Code);
                }
            }
        }


        private async Task AddAsync(ExchangeRate exchangeRate)
        {
            _context.ExchangeRates.Add(exchangeRate);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateAsync(ExchangeRate exchangeRate)
        {
            _context.Entry(exchangeRate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private async Task DeleteAsync(string code)
        {
            var exchangeRate = await GetByCodeAsync(code);
            if (exchangeRate != null)
            {
                _context.ExchangeRates.Remove(exchangeRate);
                await _context.SaveChangesAsync();
            }
        }
    }
}
