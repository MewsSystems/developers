using System.Collections.Specialized;
using System.Net;
using AutoFixture;
using FluentAssertions;
using Mews.ERP.AppService.Data.Models;
using Mews.ERP.AppService.Features.Fetch.Builders.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Networking.Responses;
using Moq;
using Moq.AutoMock;
using RestSharp;

namespace Mews.ERP.AppService.UnitTests.Features.Fetch.Networking.Providers;

public class CnbExchangeRatesProviderTests
{
    private const string TestDataFileLocation = "Features/Fetch/Networking/TestData";
    
    private readonly AutoMocker autoMocker = new();

    private readonly Fixture fixture = new();

    private readonly ICnbExchangeRatesProvider sut;
    
    public CnbExchangeRatesProviderTests()
    {
        sut = autoMocker.CreateInstance<CnbExchangeRatesProvider>();
    }
    
    [Fact]
    public async Task GetExchangeRates_Should_Filter_Based_On_Provided_Currencies_List()
    {
        // Arrange
        var currencies = new List<Currency>()
        {
            new("AUD"),
            new("EUR"),
            new("GBP"),
            new("USD")
        };
        
        var mockedResponse = await File.ReadAllTextAsync($"{TestDataFileLocation}/mockedResponse.json");
        var cachedValue = Enumerable.Empty<ExchangeRateResponse>();
        
        autoMocker
            .GetMock<IRestRequestBuilder>()
            .Setup(b => b.Build(It.IsAny<string>(), It.IsAny<NameValueCollection>()))
            .Returns(new RestRequest());
        
        autoMocker
            .GetMock<IRestClient>()
            .Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RestResponse()
            {
                Content = mockedResponse,
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true
            });
        
        // Act
        var result = await sut.GetExchangeRatesAsync(currencies);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(currencies.Count);
        
        currencies
            .All(c => result.Any(x => x.SourceCurrency.Code == c.Code))
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task GetExchangeRates_Should_Throw_Application_Exception_When_Request_Is_Not_Successful()
    {
        // Arrange
        var currencies = fixture.CreateMany<Currency>(5);
        
        autoMocker
            .GetMock<IRestRequestBuilder>()
            .Setup(b => b.Build(It.IsAny<string>(), It.IsAny<NameValueCollection>()))
            .Returns(new RestRequest());
        
        autoMocker
            .GetMock<IRestClient>()
            .Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RestResponse()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccessStatusCode = false,
                Content = string.Empty
            });

        // Act
        Func<Task> action = async () => await sut.GetExchangeRatesAsync(currencies);

        // Assert
        await action.Should().ThrowAsync<ApplicationException>();
    }
    
    [Fact]
    public async Task GetExchangeRates_Should_Return_Empty_Collection_When_No_Currencies_Are_Provided()
    {
        // Act
        var result = await sut.GetExchangeRatesAsync(Enumerable.Empty<Currency>());
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetExchangeRates_Should_Return_Empty_Collection_When_ResponseContent_Is_Empty(string? responseContent)
    {
        // Arrange
        var currencies = new List<Currency>()
        {
            new("AUD"),
            new("EUR"),
            new("GBP"),
            new("USD")
        };

        autoMocker
            .GetMock<IRestRequestBuilder>()
            .Setup(b => b.Build(It.IsAny<string>(), It.IsAny<NameValueCollection>()))
            .Returns(new RestRequest());
        
        autoMocker
            .GetMock<IRestClient>()
            .Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RestResponse()
            {
                Content = responseContent,
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true
            });
        
        // Act
        var result = await sut.GetExchangeRatesAsync(currencies);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}