using DataLayer.Repositories;

namespace DataLayer;

public interface IUnitOfWork : IDisposable
{
    // Repositories
    ICurrencyRepository Currencies { get; }
    IExchangeRateProviderRepository ExchangeRateProviders { get; }
    IExchangeRateRepository ExchangeRates { get; }
    IExchangeRateProviderConfigurationRepository ExchangeRateProviderConfigurations { get; }
    ISystemConfigurationRepository SystemConfigurations { get; }
    IUserRepository Users { get; }
    IExchangeRateFetchLogRepository ExchangeRateFetchLogs { get; }
    IErrorLogRepository ErrorLogs { get; }

    // Transaction management
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
