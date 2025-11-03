using ExchangeRateUpdater.Config;
using Serilog.Events;

namespace ExchangeRateUpdater.Tests.Config;

public class AppConfigurationTests
{
    private const string ValidDailyRateUrl = "https://api.example.com/rates";
    private const string ValidCurrencies = "USD,EUR,GBP";
    private const string ValidLogLevel = "Information";
    private const string ValidCzkCurrencyCode = "CZK";
    private const string ValidConnectionString = "Server=localhost;Database=test;";

    [Fact]
    public void GetCurrencies_WithValidCurrencies_ReturnsExpectedCurrencies()
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = ValidCurrencies,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act
        var currencies = config.GetCurrencies().ToList();

        // Assert
        Assert.Equal(3, currencies.Count);
        Assert.Contains(currencies, c => c.Code == "USD");
        Assert.Contains(currencies, c => c.Code == "EUR");
        Assert.Contains(currencies, c => c.Code == "GBP");
    }

    [Fact]
    public void GetCurrencies_WithEmptyCurrencies_ReturnsEmptyCollection()
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = "",
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act
        var currencies = config.GetCurrencies();

        // Assert
        Assert.Empty(currencies);
    }

    [Fact]
    public void GetCurrencies_WithNullCurrencies_ReturnsEmptyCollection()
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = null,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act
        var currencies = config.GetCurrencies();

        // Assert
        Assert.Empty(currencies);
    }

    [Fact]
    public void GetCurrencies_WithWhitespaceCurrencies_ReturnsEmptyCollection()
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = "   ",
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act
        var currencies = config.GetCurrencies();

        // Assert
        Assert.Empty(currencies);
    }

    [Fact]
    public void GetCurrencies_TrimsAndUppercasesCurrencyCodes()
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = " usd , eur , gbp ",
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act
        var currencies = config.GetCurrencies().ToList();

        // Assert
        Assert.All(currencies, c => Assert.Equal(c.Code, c.Code.ToUpperInvariant()));
    }

    [Fact]
    public void Validate_WithValidConfiguration_DoesNotThrow()
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = ValidCurrencies,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act & Assert
        var exception = Record.Exception(() => config.Validate());
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Validate_WithInvalidDailyRateUrl_ThrowsInvalidOperationException(string invalidUrl)
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = invalidUrl,
            Currencies = ValidCurrencies,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => config.Validate());
        Assert.Contains("DAILY_RATE_URL", exception.Message);
    }

    [Theory]
    [InlineData("csv")]
    [InlineData("CSV")]
    [InlineData("Csv")]
    [InlineData("rest")]
    [InlineData("REST")]
    [InlineData("Rest")]
    public void Validate_WithCaseInsensitiveProviderType_DoesNotThrow(string providerType)
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = ValidCurrencies,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = Enum.Parse<RateProviderType>(providerType, true),
            ExporterType = RateExporterType.Console
        };

        // Act & Assert
        var exception = Record.Exception(() => config.Validate());
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("console")]
    [InlineData("CONSOLE")]
    [InlineData("Console")]
    [InlineData("database")]
    [InlineData("DATABASE")]
    [InlineData("Database")]
    public void Validate_WithCaseInsensitiveExporterType_DoesNotThrow(string exporterType)
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = ValidCurrencies,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = Enum.Parse<RateExporterType>(exporterType, true),
        };

        // Act & Assert
        var exception = Record.Exception(() => config.Validate());
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("XXX")]
    [InlineData("USD,INVALID")]
    [InlineData("ABC,DEF,GHI")]
    public void Validate_WithInvalidCurrencyCodes_ThrowsInvalidOperationException(string invalidCurrencies)
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = invalidCurrencies,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => config.Validate());
        Assert.Contains("Invalid currency code", exception.Message);
    }

    [Fact]
    public void Validate_WithInvalidProviderType_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = ValidCurrencies,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = (RateProviderType)999,
            ExporterType = RateExporterType.Console
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => config.Validate());
        Assert.Contains("Invalid provider type", exception.Message);
    }

    [Fact]
    public void Validate_WithInvalidExporterType_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = ValidCurrencies,
            LogLevel = ValidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = (RateExporterType)999
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => config.Validate());
        Assert.Contains("Invalid exporter type", exception.Message);
    }

    [Theory]
    [InlineData("Verbose", LogEventLevel.Verbose)]
    [InlineData("VERBOSE", LogEventLevel.Verbose)]
    [InlineData("Debug", LogEventLevel.Debug)]
    [InlineData("DEBUG", LogEventLevel.Debug)]
    [InlineData("Information", LogEventLevel.Information)]
    [InlineData("INFORMATION", LogEventLevel.Information)]
    [InlineData("Warning", LogEventLevel.Warning)]
    [InlineData("WARNING", LogEventLevel.Warning)]
    [InlineData("Error", LogEventLevel.Error)]
    [InlineData("ERROR", LogEventLevel.Error)]
    [InlineData("Fatal", LogEventLevel.Fatal)]
    [InlineData("FATAL", LogEventLevel.Fatal)]
    public void GetLogLevel_WithValidLogLevel_ReturnsExpectedLevel(string logLevel, LogEventLevel expected)
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = ValidCurrencies,
            LogLevel = logLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act
        var result = config.GetLogLevel();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData(null)]
    public void GetLogLevel_WithInvalidLogLevel_ReturnsInformationAsDefault(string invalidLogLevel)
    {
        // Arrange
        var config = new AppConfiguration
        {
            DailyRateUrl = ValidDailyRateUrl,
            Currencies = ValidCurrencies,
            LogLevel = invalidLogLevel,
            CzkCurrencyCode = ValidCzkCurrencyCode,
            DatabaseConnectionString = ValidConnectionString,
            ProviderType = RateProviderType.Csv,
            ExporterType = RateExporterType.Console
        };

        // Act
        var result = config.GetLogLevel();

        // Assert
        Assert.Equal(LogEventLevel.Information, result);
    }
}