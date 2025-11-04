using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateUpdater.IntegrationTests.Integration.Repository;

[Trait("Category", "Integration")]
public class RepositoryIntegrationTests : IClassFixture<TestBase>, IAsyncLifetime
{
    private const string SourceCurrency = "USD";
    private const string TargetCurrency = "CZK";
    private const decimal ValidRate = 22.500m;
    
    private readonly TestBase _fixture;
    private readonly DateOnly _testDate = DateOnly.FromDateTime(DateTime.UtcNow);
    private Repository<ExchangeRateEntity> _repository = null!;

    public RepositoryIntegrationTests(TestBase fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync()
    {
        _repository = new Repository<ExchangeRateEntity>(_fixture.DbContext);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _fixture.DbContext.ExchangeRates.ExecuteDeleteAsync();
    }

    [Fact]
    public async Task AddAsync_WithValidEntity_InsertsIntoDatabase()
    {
        // Arrange
        var entity = new ExchangeRateEntity
        {
            SourceCurrency = "USD",
            TargetCurrency = "CZK",
            Rate = 22.500m,
            Date = DateOnly.FromDateTime(DateTime.Now)
        };

        // Act
        await _repository.AddExchangeRateAsync(entity);
        await _repository.SaveChangesAsync();

        // Assert
        var savedEntity = await _fixture.DbContext.ExchangeRates.FindAsync(entity.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal("USD", savedEntity.SourceCurrency);
        Assert.Equal("CZK", savedEntity.TargetCurrency);
        Assert.Equal(22.500m, savedEntity.Rate);
    }

    [Fact]
    public async Task AddAsync_WithNullEntity_ThrowsException()
    {
        // Arrange
        ExchangeRateEntity entity = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.AddExchangeRateAsync(entity));
    }

    [Fact]
    public async Task AddAsync_PreservesAllEntityProperties()
    {
        // Arrange
        var entity = new ExchangeRateEntity
        {
            SourceCurrency = "USD",
            TargetCurrency = "CZK",
            Rate = 22.567m,
            Date = DateOnly.Parse("2025-10-01")
        };

        // Act
        await _repository.AddExchangeRateAsync(entity);
        await _repository.SaveChangesAsync();

        // Assert
        var savedEntity = await _fixture.DbContext.ExchangeRates.FindAsync(entity.Id);
        Assert.Equal(entity.SourceCurrency, savedEntity.SourceCurrency);
        Assert.Equal(entity.TargetCurrency, savedEntity.TargetCurrency);
        Assert.Equal(entity.Rate, savedEntity.Rate);
        Assert.Equal(entity.Date, savedEntity.Date);
    }

    [Fact]
    public async Task AddAsync_WithMultipleEntities_InsertsAllIntoDatabase()
    {
        // Arrange
        var entity1 = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = "EUR",
            Rate = 0.92m,
            Date = _testDate
        };
        var entity2 = new ExchangeRateEntity
        {
            SourceCurrency = "GBP",
            TargetCurrency = SourceCurrency,
            Rate = 1.27m,
            Date = _testDate
        };
        var entity3 = new ExchangeRateEntity
        {
            SourceCurrency = "JPY",
            TargetCurrency = SourceCurrency,
            Rate = 0.0067m,
            Date = _testDate
        };

        // Act
        await _repository.AddExchangeRateAsync(entity1);
        await _repository.AddExchangeRateAsync(entity2);
        await _repository.AddExchangeRateAsync(entity3);
        await _repository.SaveChangesAsync();

        // Assert
        var savedEntity1 = await _fixture.DbContext.ExchangeRates.FindAsync(entity1.Id);
        var savedEntity2 = await _fixture.DbContext.ExchangeRates.FindAsync(entity2.Id);
        var savedEntity3 = await _fixture.DbContext.ExchangeRates.FindAsync(entity3.Id);

        Assert.NotNull(savedEntity1);
        Assert.NotNull(savedEntity2);
        Assert.NotNull(savedEntity3);
    }

