using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExchangeRateUpdater.Tests.Data;

[Trait("Category", "Unit")]
public class ExchangeRateDbContextTests
{
    private const string ValidConnectionString = "Host=localhost;Database=test;Username=user;Password=pass";
    private readonly Mock<IAppConfiguration> _mockConfig = new();

    private readonly DbContextOptions<ExchangeRateDbContext> _options =
        new DbContextOptionsBuilder<ExchangeRateDbContext>()
            .Options;

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange
        _mockConfig.Setup(c => c.DatabaseConnectionString).Returns(ValidConnectionString);

        // Act
        var context = new ExchangeRateDbContext(_options, _mockConfig.Object);

        // Assert
        Assert.NotNull(context);
        Assert.NotNull(context.ExchangeRates);
    }

    [Fact]
    public void OnConfiguring_WithValidConnectionString_ConfiguresNpgsql()
    {
        // Arrange
        _mockConfig.Setup(c => c.DatabaseConnectionString).Returns(ValidConnectionString);
        var context = new ExchangeRateDbContext(_options, _mockConfig.Object);

        // Act
        var exception = Record.Exception(() => context.Database.CanConnect());

        // Assert
        _mockConfig.Verify(c => c.DatabaseConnectionString, Times.AtLeastOnce);
    }

    [Fact]
    public void OnConfiguring_WithEmptyConnectionString_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockConfig.Setup(c => c.DatabaseConnectionString).Returns(string.Empty);
        var context = new ExchangeRateDbContext(_options, _mockConfig.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => context.Database.CanConnect());
        Assert.Contains("Database connection string is not configured", exception.Message);
    }

    [Fact]
    public void OnConfiguring_WithWhitespaceConnectionString_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockConfig.Setup(c => c.DatabaseConnectionString).Returns("   ");
        var context = new ExchangeRateDbContext(_options, _mockConfig.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => context.Database.CanConnect());
        Assert.Contains("Database connection string is not configured", exception.Message);
    }
}