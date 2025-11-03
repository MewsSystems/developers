using ExchangeRateUpdater.Config;

namespace ExchangeRateUpdater.Tests.Config;

public class ConfigurationLoaderTests : IDisposable
{
    private const string ValidDailyRateUrl = "https://example.com/rates";
    private const string ValidCurrencies = "USD,EUR,GBP";
    private const string ValidLogLevel = "Information";
    private const string ValidConnectionString = "Server=localhost;Database=test;";
    private const string ValidProviderType = "Csv";
    private const string ValidExporterType = "Console";

    public ConfigurationLoaderTests()
    {
        ClearEnvironmentVariables();
        SetValidEnvironmentVariables();
    }

    public void Dispose()
    {
        ClearEnvironmentVariables();
    }

    private static void ClearEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("DAILY_RATE_URL", null);
        Environment.SetEnvironmentVariable("CURRENCIES", null);
        Environment.SetEnvironmentVariable("LOG_LEVEL", null);
        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", null);
        Environment.SetEnvironmentVariable("PROVIDER_TYPE", null);
        Environment.SetEnvironmentVariable("EXPORTER_TYPE", null);
    }

    private static void SetValidEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("DAILY_RATE_URL", ValidDailyRateUrl);
        Environment.SetEnvironmentVariable("CURRENCIES", ValidCurrencies);
        Environment.SetEnvironmentVariable("LOG_LEVEL", ValidLogLevel);
        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", ValidConnectionString);
        Environment.SetEnvironmentVariable("PROVIDER_TYPE", ValidProviderType);
        Environment.SetEnvironmentVariable("EXPORTER_TYPE", ValidExporterType);
    }

    [Fact]
    public void Load_WithAllValidEnvironmentVariables_ReturnsValidConfiguration()
    {
        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal(ValidDailyRateUrl, config.DailyRateUrl);
        Assert.Equal(ValidCurrencies, config.Currencies);
        Assert.Equal(ValidLogLevel, config.LogLevel);
        Assert.Equal(ValidConnectionString, config.DatabaseConnectionString);
        Assert.Equal(RateProviderType.Csv, config.ProviderType);
        Assert.Equal(RateExporterType.Console, config.ExporterType);
    }

    [Fact]
    public void Load_WithMissingCurrencies_UsesDefaultCurrencies()
    {
        // Arrange
        Environment.SetEnvironmentVariable("CURRENCIES", null);

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal("USD,EUR,GBP", config.Currencies);
    }

    [Fact]
    public void Load_WithEmptyCurrencies_UsesDefaultCurrencies()
    {
        // Arrange
        Environment.SetEnvironmentVariable("CURRENCIES", "");

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal("USD,EUR,GBP", config.Currencies);
    }

    [Fact]
    public void Load_WithMissingProviderType_UsesDefaultCsv()
    {
        // Arrange
        Environment.SetEnvironmentVariable("PROVIDER_TYPE", null);

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal(RateProviderType.Csv, config.ProviderType);
    }

    [Fact]
    public void Load_WithEmptyProviderType_UsesDefaultCsv()
    {
        // Arrange
        Environment.SetEnvironmentVariable("PROVIDER_TYPE", "");

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal(RateProviderType.Csv, config.ProviderType);
    }

    [Theory]
    [InlineData("csv", RateProviderType.Csv)]
    [InlineData("CSV", RateProviderType.Csv)]
    [InlineData("Csv", RateProviderType.Csv)]
    [InlineData("rest", RateProviderType.Rest)]
    [InlineData("REST", RateProviderType.Rest)]
    [InlineData("Rest", RateProviderType.Rest)]
    public void Load_WithCaseInsensitiveProviderType_ParsesCorrectly(string providerType, RateProviderType expected)
    {
        // Arrange
        Environment.SetEnvironmentVariable("PROVIDER_TYPE", providerType);

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal(expected, config.ProviderType);
    }

    [Fact]
    public void Load_WithMissingExporterType_UsesDefaultConsole()
    {
        // Arrange
        Environment.SetEnvironmentVariable("EXPORTER_TYPE", null);

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal(RateExporterType.Console, config.ExporterType);
    }

    [Fact]
    public void Load_WithEmptyExporterType_UsesDefaultConsole()
    {
        // Arrange
        Environment.SetEnvironmentVariable("EXPORTER_TYPE", "");

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal(RateExporterType.Console, config.ExporterType);
    }

    [Theory]
    [InlineData("console", RateExporterType.Console)]
    [InlineData("CONSOLE", RateExporterType.Console)]
    [InlineData("Console", RateExporterType.Console)]
    [InlineData("database", RateExporterType.Database)]
    [InlineData("DATABASE", RateExporterType.Database)]
    [InlineData("Database", RateExporterType.Database)]
    public void Load_WithCaseInsensitiveExporterType_ParsesCorrectly(string exporterType, RateExporterType expected)
    {
        // Arrange
        Environment.SetEnvironmentVariable("EXPORTER_TYPE", exporterType);

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal(expected, config.ExporterType);
    }

    [Fact]
    public void Load_WithMissingDailyRateUrl_ReturnsEmptyString()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DAILY_RATE_URL", null);

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal(string.Empty, config.DailyRateUrl);
    }

    [Fact]
    public void Load_WithMissingLogLevel_UsesDefaultDebug()
    {
        // Arrange
        Environment.SetEnvironmentVariable("LOG_LEVEL", null);

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal("Debug", config.LogLevel);
    }

    [Fact]
    public void Load_WithMissingDatabaseConnectionString_ReturnsEmptyString()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", null);

        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal("", config.DatabaseConnectionString);
    }

    [Fact]
    public void Load_WithInvalidProviderType_ThrowsArgumentException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("PROVIDER_TYPE", "InvalidType");

        // Act & Assert
        Assert.Throws<ArgumentException>(ConfigurationLoader.Load);
    }

    [Fact]
    public void Load_WithInvalidExporterType_ThrowsArgumentException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("EXPORTER_TYPE", "InvalidType");

        // Act & Assert
        Assert.Throws<ArgumentException>(ConfigurationLoader.Load);
    }

    [Fact]
    public void Load_AlwaysSetsCzkCurrencyCode()
    {
        // Act
        var config = ConfigurationLoader.Load();

        // Assert
        Assert.Equal("CZK", config.CzkCurrencyCode);
    }
}