using DataLayer.Entities;

namespace DataLayer.Repositories;

public interface IExchangeRateFetchLogRepository : IRepository<ExchangeRateFetchLog>
{
    Task<IEnumerable<ExchangeRateFetchLog>> GetRecentLogsAsync(int count = 50, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRateFetchLog>> GetLogsByProviderAsync(int providerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRateFetchLog>> GetFailedLogsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRateFetchLog>> GetLogsByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);
}
