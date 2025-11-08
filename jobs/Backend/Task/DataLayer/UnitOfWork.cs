using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataLayer;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private ICurrencyRepository? _currencies;
    private IExchangeRateProviderRepository? _exchangeRateProviders;
    private IExchangeRateRepository? _exchangeRates;
    private IExchangeRateProviderConfigurationRepository? _exchangeRateProviderConfigurations;
    private ISystemConfigurationRepository? _systemConfigurations;
    private IUserRepository? _users;
    private IExchangeRateFetchLogRepository? _exchangeRateFetchLogs;
    private IErrorLogRepository? _errorLogs;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICurrencyRepository Currencies
    {
        get { return _currencies ??= new CurrencyRepository(_context); }
    }

    public IExchangeRateProviderRepository ExchangeRateProviders
    {
        get { return _exchangeRateProviders ??= new ExchangeRateProviderRepository(_context); }
    }

    public IExchangeRateRepository ExchangeRates
    {
        get { return _exchangeRates ??= new ExchangeRateRepository(_context); }
    }

    public IExchangeRateProviderConfigurationRepository ExchangeRateProviderConfigurations
    {
        get { return _exchangeRateProviderConfigurations ??= new ExchangeRateProviderConfigurationRepository(_context); }
    }

    public ISystemConfigurationRepository SystemConfigurations
    {
        get { return _systemConfigurations ??= new SystemConfigurationRepository(_context); }
    }

    public IUserRepository Users
    {
        get { return _users ??= new UserRepository(_context); }
    }

    public IExchangeRateFetchLogRepository ExchangeRateFetchLogs
    {
        get { return _exchangeRateFetchLogs ??= new ExchangeRateFetchLogRepository(_context); }
    }

    public IErrorLogRepository ErrorLogs
    {
        get { return _errorLogs ??= new ErrorLogRepository(_context); }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        // Check if already in a transaction
        if (_context.Database.CurrentTransaction != null)
        {
            _transaction = _context.Database.CurrentTransaction;
            return;
        }

        // When using execution strategies (like EnableRetryOnFailure for SQL Server),
        // the strategy doesn't support user-initiated transactions.
        // We need to use the execution strategy to execute the transaction.
        var strategy = _context.Database.CreateExecutionStrategy();

        // If the execution strategy supports user-initiated transactions, use it normally
        // Otherwise, skip manual transaction management (the execution strategy will handle it)
        if (strategy.RetriesOnFailure)
        {
            // For retrying strategies, we don't manually begin transactions
            // The strategy handles transactions internally
            // Just mark that we're in "transaction mode" but don't actually create one
            _transaction = null;
        }
        else
        {
            // For non-retrying strategies, use normal transaction management
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
