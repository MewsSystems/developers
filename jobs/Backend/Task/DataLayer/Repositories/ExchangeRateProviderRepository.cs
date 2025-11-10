using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class ExchangeRateProviderRepository : Repository<ExchangeRateProvider>, IExchangeRateProviderRepository
{
    public ExchangeRateProviderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ExchangeRateProvider?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.BaseCurrency)
            .Include(p => p.Configurations)
            .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<ExchangeRateProvider>> GetActiveProvidersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.BaseCurrency)
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<ExchangeRateProvider?> GetWithConfigurationsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.BaseCurrency)
            .Include(p => p.Configurations)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task UpdateHealthStatusAsync(int providerId, bool success, CancellationToken cancellationToken = default)
    {
        var provider = await _dbSet.FindAsync(new object[] { providerId }, cancellationToken);
        if (provider == null) return;

        provider.Modified = DateTimeOffset.UtcNow;

        if (success)
        {
            provider.LastSuccessfulFetch = DateTimeOffset.UtcNow;
            provider.ConsecutiveFailures = 0;
        }
        else
        {
            provider.LastFailedFetch = DateTimeOffset.UtcNow;
            provider.ConsecutiveFailures++;
        }
    }
}
