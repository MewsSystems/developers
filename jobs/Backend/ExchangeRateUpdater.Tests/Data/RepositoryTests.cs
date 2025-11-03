using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExchangeRateUpdater.Tests.Data;

public class RepositoryTests : IDisposable
{
    private readonly ExchangeRateDbContext _context;
    private readonly Repository<ExchangeRateEntity> _repository;
    private readonly ExchangeRateEntity _validExchangeRate;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ExchangeRateDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mockConfig = new Mock<IAppConfiguration>();
        mockConfig.Setup(c => c.DatabaseConnectionString).Returns("InMemory");

        _context = new ExchangeRateDbContext(options, mockConfig.Object);
        _repository = new Repository<ExchangeRateEntity>(_context);

        _validExchangeRate = new ExchangeRateEntity
        {
            SourceCurrency = "USD",
            TargetCurrency = "CZK",
            Rate = 22.5m,
            Date = DateOnly.MinValue
        };
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public void Constructor_WithValidDbContext_CreatesInstance()
    {
        // Arrange & Act
        var repository = new Repository<ExchangeRateEntity>(_context);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task AddExchangeRateAsync_WithValidEntity_AddsToContext()
    {
        // Arrange
        var initialCount = await _context.ExchangeRates.CountAsync();

        // Act
        await _repository.AddExchangeRateAsync(_validExchangeRate);
        await _context.SaveChangesAsync();

        // Assert
        var finalCount = await _context.ExchangeRates.CountAsync();
        Assert.Equal(initialCount + 1, finalCount);
    }

    [Fact]
    public async Task AddExchangeRateAsync_WithValidEntity_EntityIsTracked()
    {
        // Arrange & Act
        await _repository.AddExchangeRateAsync(_validExchangeRate);

        // Assert
        var entry = _context.Entry(_validExchangeRate);
        Assert.Equal(EntityState.Added, entry.State);
    }

    [Fact]
    public async Task AddExchangeRateAsync_WithNullEntity_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _repository.AddExchangeRateAsync(null));
    }

    [Fact]
    public async Task AddExchangeRateAsync_WithMultipleEntities_AddsAllToContext()
    {
        // Arrange
        var rate1 = new ExchangeRateEntity
        {
            SourceCurrency = "EUR",
            TargetCurrency = "CZK",
            Rate = 25.0m,
            Date = DateOnly.MinValue
        };

        var rate2 = new ExchangeRateEntity
        {
            SourceCurrency = "GBP",
            TargetCurrency = "CZK",
            Rate = 30.0m,
            Date = DateOnly.MinValue
        };

        // Act
        await _repository.AddExchangeRateAsync(rate1);
        await _repository.AddExchangeRateAsync(rate2);
        await _repository.SaveChangesAsync();

        // Assert
        var count = await _context.ExchangeRates.CountAsync();
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task SaveChangesAsync_AfterAddingEntity_PersistsToDatabase()
    {
        // Arrange
        await _repository.AddExchangeRateAsync(_validExchangeRate);

        // Act
        await _repository.SaveChangesAsync();

        // Assert
        var savedEntity = await _context.ExchangeRates
            .FirstOrDefaultAsync(e => e.SourceCurrency == _validExchangeRate.SourceCurrency && e.TargetCurrency == _validExchangeRate.TargetCurrency);

        Assert.NotNull(savedEntity);
        Assert.Equal(_validExchangeRate.Rate, savedEntity.Rate);
    }

    [Fact]
    public async Task SaveChangesAsync_WithoutAddingEntity_DoesNotThrow()
    {
        // Arrange & Act
        var exception = await Record.ExceptionAsync(() => _repository.SaveChangesAsync());

        // Assert
        Assert.Null(exception);
    }
}