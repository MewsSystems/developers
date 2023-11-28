using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Services;

namespace ExchangeRateUpdater.Core.IntegrationTests;

public class ExchangeRateHttpClientTests
{
    private IExchangeRateHttpClient _sut = null!;
    private Mock<IApiConfiguration> _apiConfigurationMock = null!;
    
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
            .NotBeEmpty()
            .And
            .HaveCount(31)
            .And
            .AllSatisfy(r =>
            {
                r.SourceCurrency.Should().NotBeNull();
                r.SourceCurrency.Code.Should().NotBeNullOrEmpty();
                r.TargetCurrency.Should().NotBeNull();
                r.TargetCurrency.Code.Should().NotBeNullOrEmpty();
                r.Value.Should().NotBe(0);
            });
    }
}