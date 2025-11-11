using DataLayer;
using DataLayer.Dapper;
using DomainLayer.Interfaces.Repositories;

namespace InfrastructureLayer.Persistence.Adapters;

/// <summary>
/// Adapts DataLayer error log repository to DomainLayer interface.
/// This bridges the DataLayer and DomainLayer without creating coupling.
/// </summary>
public class ErrorLogRepositoryAdapter : IErrorLogRepository
{
    private readonly IUnitOfWork _dataLayerUnitOfWork;
    private readonly IStoredProcedureService _storedProcedureService;

    public ErrorLogRepositoryAdapter(
        IUnitOfWork dataLayerUnitOfWork,
        IStoredProcedureService storedProcedureService)
    {
        _dataLayerUnitOfWork = dataLayerUnitOfWork;
        _storedProcedureService = storedProcedureService;
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

    public async Task<long> LogErrorAsync(
        string severity,
        string source,
        string message,
        string? exception = null,
        string? stackTrace = null,
        CancellationToken cancellationToken = default)
    {
        return await _storedProcedureService.LogErrorAsync(
            severity,
            source,
            message,
            exception,
            stackTrace,
            cancellationToken);
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
