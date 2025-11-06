using DataLayer;
using DomainLayer.Interfaces.Repositories;

namespace InfrastructureLayer.Persistence.Adapters;

/// <summary>
/// Adapts DataLayer error log repository to DomainLayer interface.
/// This bridges the DataLayer and DomainLayer without creating coupling.
/// </summary>
public class ErrorLogRepositoryAdapter : IErrorLogRepository
{
    private readonly IUnitOfWork _dataLayerUnitOfWork;

    public ErrorLogRepositoryAdapter(IUnitOfWork dataLayerUnitOfWork)
    {
        _dataLayerUnitOfWork = dataLayerUnitOfWork;
    }

    public async Task<IEnumerable<ErrorLogEntry>> GetRecentErrorsAsync(
        int count = 100,
        CancellationToken cancellationToken = default)
    {
        var errors = await _dataLayerUnitOfWork.ErrorLogs.GetRecentErrorsAsync(count, cancellationToken);
        return errors.Select(MapToErrorLogEntry);
    }

    public async Task<IEnumerable<ErrorLogEntry>> GetErrorsBySeverityAsync(
        string severity,
        CancellationToken cancellationToken = default)
    {
        var errors = await _dataLayerUnitOfWork.ErrorLogs.GetErrorsBySeverityAsync(severity, cancellationToken);
        return errors.Select(MapToErrorLogEntry);
    }

    public async Task<IEnumerable<ErrorLogEntry>> GetErrorsByDateRangeAsync(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default)
    {
        var errors = await _dataLayerUnitOfWork.ErrorLogs.GetErrorsByDateRangeAsync(
            startDate,
            endDate,
            cancellationToken);
        return errors.Select(MapToErrorLogEntry);
    }

    private static ErrorLogEntry MapToErrorLogEntry(DataLayer.Entities.ErrorLog error)
    {
        return new ErrorLogEntry(
            error.Id,
            error.Timestamp,
            error.Severity,
            error.Source,
            error.Message,
            error.Exception,
            error.StackTrace);
    }
}
