using DataLayer.Dapper;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.Interfaces.Repositories;
using InfrastructureLayer.Persistence.Adapters;

namespace InfrastructureLayer.Persistence;

/// <summary>
/// Implements DomainLayer IUnitOfWork by adapting DataLayer repositories.
/// This is the bridge between ApplicationLayer and DataLayer following clean architecture.
/// </summary>
public class DomainUnitOfWork : IUnitOfWork
{
    private readonly DataLayer.IUnitOfWork _dataLayerUnitOfWork;
    private readonly IStoredProcedureService _storedProcedureService;
    private ICurrencyRepository? _currencies;
    private IExchangeRateProviderRepository? _exchangeRateProviders;
    private IExchangeRateRepository? _exchangeRates;
    private IUserRepository? _users;
    private DomainLayer.Interfaces.Repositories.IExchangeRateFetchLogRepository? _fetchLogs;
    private DomainLayer.Interfaces.Repositories.IErrorLogRepository? _errorLogs;

    public DomainUnitOfWork(
        DataLayer.IUnitOfWork dataLayerUnitOfWork,
        IStoredProcedureService storedProcedureService)
    {
        _dataLayerUnitOfWork = dataLayerUnitOfWork;
        _storedProcedureService = storedProcedureService;
    }

    public ICurrencyRepository Currencies
    {
        get { return _currencies ??= new CurrencyRepositoryAdapter(_dataLayerUnitOfWork); }
    }

    public IExchangeRateProviderRepository ExchangeRateProviders
    {
        get { return _exchangeRateProviders ??= new ExchangeRateProviderRepositoryAdapter(_dataLayerUnitOfWork); }
    }

    public IExchangeRateRepository ExchangeRates
    {
        get { return _exchangeRates ??= new ExchangeRateRepositoryAdapter(_dataLayerUnitOfWork, _storedProcedureService); }
    }

    public IUserRepository Users
    {
        get { return _users ??= new UserRepositoryAdapter(_dataLayerUnitOfWork); }
    }

    public DomainLayer.Interfaces.Repositories.IExchangeRateFetchLogRepository FetchLogs
    {
        get { return _fetchLogs ??= new FetchLogRepositoryAdapter(_dataLayerUnitOfWork, _storedProcedureService); }
    }

    public DomainLayer.Interfaces.Repositories.IErrorLogRepository ErrorLogs
    {
        get { return _errorLogs ??= new ErrorLogRepositoryAdapter(_dataLayerUnitOfWork, _storedProcedureService); }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dataLayerUnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dataLayerUnitOfWork.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dataLayerUnitOfWork.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dataLayerUnitOfWork.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dataLayerUnitOfWork.Dispose();
    }
}
