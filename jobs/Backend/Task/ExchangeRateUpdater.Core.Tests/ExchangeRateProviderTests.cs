using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Services;
using ExchangeRateUpdater.Infra;
using ExchangeRateUpdater.Infra.Http;
using ExchangeRateUpdater.Infra.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;
using ExchangeRate = ExchangeRateUpdater.Infra.Models.ExchangeRate;

namespace ExchangeRateUpdater.Core.Tests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnExpectedRatesWhenHttpCallIsSuccessful()
    {
        // arrange
        var response = new ExchangeRateResponse
        {
            Rates = new[]
            {
                new ExchangeRate { CurrencyCode = "USD", Amount = 1, Rate = 100 },
                new ExchangeRate { CurrencyCode = "EUR", Amount = 10, Rate = 100 },
                new ExchangeRate { CurrencyCode = "BBB", Amount = 1, Rate = 100 }
            }
        };
        var cbnHttpClientMock = new Mock<ICnbHttpClient>();
        cbnHttpClientMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(Result<ExchangeRateResponse, HttpError>.Success(response));
        var sut = new ExchangeRateProvider(cbnHttpClientMock.Object, new Mock<ILogger<ExchangeRateProvider>>().Object);
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("AAA"),
        };
        
        // act
        var rates = await sut.GetExchangeRatesAsync(currencies);

        // assert
        var rateList = rates.ToList();
        rateList.ShouldNotBeNull();
        rateList.ShouldAllBe(rate => rate.TargetCurrency.Code == "CZH");
        rateList.Count.ShouldBe(2);
        rateList.ShouldContain(rate => rate.SourceCurrency.Code == "EUR" && rate.Value == 10);
        rateList.ShouldContain(rate => rate.SourceCurrency.Code == "USD" && rate.Value == 100);
        rateList.ShouldNotContain(rate => rate.SourceCurrency.Code == "AAA");
        rateList.ShouldNotContain(rate => rate.SourceCurrency.Code == "BBB");
    }
    
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnEmptyListWhenHttpCallReturnsEmptyList()
    {
        // arrange
        var response = new ExchangeRateResponse();
        var cbnHttpClientMock = new Mock<ICnbHttpClient>();
        cbnHttpClientMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(Result<ExchangeRateResponse, HttpError>.Success(response));
        var sut = new ExchangeRateProvider(cbnHttpClientMock.Object, new Mock<ILogger<ExchangeRateProvider>>().Object);
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("AAA"),
        };
        
        // act
        var rates = await sut.GetExchangeRatesAsync(currencies);

        // assert
        var rateList = rates.ToList();
        rateList.ShouldNotBeNull();
        rateList.ShouldBeEmpty();
    }
    
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnEmptyListWhenHttpCallReturnsError()
    {
        // arrange
        var cbnHttpClientMock = new Mock<ICnbHttpClient>();
        cbnHttpClientMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(Result<ExchangeRateResponse, HttpError>.Failure(new HttpError(It.IsAny<HttpStatusCode>(),
                It.IsAny<string>())));
        var sut = new ExchangeRateProvider(cbnHttpClientMock.Object, new Mock<ILogger<ExchangeRateProvider>>().Object);
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("AAA"),
        };
        
        // act
        var rates = await sut.GetExchangeRatesAsync(currencies);

        // assert
        var rateList = rates.ToList();
        rateList.ShouldNotBeNull();
        rateList.ShouldBeEmpty();
    }
    
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnEmptyListWhenNoCurrencies()
    {
        // arrange
        var response = new ExchangeRateResponse
        {
            Rates = new[]
            {
                new ExchangeRate { CurrencyCode = "USD", Amount = 1, Rate = 100 },
                new ExchangeRate { CurrencyCode = "EUR", Amount = 1, Rate = 100 },
                new ExchangeRate { CurrencyCode = "BBB", Amount = 1, Rate = 100 }
            }
        };
        var cbnHttpClientMock = new Mock<ICnbHttpClient>();
        cbnHttpClientMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(Result<ExchangeRateResponse, HttpError>.Success(response));
        var sut = new ExchangeRateProvider(cbnHttpClientMock.Object, new Mock<ILogger<ExchangeRateProvider>>().Object);
        var currencies = Enumerable.Empty<Currency>();
        
        // act
        var rates = await sut.GetExchangeRatesAsync(currencies);

        // assert
        var rateList = rates.ToList();
        rateList.ShouldNotBeNull();
        rateList.ShouldBeEmpty();
    }
}