using Microsoft.EntityFrameworkCore;
using ExchangeRateFinder.Infrastructure.Models;


namespace ExchangeRateUpdater.Infrastructure.Repositories
{
    public interface IExchangeRateRepository
    {
        Task<List<ExchangeRate>> GetAllAsync(CancellationToken cancellation = default);
        Task<ExchangeRate> GetAsync(string sourceCurrencyCode, string targetCurrencyCode,  CancellationToken cancellation = default);
        Task UpdateAllAsync(string sourceCurrencyCode, List<ExchangeRate> newData, CancellationToken cancellation = default);
    }

    public class ExchangeRateRepository : IExchangeRateRepository

    {
        private readonly ExchangeRateDbContext _context;

        public ExchangeRateRepository(ExchangeRateDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExchangeRate>> GetAllAsync(CancellationToken cancellation = default)
        {
            return await _context.ExchangeRates.ToListAsync(cancellation);
        }

        public async Task<ExchangeRate> GetAsync(string sourceCurrencyCode, string targetCurrencyCode, CancellationToken cancellation = default)
        {
            return await _context.ExchangeRates.FirstOrDefaultAsync(e => e.TargetCurrencyCode == targetCurrencyCode && e.SourceCurrencyCode == sourceCurrencyCode, cancellation);
        }

        public async Task UpdateAllAsync(string targetCurrencyCode, List<ExchangeRate> newData, CancellationToken cancellationToken = default)
        {
            var existingData = await GetAllAsync(cancellationToken);

            foreach(var newDataItem in newData)
            {
                var existingItem = await GetAsync(newDataItem.SourceCurrencyCode, targetCurrencyCode, cancellationToken);
                if(existingItem != null)
                {
                    await UpdateAsync(existingItem, newDataItem, cancellationToken);
                }
                else
                {
                    await AddAsync(newDataItem, cancellationToken);
                }
            }

            foreach (var existingItem in existingData)
            {
                if (!newData.Any(n => n.TargetCurrencyCode == existingItem.TargetCurrencyCode 
                && n.SourceCurrencyCode == existingItem.SourceCurrencyCode))
                {
                    await DeleteAsync(existingItem.SourceCurrencyCode, targetCurrencyCode, cancellationToken);
                }
            }
        }


        private async Task AddAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default)
        {
            _context.ExchangeRates.Add(exchangeRate);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdateAsync(ExchangeRate existingEntity, ExchangeRate updatedEntity, CancellationToken cancellationToken = default)
        {
            // That should be the only updated properties
            existingEntity.Value = updatedEntity.Value;
            existingEntity.Amount = updatedEntity.Amount;

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task DeleteAsync(string sourceCurrencyCode, string targetCurrencyCode, CancellationToken cancellationToken = default)
        {
            var exchangeRate = await GetAsync(sourceCurrencyCode, targetCurrencyCode, cancellationToken);
            if (exchangeRate != null)
            {
                _context.ExchangeRates.Remove(exchangeRate);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
