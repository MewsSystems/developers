using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Services;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.Core.IntegrationTests;

public class ExchangeRateHttpClientTests
{
    private IExchangeRateHttpClient _sut;
    private Mock<IApiConfiguration> _apiConfigurationMock;
    
    [SetUp]
    public void Setup()
    {
        _apiConfigurationMock = new Mock<IApiConfiguration>();
        _sut = new ExchangeRateHttpClient(_apiConfigurationMock.Object);
    }

    [Test]
    public async Task Given_ApiConfiguration_When_GetExchangeRates_Then_Expected_ExchangeRates()
    {
        // given
        _apiConfigurationMock.SetupGet(c => c.ApiUrl)
            .Returns("https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");

        // when
        var result = await _sut.GetExchangeRates();
        
        // then
        result.Should()
            .NotBeNull()
            .And
            .NotBeEmpty();
    }
}