    [Fact]
    public async Task AddAsync_WithoutSaveChanges_DoesNotPersistToDatabase()
    {
        // Arrange
        var entity = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = TargetCurrency,
            Rate = ValidRate,
            Date = _testDate
        };

        // Act
        await _repository.AddExchangeRateAsync(entity);

        // Assert
        var count = await _fixture.DbContext.ExchangeRates.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task AddAsync_WithDifferentRatesForSameCurrencies_InsertsAll()
    {
        // Arrange
        var entity1 = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = TargetCurrency,
            Rate = 22.500m,
            Date = _testDate
        };
        var entity2 = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = TargetCurrency,
            Rate = 22.800m,
            Date = _testDate.AddDays(1)
        };

        // Act
        await _repository.AddExchangeRateAsync(entity1);
        await _repository.AddExchangeRateAsync(entity2);
        await _repository.SaveChangesAsync();

        // Assert
        var rates = await _fixture.DbContext.ExchangeRates
            .Where(e => e.SourceCurrency == SourceCurrency && e.TargetCurrency == TargetCurrency)
            .ToListAsync();

        Assert.Equal(2, rates.Count);
        Assert.Contains(rates, r => r.Rate == 22.500m);
        Assert.Contains(rates, r => r.Rate == 22.800m);
    }

    [Fact]
    public async Task AddAsync_WithZeroRate_InsertsIntoDatabase()
    {
        // Arrange
        var entity = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = TargetCurrency,
            Rate = 0m,
            Date = _testDate
        };

        // Act
        await _repository.AddExchangeRateAsync(entity);
        await _repository.SaveChangesAsync();

        // Assert
        var savedEntity = await _fixture.DbContext.ExchangeRates.FindAsync(entity.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal(0m, savedEntity.Rate);
    }

    [Fact]
    public async Task SaveChangesAsync_WithNoPendingChanges_CompletesSuccessfully()
    {
        // Act & Assert
        await _repository.SaveChangesAsync();
    }

    [Fact]
    public async Task SaveChangesAsync_CalledMultipleTimes_PersistsOnlyOnce()
    {
        // Arrange
        var entity = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = TargetCurrency,
            Rate = ValidRate,
            Date = _testDate
        };
        await _repository.AddExchangeRateAsync(entity);

        // Act
        await _repository.SaveChangesAsync();
        await _repository.SaveChangesAsync();

        // Assert
        var count = await _fixture.DbContext.ExchangeRates.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task AddAsync_WithSameCurrencies_InsertsIntoDatabase()
    {
        // Arrange
        var entity = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = SourceCurrency,
            Rate = 1.0m,
            Date = _testDate
        };

        // Act
        await _repository.AddExchangeRateAsync(entity);
        await _repository.SaveChangesAsync();

        // Assert
        var savedEntity = await _fixture.DbContext.ExchangeRates.FindAsync(entity.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal(SourceCurrency, savedEntity.SourceCurrency);
        Assert.Equal(SourceCurrency, savedEntity.TargetCurrency);
    }

    [Fact]
    public async Task AddAsync_WithVeryLargeRate_InsertsIntoDatabase()
    {
        // Arrange
        var entity = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = TargetCurrency,
            Rate = 999999999.99m,
            Date = _testDate
        };

        // Act
        await _repository.AddExchangeRateAsync(entity);
        await _repository.SaveChangesAsync();

        // Assert
        var savedEntity = await _fixture.DbContext.ExchangeRates.FindAsync(entity.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal(999999999.99m, savedEntity.Rate);
    }

    [Fact]
    public async Task AddAsync_WithFutureDate_InsertsIntoDatabase()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1));
        var entity = new ExchangeRateEntity
        {
            SourceCurrency = SourceCurrency,
            TargetCurrency = TargetCurrency,
            Rate = ValidRate,
            Date = futureDate
        };

        // Act
        await _repository.AddExchangeRateAsync(entity);
        await _repository.SaveChangesAsync();

        // Assert
        var savedEntity = await _fixture.DbContext.ExchangeRates.FindAsync(entity.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal(futureDate, savedEntity.Date);
    }
}