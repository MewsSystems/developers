using ApplicationLayer.Common.Interfaces;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.Interfaces.Queries;
using DomainLayer.Interfaces.Repositories;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Unit.ApplicationLayer;

/// <summary>
/// Base class for ApplicationLayer unit tests.
/// Provides common mocking setup for repositories and services.
/// </summary>
public abstract class TestBase
{
    // Mocked dependencies - Mock at DataLayer boundary
    protected Mock<IUnitOfWork> MockUnitOfWork { get; }
    protected Mock<ISystemViewQueries> MockViewQueries { get; }
    protected Mock<IDateTimeProvider> MockDateTimeProvider { get; }
    protected Mock<IPasswordHasher> MockPasswordHasher { get; }
    protected Mock<IBackgroundJobService> MockBackgroundJobService { get; }

    // Individual repository mocks (accessed via UnitOfWork)
    protected Mock<ICurrencyRepository> MockCurrencyRepository { get; }
    protected Mock<IExchangeRateRepository> MockExchangeRateRepository { get; }
    protected Mock<IExchangeRateProviderRepository> MockProviderRepository { get; }
    protected Mock<IUserRepository> MockUserRepository { get; }
    protected Mock<IExchangeRateFetchLogRepository> MockFetchLogRepository { get; }
    protected Mock<IErrorLogRepository> MockErrorLogRepository { get; }

    protected TestBase()
    {
        // Initialize mocks
        MockUnitOfWork = new Mock<IUnitOfWork>();
        MockViewQueries = new Mock<ISystemViewQueries>();
        MockDateTimeProvider = new Mock<IDateTimeProvider>();
        MockPasswordHasher = new Mock<IPasswordHasher>();
        MockBackgroundJobService = new Mock<IBackgroundJobService>();

        // Initialize repository mocks
        MockCurrencyRepository = new Mock<ICurrencyRepository>();
        MockExchangeRateRepository = new Mock<IExchangeRateRepository>();
        MockProviderRepository = new Mock<IExchangeRateProviderRepository>();
        MockUserRepository = new Mock<IUserRepository>();
        MockFetchLogRepository = new Mock<IExchangeRateFetchLogRepository>();
        MockErrorLogRepository = new Mock<IErrorLogRepository>();

        // Wire up repository mocks to UnitOfWork
        MockUnitOfWork.Setup(x => x.Currencies).Returns(MockCurrencyRepository.Object);
        MockUnitOfWork.Setup(x => x.ExchangeRates).Returns(MockExchangeRateRepository.Object);
        MockUnitOfWork.Setup(x => x.ExchangeRateProviders).Returns(MockProviderRepository.Object);
        MockUnitOfWork.Setup(x => x.Users).Returns(MockUserRepository.Object);
        MockUnitOfWork.Setup(x => x.FetchLogs).Returns(MockFetchLogRepository.Object);
        MockUnitOfWork.Setup(x => x.ErrorLogs).Returns(MockErrorLogRepository.Object);

        // Default DateTimeProvider setup
        MockDateTimeProvider.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2025, 11, 6, 12, 0, 0, TimeSpan.Zero));
        MockDateTimeProvider.Setup(x => x.Today).Returns(new DateOnly(2025, 11, 6));
    }

    /// <summary>
    /// Creates a mock logger for the specified type.
    /// </summary>
    protected Mock<ILogger<T>> CreateMockLogger<T>()
    {
        return new Mock<ILogger<T>>();
    }

    /// <summary>
    /// Verifies that SaveChangesAsync was called on the UnitOfWork.
    /// </summary>
    protected void VerifySaveChangesCalled(Times? times = null)
    {
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), times ?? Times.Once());
    }

    /// <summary>
    /// Verifies that SaveChangesAsync was NOT called on the UnitOfWork.
    /// </summary>
    protected void VerifySaveChangesNotCalled()
    {
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }
}
