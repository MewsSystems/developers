using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class ExchangeRateFetchLogRepository : Repository<ExchangeRateFetchLog>, IExchangeRateFetchLogRepository
{
    public ExchangeRateFetchLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ExchangeRateFetchLog>> GetRecentLogsAsync(int count = 50, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Provider)
            .Include(l => l.RequestedByUser)
            .OrderByDescending(l => l.FetchStarted)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExchangeRateFetchLog>> GetLogsByProviderAsync(int providerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Provider)
            .Include(l => l.RequestedByUser)
            .Where(l => l.ProviderId == providerId)
            .OrderByDescending(l => l.FetchStarted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExchangeRateFetchLog>> GetFailedLogsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Provider)
            .Include(l => l.RequestedByUser)
            .Where(l => l.Status == "Failed")
            .OrderByDescending(l => l.FetchStarted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExchangeRateFetchLog>> GetLogsByDateRangeAsync(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Provider)
            .Where(l => l.FetchStarted >= startDate && l.FetchStarted <= endDate)
            .OrderByDescending(l => l.FetchStarted)
            .ToListAsync(cancellationToken);
    }
}
