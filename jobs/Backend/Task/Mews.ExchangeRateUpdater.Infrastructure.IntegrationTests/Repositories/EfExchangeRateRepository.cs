using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Mews.ExchangeRateUpdater.Infrastructure.Persistance;
using Mews.ExchangeRateUpdater.Infrastructure.Persistance.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mews.ExchangeRateUpdater.IntegrationTests.Infrastructure;

public class EfExchangeRateRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _dbContext;
    private readonly EfExchangeRateRepository _repository;

    public EfExchangeRateRepositoryTests()
    {
        // Arrange test database
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _dbContext = new AppDbContext(options);
        _dbContext.Database.EnsureCreated();

        var logger = new Mock<ILogger<EfExchangeRateRepository>>();
        _repository = new EfExchangeRateRepository(_dbContext, logger.Object);
    }

    [Fact]
    public async Task UpsertRatesAsync_InsertsNewRates()
    {
        // Arrange
        var date = DateTime.UtcNow.Date;
        var rates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.345m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.123m)
        };

        // Act
        await _repository.UpsertRatesAsync(rates, date, CancellationToken.None);

        // Assert
        var all = _dbContext.ExchangeRates.ToList();
        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task UpsertRatesAsync_UpdatesExistingRates()
    {
        // Arrange
        var date = DateTime.UtcNow.Date;
        var initial = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 20m)
        };

        await _repository.UpsertRatesAsync(initial, date, CancellationToken.None);

        var updated = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25m)
        };

        // Act
        await _repository.UpsertRatesAsync(updated, date, CancellationToken.None);

        // Assert
        var entity = _dbContext.ExchangeRates.First();
        Assert.Equal(25m, entity.Value);
    }

    [Fact]
    public async Task GetRatesAsync_ReturnsMatchingRates()
    {
        // Arrange
        var date = DateTime.UtcNow.Date;
        var rates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.345m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.123m)
        };

        await _repository.UpsertRatesAsync(rates, date, CancellationToken.None);

        // Act
        var result = await _repository.GetRatesAsync(date, new[] { new Currency("USD") }, CancellationToken.None);
        var rate = result.First();

        // Assert
        Assert.Single(result);
        Assert.Equal("USD", rate.SourceCurrency.Code);
    }

    [Fact]
    public async Task HasRatesForDateAsync_ReturnsTrue_WhenRatesExist()
    {
        // Arrange
        var date = DateTime.UtcNow.Date;
        var rates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.345m)
        };

        await _repository.UpsertRatesAsync(rates, date, CancellationToken.None);

        // Act
        var exists = await _repository.HasRatesForDateAsync(date, CancellationToken.None);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task HasRatesForDateAsync_ReturnsFalse_WhenNoRatesExist()
    {
        // Act
        var exists = await _repository.HasRatesForDateAsync(DateTime.UtcNow.Date, CancellationToken.None);

        // Assert
        Assert.False(exists);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
        _connection?.Dispose();
    }
}
