using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateHttpClientTests
{
    private const string BaseUrl = "https://cnb.cz";
    
    private readonly CnbExchangeRateHttpClient _sut;
    private readonly MockHttpMessageHandler _httpHandlerMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    
    // no dependencies, just easier to use the object than to mock
    private readonly ExchangeRateParser _parser;

    public ExchangeRateHttpClientTests()
    {
        _httpHandlerMock = new MockHttpMessageHandler();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _parser = new ExchangeRateParser();

        var httpClient = _httpHandlerMock.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseUrl);
        
        _sut = new CnbExchangeRateHttpClient(httpClient, _parser, _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task FormatsUrlCorrectly()
    {
        _dateTimeProviderMock.SetupGet(x => x.Current)
            .Returns(new DateTime(2000, 1, 1));

        var expectedUrl = Path.Combine(BaseUrl,
            "/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date=01.01.2000");
        
        var mockedRequest = _httpHandlerMock.When(expectedUrl)
            .Respond(new StringContent(""));

        await _sut.GetTodaysExchangeRates(CancellationToken.None);
        
        Assert.Equal(1, _httpHandlerMock.GetMatchCount(mockedRequest));
    }
}

