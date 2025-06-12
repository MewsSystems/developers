using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.DataSources.Cnb;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NodaTime;
using Xunit;

namespace ExchangeRateUpdater.Tests.Unit.DataSources.Cnb;

public class CnbExchangeRateDataSourceTests
{
    private readonly Mock<HttpClient> _httpClientMock;
    private readonly Mock<ILogger<CnbExchangeRateDataSource>> _loggerMock;
    private readonly Mock<IOptions<ExchangeRateServiceOptions>> _optionsMock;
    private readonly CnbExchangeRateDataSource _dataSource;

    public CnbExchangeRateDataSourceTests()
    {
        _httpClientMock = new Mock<HttpClient>();
        _loggerMock = new Mock<ILogger<CnbExchangeRateDataSource>>();
        _optionsMock = new Mock<IOptions<ExchangeRateServiceOptions>>();

        _optionsMock.Setup(x => x.Value)
            .Returns(new ExchangeRateServiceOptions());

        _dataSource = new CnbExchangeRateDataSource(
            _httpClientMock.Object,
            _loggerMock.Object,
            _optionsMock.Object);
    }

    [Fact]
    public void ParseCnbData_ComprehensiveDataSet_ParsesAllCurrenciesWithCorrectAmounts()
    {
        // Arrange
        var content = @"16 May 2025 #93
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|14.280
Brazil|real|1|BRL|3.913
Bulgaria|lev|1|BGN|12.746
Canada|dollar|1|CAD|15.941
China|renminbi|1|CNY|3.091
Denmark|krone|1|DKK|3.342
EMU|euro|1|EUR|24.930
Hongkong|dollar|1|HKD|2.850
Hungary|forint|100|HUF|6.184
Iceland|krona|100|ISK|17.087
IMF|SDR|1|XDR|30.046
India|rupee|100|INR|26.020
Indonesia|rupiah|1000|IDR|1.355
Israel|new shekel|1|ILS|6.267
Japan|yen|100|JPY|15.291
Malaysia|ringgit|1|MYR|5.185
Mexico|peso|1|MXN|1.142
New Zealand|dollar|1|NZD|13.128
Norway|krone|1|NOK|2.143
Philippines|peso|100|PHP|39.980
Poland|zloty|1|PLN|5.855
Romania|leu|1|RON|4.884
Singapore|dollar|1|SGD|17.147
South Africa|rand|1|ZAR|1.231
South Korea|won|100|KRW|1.594
Sweden|krona|1|SEK|2.280
Switzerland|franc|1|CHF|26.581
Thailand|baht|100|THB|66.716
Turkey|lira|100|TRY|57.357
United Kingdom|pound|1|GBP|29.587
USA|dollar|1|USD|22.274";

        var date = new LocalDate(2025, 5, 16);

        // Act
        var result = _dataSource.ParseCnbData(content, date);

        // Assert
        result.Should()
            .NotBeNull();
        result.Date.Should()
            .Be(date);
        result.PublishedDate.Should()
            .Be(date);
        result.Rates.Should()
            .HaveCount(31); // Verify all 31 currencies were parsed

        // Test regular amount (1) currencies
        var usdRate = result.Rates.First(r => r.Currency == "USD");
        usdRate.Country.Should()
            .Be("USA");
        usdRate.Amount.Should()
            .Be(1);
        usdRate.Rate.Should()
            .Be(22.274m);

        var eurRate = result.Rates.First(r => r.Currency == "EUR");
        eurRate.Country.Should()
            .Be("EMU");
        eurRate.Amount.Should()
            .Be(1);
        eurRate.Rate.Should()
            .Be(24.930m);

        // Test amount of 100 currencies
        var hufRate = result.Rates.First(r => r.Currency == "HUF");
        hufRate.Country.Should()
            .Be("Hungary");
        hufRate.Amount.Should()
            .Be(100);
        hufRate.Rate.Should()
            .Be(6.184m);

        var jpyRate = result.Rates.First(r => r.Currency == "JPY");
        jpyRate.Country.Should()
            .Be("Japan");
        jpyRate.Amount.Should()
            .Be(100);
        jpyRate.Rate.Should()
            .Be(15.291m);

        // Test amount of 1000 currencies
        var idrRate = result.Rates.First(r => r.Currency == "IDR");
        idrRate.Country.Should()
            .Be("Indonesia");
        idrRate.Amount.Should()
            .Be(1000);
        idrRate.Rate.Should()
            .Be(1.355m);

        // Verify that all expected currencies are present
        var allCurrencies = result.Rates.Select(r => r.Currency)
            .ToList();
        allCurrencies.Should()
            .Contain(new[]
            {
                "AUD", "BRL", "BGN", "CAD", "CNY", "DKK", "EUR", "HKD", "HUF",
                "ISK", "XDR", "INR", "IDR", "ILS", "JPY", "MYR", "MXN", "NZD",
                "NOK", "PHP", "PLN", "RON", "SGD", "ZAR", "KRW", "SEK", "CHF",
                "THB", "TRY", "GBP", "USD"
            });
    }

    [Fact]
    public void ParseCnbData_EmptyContent_ReturnsEmptyRates()
    {
        // Arrange
        var content = "";
        var date = new LocalDate(2025, 5, 16);

        // Act
        var result = _dataSource.ParseCnbData(content, date);

        // Assert
        result.Should()
            .NotBeNull();
        result.Date.Should()
            .Be(date);
        result.Rates.Should()
            .BeEmpty();
    }

    [Fact]
    public void ParseCnbData_InvalidLineFormat_SkipsInvalidLine()
    {
        // Arrange
        var content = @"16 May 2025 #93
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|14.280
InvalidLine
Canada|dollar|1|CAD|16.431";

        var date = new LocalDate(2025, 5, 16);

        // Act
        var result = _dataSource.ParseCnbData(content, date);

        // Assert
        result.Should()
            .NotBeNull();
        result.Rates.Should()
            .HaveCount(2);
        result.Rates.Should()
            .Contain(r => r.Currency == "AUD");
        result.Rates.Should()
            .Contain(r => r.Currency == "CAD");
    }

    [Fact]
    public void ParseCnbData_InvalidNumberFormat_SkipsInvalidLine()
    {
        // Arrange
        var content = @"16 May 2025 #93
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|14.280
Canada|dollar|InvalidNumber|CAD|16.431
USA|dollar|1|USD|22.274";

        var date = new LocalDate(2025, 5, 16);

        // Act
        var result = _dataSource.ParseCnbData(content, date);

        // Assert
        result.Should()
            .NotBeNull();
        result.Rates.Should()
            .HaveCount(2);
        result.Rates.Should()
            .Contain(r => r.Currency == "AUD");
        result.Rates.Should()
            .Contain(r => r.Currency == "USD");
    }

    [Fact]
    public void ParseDecimalWithEitherSeparator_HandlesEitherDecimalSeparator()
    {
        // Arrange & Act & Assert
        CnbExchangeRateDataSource.ParseDecimalWithEitherSeparator("14.280")
            .Should()
            .Be(14.280m);
        CnbExchangeRateDataSource.ParseDecimalWithEitherSeparator("14,280")
            .Should()
            .Be(14.280m);
    }
}