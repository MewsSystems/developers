using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class ExchangeRateProviderConfigurationRepository : Repository<ExchangeRateProviderConfiguration>, IExchangeRateProviderConfigurationRepository
{
    public ExchangeRateProviderConfigurationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ExchangeRateProviderConfiguration>> GetByProviderIdAsync(int providerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.ProviderId == providerId)
            .Include(c => c.Provider)
            .OrderBy(c => c.SettingKey)
            .ToListAsync(cancellationToken);
    }

    public async Task<ExchangeRateProviderConfiguration?> GetByProviderAndKeyAsync(int providerId, string settingKey, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Provider)
            .FirstOrDefaultAsync(c => c.ProviderId == providerId && c.SettingKey == settingKey, cancellationToken);
    }
}
