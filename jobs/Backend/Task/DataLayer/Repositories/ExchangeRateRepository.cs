using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class ExchangeRateRepository : Repository<ExchangeRate>, IExchangeRateRepository
{
    public ExchangeRateRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ExchangeRate>> GetCurrentRatesAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await _dbSet
            .Include(r => r.Provider)
            .Include(r => r.BaseCurrency)
            .Include(r => r.TargetCurrency)
            .Where(r => r.ValidDate == today && r.Provider.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExchangeRate>> GetLatestRatesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Provider)
            .Include(r => r.BaseCurrency)
            .Include(r => r.TargetCurrency)
            .Where(r => r.Provider.IsActive)
            .GroupBy(r => new { r.ProviderId, r.BaseCurrencyId, r.TargetCurrencyId })
            .Select(g => g.OrderByDescending(r => r.ValidDate).First())
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExchangeRate>> GetRatesByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Provider)
            .Include(r => r.BaseCurrency)
            .Include(r => r.TargetCurrency)
            .Where(r => r.ValidDate >= startDate && r.ValidDate <= endDate)
            .OrderBy(r => r.ValidDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExchangeRate>> GetRatesByCurrencyPairAsync(int baseCurrencyId, int targetCurrencyId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Provider)
            .Include(r => r.BaseCurrency)
            .Include(r => r.TargetCurrency)
            .Where(r => r.BaseCurrencyId == baseCurrencyId && r.TargetCurrencyId == targetCurrencyId)
            .OrderByDescending(r => r.ValidDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ExchangeRate?> GetRateAsync(int providerId, int baseCurrencyId, int targetCurrencyId, DateOnly validDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Provider)
            .Include(r => r.BaseCurrency)
            .Include(r => r.TargetCurrency)
            .FirstOrDefaultAsync(r =>
                r.ProviderId == providerId &&
                r.BaseCurrencyId == baseCurrencyId &&
                r.TargetCurrencyId == targetCurrencyId &&
                r.ValidDate == validDate, cancellationToken);
    }

    public async Task<IEnumerable<ExchangeRate>> GetRatesByProviderAndDateRangeAsync(int providerId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.ProviderId == providerId && r.ValidDate >= startDate && r.ValidDate <= endDate)
            .OrderBy(r => r.ValidDate)
            .ToListAsync(cancellationToken);
    }
}
