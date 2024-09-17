using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using FluentAssertions;
using Mews.ExchangeRate.API.Controllers;
using Mews.ExchangeRate.Domain;
using Mews.ExchangeRate.Provider.CNB;
using Mews.ExchangeRate.Provider.CNB.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Mews.ExchangeRate.API.IntegrationTests.Controllers;
public class ExchangeRateControllerTests
{
    private Fixture _fixture;

    public ExchangeRateControllerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        //TODO: Cleanup arrange stage by using Autofixture Customizations like in other projects
        var mapper = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new Provider.CNB.Mapper.DtoToDomainProfile());
            mc.AddProfile(new API.Mappers.DtoToDomainProfile());
            mc.AddProfile(new API.Mappers.DomainToDtoProfile());
        }).CreateMapper();
        _fixture.Inject(mapper);
    }

    [Fact]
    public async Task Post_ReturnsEURExchangeRate_WhenPassingEURCurrencyAsParameter()
    {
        //TODO: Cleanup arrange stage by using Autofixture Customizations like it has been done in other projects
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

        IRetrieveExchangeRatesFromSource cnbExchangeRateProvider = _fixture.Freeze<CnbExchangeRateProvider>();
        _fixture.Inject(cnbExchangeRateProvider);

        IProvideExchangeRates exchangeRateProvider = _fixture.Freeze<ExchangeRateProvider>();
        _fixture.Inject(exchangeRateProvider);

        // --------- 

        var sut = new ExchangeRateController(_fixture.Create<ILogger<ExchangeRateController>>(),
            _fixture.Create<IProvideExchangeRates>(),
            _fixture.Create<IMapper>());
        var result =await  sut.Post(new List<Dtos.Currency>() { new Dtos.Currency("EUR") });
        result.Should().BeOfType<OkObjectResult>();

        var okResult = (OkObjectResult)result;
        ((IEnumerable<Dtos.ExchangeRate>)okResult.Value).Count().Should().Be(1);

        var eurExchangeRate = ((IEnumerable<Dtos.ExchangeRate>)okResult.Value).ElementAt(0);
        eurExchangeRate.Should().Match<Dtos.ExchangeRate>(e => e.SourceCurrency.Code.Equals("EUR") || e.TargetCurrency.Code.Equals("EUR"));
    }

    // TODO
    // write tests for exception handling
    // write tests for expected http responses
}
