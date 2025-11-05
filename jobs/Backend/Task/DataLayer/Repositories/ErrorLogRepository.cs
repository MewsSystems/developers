using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class ErrorLogRepository : Repository<ErrorLog>, IErrorLogRepository
{
    public ErrorLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ErrorLog>> GetRecentErrorsAsync(int count = 100, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.User)
            .OrderByDescending(e => e.Timestamp)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ErrorLog>> GetErrorsBySeverityAsync(string severity, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.User)
            .Where(e => e.Severity == severity)
            .OrderByDescending(e => e.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ErrorLog>> GetErrorsByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.User)
            .Where(e => e.Timestamp >= startDate && e.Timestamp <= endDate)
            .OrderByDescending(e => e.Timestamp)
            .ToListAsync(cancellationToken);
    }
}
