using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Json;
using Xunit.Abstractions;

namespace Mews.ExchangeRate.Http.Cnb.IntegrationTests;

public class ExchangeRateServiceClientShould
{
    private readonly ITestOutputHelper _output;

    public ExchangeRateServiceClientShould(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task GetCurrencyExchangeRatesAsync_WithEmptyDateAndLanguage_ReturnEmptyRates()
    {
        var dateWithEmptyValues = default(DateTime);
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions() { 
            ApiBaseUrl= "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint= "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint= "/cnbapi/fxrates/daily-month",
            Language="EN"
        });


        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            new ExchangeRateServiceHttpClient(new HttpClient()),
            optionsSnapshotMock.Object
            );

        var exchangeRates = await serviceClient.GetCurrencyExchangeRatesAsync(dateWithEmptyValues);

        _output.WriteLine(JsonSerializer.Serialize(exchangeRates));

        Assert.NotNull(exchangeRates);
        Assert.Empty(exchangeRates);
    }

    [Fact]
    public async Task GetCurrencyExchangeRatesAsync_WithFutureValidDate_ReturnCurrentExchangeRates()
    {
        var dateWithEmptyValues = DateTime.UtcNow.AddMonths(2);
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            new ExchangeRateServiceHttpClient(new HttpClient()),
            optionsSnapshotMock.Object
            );

        var exchangeRates = await serviceClient.GetCurrencyExchangeRatesAsync(dateWithEmptyValues);
        _output.WriteLine(JsonSerializer.Serialize(exchangeRates));

        Assert.NotNull(exchangeRates);
        Assert.NotEmpty(exchangeRates);
        Assert.All(exchangeRates, rate =>
        {
            Assert.False(string.IsNullOrWhiteSpace(rate.CurrencyCode));
            Assert.False(string.IsNullOrWhiteSpace(rate.CurrencyName));
            Assert.False(string.IsNullOrWhiteSpace(rate.Country));
            Assert.True(rate.Amount > 0);
            Assert.True(rate.Rate > 0);
        });
    }

    [Fact]
    public async Task GetCurrencyExchangeRatesAsync_WithValidDateAndLanguage_ReturnExchangeRates()
    {
        var dateWithKnownValues = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc);
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            new ExchangeRateServiceHttpClient(new HttpClient()),
            optionsSnapshotMock.Object
            );

        var exchangeRates = await serviceClient.GetCurrencyExchangeRatesAsync(dateWithKnownValues);
        _output.WriteLine(JsonSerializer.Serialize(exchangeRates));

        Assert.NotNull(exchangeRates);
        Assert.NotEmpty(exchangeRates);
        Assert.All(exchangeRates, rate =>
        {
            Assert.False(string.IsNullOrWhiteSpace(rate.CurrencyCode));
            Assert.False(string.IsNullOrWhiteSpace(rate.CurrencyName));
            Assert.False(string.IsNullOrWhiteSpace(rate.Country));
            Assert.True(rate.Amount >= 0);
            Assert.True(rate.Rate >= 0);
        });
    }

    [Fact]
    public async Task GetForeignCurrencyExchangeRatesAsync_WithCurrentDateAndLanguage_ReturnEmptyExchangeRates()
    {
        var currentDate = DateTime.UtcNow;
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            new ExchangeRateServiceHttpClient(new HttpClient()),
            optionsSnapshotMock.Object
        );

        var exchangeRates = await serviceClient.GetForeignCurrencyExchangeRatesAsync(currentDate);
        _output.WriteLine(JsonSerializer.Serialize(exchangeRates));

        Assert.NotNull(exchangeRates);
        Assert.Empty(exchangeRates);
    }

    [Fact]
    public async Task GetForeignCurrencyExchangeRatesAsync_WithEmptyDateAndLanguageReturningEmptyRates_ReturnEmptyRates()
    {
        var currentDate = default(DateTime);
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            new ExchangeRateServiceHttpClient(new HttpClient()),
            optionsSnapshotMock.Object
        );

        var exchangeRates = await serviceClient.GetForeignCurrencyExchangeRatesAsync(currentDate);
        _output.WriteLine(JsonSerializer.Serialize(exchangeRates));

        Assert.NotNull(exchangeRates);
        Assert.Empty(exchangeRates);
    }

    [Fact]
    public async Task GetForeignCurrencyExchangeRatesAsync_WithFutureDateAndLanguage_ReturnEmptyExchangeRates()
    {
        var currentDate = DateTime.UtcNow.AddYears(1);
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            new ExchangeRateServiceHttpClient(new HttpClient()),
            optionsSnapshotMock.Object
        );

        var exchangeRates = await serviceClient.GetForeignCurrencyExchangeRatesAsync(currentDate);
        _output.WriteLine(JsonSerializer.Serialize(exchangeRates));

        Assert.NotNull(exchangeRates);
        Assert.Empty(exchangeRates);
    }

    [Fact]
    public async Task GetForeignCurrencyExchangeRatesAsync_WithValidDateAndLanguage_ReturnExchangeRates()
    {
        var dateWithKnownValues = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc);
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            new ExchangeRateServiceHttpClient(new HttpClient()),
            optionsSnapshotMock.Object
            );

        var exchangeRates = await serviceClient.GetForeignCurrencyExchangeRatesAsync(dateWithKnownValues);
        _output.WriteLine(JsonSerializer.Serialize(exchangeRates));

        Assert.NotNull(exchangeRates);
        Assert.NotEmpty(exchangeRates);
        Assert.All(exchangeRates, rate =>
        {
            Assert.False(string.IsNullOrWhiteSpace(rate.CurrencyCode));
            Assert.False(string.IsNullOrWhiteSpace(rate.CurrencyName));
            Assert.False(string.IsNullOrWhiteSpace(rate.Country));
            Assert.True(rate.Amount >= 0);
            Assert.True(rate.Rate >= 0);
        });
    }
}