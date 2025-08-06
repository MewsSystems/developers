using ExchangeRateUpdater.Infrastructure.Services;

namespace ExchangeRateUpdater.Infrastructure.Tests.Services;

public class CacheKeyGeneratorTests
{
    [Fact]
    public void GenerateDailyRatesKey_WithValidProviderName_ShouldReturnCorrectKey()
    {
        var providerName = "TestProvider";
        var date = new DateTime(2024, 1, 15);

        var result = CacheKeyGenerator.GenerateDailyRatesKey(providerName, date);

        Assert.Equal("ExchangeRates:TestProvider:Daily:2024-01-15", result);
    }

    [Fact]
    public void GenerateDailyRatesKey_WithNullDate_ShouldNotIncludeDate()
    {
        var providerName = "TestProvider";

        var result = CacheKeyGenerator.GenerateDailyRatesKey(providerName, null);

        Assert.Equal("ExchangeRates:TestProvider:Daily", result);
    }

    [Fact]
    public void GenerateMonthlyRatesKey_WithValidProviderName_ShouldReturnCorrectKey()
    {
        var providerName = "TestProvider";
        var date = new DateTime(2024, 1, 15);

        var result = CacheKeyGenerator.GenerateMonthlyRatesKey(providerName, date);

        Assert.Equal("ExchangeRates:TestProvider:Monthly:2024-01", result);
    }

    [Fact]
    public void GenerateMonthlyRatesKey_WithNullDate_ShouldNotIncludeDate()
    {
        var providerName = "TestProvider";

        var result = CacheKeyGenerator.GenerateMonthlyRatesKey(providerName, null);

        Assert.Equal("ExchangeRates:TestProvider:Monthly", result);
    }

    [Fact]
    public void GenerateCustomKey_WithValidParameters_ShouldReturnCorrectKey()
    {
        var providerName = "TestProvider";
        var suffix = "CustomSuffix";

        var result = CacheKeyGenerator.GenerateCustomKey(providerName, suffix);

        Assert.Equal("ExchangeRates:TestProvider:CustomSuffix", result);
    }

    [Fact]
    public void GenerateDailyRatesKey_WithEmptyProviderName_ShouldThrowArgumentException()
    {
        var providerName = "";
        var date = new DateTime(2024, 1, 15);

        Assert.Throws<ArgumentException>(() => CacheKeyGenerator.GenerateDailyRatesKey(providerName, date));
    }

    [Fact]
    public void GenerateMonthlyRatesKey_WithEmptyProviderName_ShouldThrowArgumentException()
    {
        var providerName = "";
        var date = new DateTime(2024, 1, 15);

        Assert.Throws<ArgumentException>(() => CacheKeyGenerator.GenerateMonthlyRatesKey(providerName, date));
    }

    [Fact]
    public void GenerateCustomKey_WithEmptyProviderName_ShouldThrowArgumentException()
    {
        var providerName = "";
        var suffix = "CustomSuffix";

        Assert.Throws<ArgumentException>(() => CacheKeyGenerator.GenerateCustomKey(providerName, suffix));
    }

    [Fact]
    public void GenerateDailyRatesKey_WithNullProviderName_ShouldThrowArgumentException()
    {
        string? providerName = null;
        var date = new DateTime(2024, 1, 15);

        Assert.Throws<ArgumentException>(() => CacheKeyGenerator.GenerateDailyRatesKey(providerName, date));
    }

    [Fact]
    public void GenerateMonthlyRatesKey_WithNullProviderName_ShouldThrowArgumentException()
    {
        string? providerName = null;
        var date = new DateTime(2024, 1, 15);

        Assert.Throws<ArgumentException>(() => CacheKeyGenerator.GenerateMonthlyRatesKey(providerName, date));
    }

    [Fact]
    public void GenerateCustomKey_WithNullProviderName_ShouldThrowArgumentException()
    {
        string? providerName = null;
        var suffix = "CustomSuffix";

        Assert.Throws<ArgumentException>(() => CacheKeyGenerator.GenerateCustomKey(providerName, suffix));
    }

    [Fact]
    public void GenerateCustomKey_WithEmptySuffix_ShouldThrowArgumentException()
    {
        var providerName = "TestProvider";
        var suffix = "";

        Assert.Throws<ArgumentException>(() => CacheKeyGenerator.GenerateCustomKey(providerName, suffix));
    }

    [Fact]
    public void GenerateCustomKey_WithNullSuffix_ShouldThrowArgumentException()
    {
        var providerName = "TestProvider";
        string? suffix = null;

        Assert.Throws<ArgumentException>(() => CacheKeyGenerator.GenerateCustomKey(providerName, suffix));
    }

    [Fact]
    public void GenerateDailyRatesKey_WithDifferentDates_ShouldReturnDifferentKeys()
    {
        var providerName = "TestProvider";
        var date1 = new DateTime(2024, 1, 15);
        var date2 = new DateTime(2024, 1, 16);

        var result1 = CacheKeyGenerator.GenerateDailyRatesKey(providerName, date1);
        var result2 = CacheKeyGenerator.GenerateDailyRatesKey(providerName, date2);

        Assert.NotEqual(result1, result2);
        Assert.Equal("ExchangeRates:TestProvider:Daily:2024-01-15", result1);
        Assert.Equal("ExchangeRates:TestProvider:Daily:2024-01-16", result2);
    }

    [Fact]
    public void GenerateMonthlyRatesKey_WithDifferentDates_ShouldReturnDifferentKeys()
    {
        var providerName = "TestProvider";
        var date1 = new DateTime(2024, 1, 15);
        var date2 = new DateTime(2024, 2, 15);

        var result1 = CacheKeyGenerator.GenerateMonthlyRatesKey(providerName, date1);
        var result2 = CacheKeyGenerator.GenerateMonthlyRatesKey(providerName, date2);

        Assert.NotEqual(result1, result2);
        Assert.Equal("ExchangeRates:TestProvider:Monthly:2024-01", result1);
        Assert.Equal("ExchangeRates:TestProvider:Monthly:2024-02", result2);
    }
} 