using Mews.ExchangeRate.Http.Abstractions;
using Mews.ExchangeRate.Http.Abstractions.Exceptions;
using Mews.ExchangeRate.Http.Cnb.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Json;
using Xunit.Abstractions;

namespace Mews.ExchangeRate.Http.Cnb.UnitTests;

public class ExchangeRateServiceClientShould
{
    private readonly ITestOutputHelper _output;

    public ExchangeRateServiceClientShould(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task GetCurrencyExchangeRatesAsync_WithNotValidDateOrLanguage_ThrowExchangeRateServiceResponseException()
    {
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var httpClientMock = new Mock<IHttpClient>();
        httpClientMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    new ErrorResponse()
                    {
                        HappenedAt = DateTime.Now.ToString(),
                        Description = "Bad request",
                        ErrorCode = "400",
                    }
                    ))
            });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            httpClientMock.Object,
            optionsSnapshotMock.Object
            );

        await Assert.ThrowsAsync<ExchangeRateServiceResponseException>(
            async () => await serviceClient.GetCurrencyExchangeRatesAsync(DateTime.Now)
            );
    }

    [Fact]
    public async Task GetCurrencyExchangeRatesAsync_WithValidDateAndLanguage_ReturnExchangeRates()
    {
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var httpClientMock = new Mock<IHttpClient>();
        var value = new ExchangeRateResponse()
        {
            ExchangeRates = new[]
                                {
                            new ExchangeRateResponse.ExchangeRate()
                            {
                                Amount = 1,
                                CurrencyCode = "CZK",
                                Country = "Rzech Republic",
                                Currency = "koruna",
                                Rate = 1,
                            }
                        },
        };

        _ = httpClientMock
            .Setup<Task<HttpResponseMessage>>(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    value
                ))
            });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            httpClientMock.Object,
            optionsSnapshotMock.Object
            );

        var exchangeRates = await serviceClient.GetCurrencyExchangeRatesAsync(DateTime.Now);

        Assert.NotNull(exchangeRates);
        Assert.Single(exchangeRates);
        Assert.Equal("CZK", exchangeRates.First().CurrencyCode);
    }

    [Fact]
    public async Task GetCurrencyExchangeRatesAsync_WithValidDateAndLanguageReturningEmptyRates_ReturnEmptyRates()
    {
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var httpClientMock = new Mock<IHttpClient>();
        var value = new ExchangeRateResponse()
        {
            ExchangeRates = Enumerable.Empty<ExchangeRateResponse.ExchangeRate>(),
        };

        _ = httpClientMock
            .Setup<Task<HttpResponseMessage>>(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    value
                ))
            });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            httpClientMock.Object,
            optionsSnapshotMock.Object
            );

        var exchangeRates = await serviceClient.GetCurrencyExchangeRatesAsync(DateTime.Now);

        Assert.NotNull(exchangeRates);
        Assert.Empty(exchangeRates);
    }

    [Fact]
    public async Task GetForeignCurrencyExchangeRatesAsync_WithNotValidDateOrLanguage_ThrowExchangeRateServiceResponseException()
    {
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var httpClientMock = new Mock<IHttpClient>();
        httpClientMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    new ErrorResponse()
                    {
                        HappenedAt = DateTime.Now.ToString(),
                        Description = "Bad request",
                        ErrorCode = "400",
                    }
                    ))
            });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            httpClientMock.Object,
            optionsSnapshotMock.Object
            );

        await Assert.ThrowsAsync<ExchangeRateServiceResponseException>(
            async () => await serviceClient.GetForeignCurrencyExchangeRatesAsync(DateTime.Now)
            );
    }

    [Fact]
    public async Task GetForeignCurrencyExchangeRatesAsync_WithValidDateAndLanguage_ReturnExchangeRates()
    {
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var httpClientMock = new Mock<IHttpClient>();
        var value = new ExchangeRateResponse()
        {
            ExchangeRates = new[]
                                {
                            new ExchangeRateResponse.ExchangeRate()
                            {
                                Amount = 1,
                                CurrencyCode = "CZK",
                                Country = "Rzech Republic",
                                Currency = "koruna",
                                Rate = 1,
                            }
                        },
        };

        _ = httpClientMock
            .Setup<Task<HttpResponseMessage>>(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    value
                ))
            });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            httpClientMock.Object,
            optionsSnapshotMock.Object
            );

        var exchangeRates = await serviceClient.GetForeignCurrencyExchangeRatesAsync(DateTime.Now);

        Assert.NotNull(exchangeRates);
        Assert.Single(exchangeRates);
        Assert.Equal("CZK", exchangeRates.First().CurrencyCode);
    }

    [Fact]
    public async Task GetForeignCurrencyExchangeRatesAsync_WithValidDateAndLanguageReturningEmptyRates_ReturnEmptyRates()
    {
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ExchangeRateServiceClientOptions>>();
        optionsSnapshotMock.Setup(o => o.Value).Returns(new ExchangeRateServiceClientOptions()
        {
            ApiBaseUrl = "https://api.cnb.cz",
            CurrencyExchangeRatesEndpoint = "/cnbapi/exrates/daily",
            ForeignExchangeRatesEndpoint = "/cnbapi/fxrates/daily-month",
            Language = "EN"
        });

        var httpClientMock = new Mock<IHttpClient>();
        var value = new ExchangeRateResponse()
        {
            ExchangeRates = Enumerable.Empty<ExchangeRateResponse.ExchangeRate>(),
        };

        _ = httpClientMock
            .Setup<Task<HttpResponseMessage>>(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    value
                ))
            });

        var serviceClient = new ExchangeRateServiceClient(
            Mock.Of<ILogger<ExchangeRateServiceClient>>(),
            httpClientMock.Object,
            optionsSnapshotMock.Object
            );

        var exchangeRates = await serviceClient.GetForeignCurrencyExchangeRatesAsync(DateTime.Now);

        Assert.NotNull(exchangeRates);
        Assert.Empty(exchangeRates);
    }
}