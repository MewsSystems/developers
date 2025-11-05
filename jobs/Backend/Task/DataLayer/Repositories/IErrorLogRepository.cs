using DataLayer.Entities;

namespace DataLayer.Repositories;

public interface IErrorLogRepository : IRepository<ErrorLog>
{
    Task<IEnumerable<ErrorLog>> GetRecentErrorsAsync(int count = 100, CancellationToken cancellationToken = default);
    Task<IEnumerable<ErrorLog>> GetErrorsBySeverityAsync(string severity, CancellationToken cancellationToken = default);
    Task<IEnumerable<ErrorLog>> GetErrorsByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);
}
