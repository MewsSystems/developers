using Microsoft.EntityFrameworkCore;
using ExchangeRateFinder.Infrastructure.Models;


namespace ExchangeRateUpdater.Infrastructure.Repositories
{
    public interface IExchangeRateRepository
    {
        Task<List<ExchangeRate>> GetAllAsync();
        Task<ExchangeRate> GetAsync(string code, string sourceCurrency);
        Task UpdateAllAsync(string sourceCurrnecy, List<ExchangeRate> newData);
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

        public async Task<ExchangeRate> GetAsync(string code, string sourceCurrency)
        {
            return await _context.ExchangeRates.FirstOrDefaultAsync(e => e.Code == code && e.SourceCurrency == sourceCurrency);
        }

        public async Task UpdateAllAsync(string sourceCurrnecy, List<ExchangeRate> newData)
        {
            var existingData = await GetAllAsync();

            foreach(var newDataItem in newData)
            {
                var existingItem = await GetAsync(newDataItem.Code, sourceCurrnecy);
                if(existingItem != null)
                {
                    await UpdateAsync(existingItem, newDataItem);
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
                    await DeleteAsync(existingItem.Code, sourceCurrnecy);
                }
            }
        }


        private async Task AddAsync(ExchangeRate exchangeRate)
        {
            _context.ExchangeRates.Add(exchangeRate);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateAsync(ExchangeRate existingEntity, ExchangeRate updatedEntity)
        {
            // That should be the only updated properties
            existingEntity.Rate = updatedEntity.Rate;
            existingEntity.Amount = updatedEntity.Amount;

            await _context.SaveChangesAsync();
        }

        private async Task DeleteAsync(string code, string sourceCurrency)
        {
            var exchangeRate = await GetAsync(code, sourceCurrency);
            if (exchangeRate != null)
            {
                _context.ExchangeRates.Remove(exchangeRate);
                await _context.SaveChangesAsync();
            }
        }
    }
}
