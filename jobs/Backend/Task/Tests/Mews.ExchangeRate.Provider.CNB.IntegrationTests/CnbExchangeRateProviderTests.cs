using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using FluentAssertions;
using Mews.ExchangeRate.Provider.CNB.Configuration;
using Mews.ExchangeRate.Provider.CNB.Mapper;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Mews.ExchangeRate.Provider.CNB.UnitTests;
public class CnbExchangeRateProviderTests
{
    private Fixture _fixture;

    public CnbExchangeRateProviderTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        var mapper = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new DtoToDomainProfile());
        }).CreateMapper();
        _fixture.Inject(mapper);
    }

    [Fact]
    public async Task GetAllExchangeRatesAsync_ReturnsExchangeRates_WhenSourceReturnsExchangeRateInformation()
    {
        var settings = _fixture.Build<CnbSettings>()
            .With(x => x.ExratesEndpoint, "http://www.contoso.com")
            .Create();
        var options = _fixture.Freeze<IOptions<CnbSettings>>();
        options.Value
            .Returns(settings);

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(settings.ExratesEndpoint)
            .Respond("application/json", "{\"rates\":[{\"validFor\":\"2019-05-17\",\"order\":94,\"country\":\"Spain\",\"currency\":\"euro\",\"amount\": 1,\"currencyCode\": \"EUR\",\"rate\":15.858}]}");

        var httpClientFactoryMock = _fixture.Freeze<IHttpClientFactory>();
        httpClientFactoryMock.CreateClient(settings.ExratesEndpoint)
            .Returns(mockHttp.ToHttpClient());

        var sut = _fixture.Create<CnbExchangeRateProvider>();

        var result = await sut.GetAllExchangeRatesAsync();
        result.Count()
            .Should()
            .Be(1);

        result.ElementAt(0).SourceCurrency.Code.Should().Be("EUR");
    }

    // TODO 
    // public async Task GetAllExchangeRatesAsync_ReturnsEmptyCollection_WhenSourceReturnsEmptyCollection()
    // public async Task GetAllExchangeRatesAsync_ReturnsEmptyCollection_WhenSourceReturnsExchangeRateInformationWithInvalidDates()
}
