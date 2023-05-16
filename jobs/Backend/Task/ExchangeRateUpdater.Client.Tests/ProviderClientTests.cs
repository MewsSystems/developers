using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Client.Client;
using ExchangeRateUpdater.Client.Exceptions;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRateUpdater.Client.Tests;

public class ProviderClientTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<DelegatingHandler> _clientHandlerMock;
    private readonly ProviderClient _sut;
    
    public ProviderClientTests()
    {
        _clientHandlerMock = new Mock<DelegatingHandler>();
        _clientHandlerMock.As<IDisposable>().Setup(s => s.Dispose());
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpClient = new HttpClient(_clientHandlerMock.Object);


        _sut = new ProviderClient(httpClient);
    }
    
    [Fact]
    public async Task GetAsync_Returns_LatestExchangeRates_WhenDateSpecified()
    {
        // arrange
        var query = "12 May 2023 #91\nCountry|Currency|Amount|Code|Rate\nEMU|euro|1|EUR|23.605\nUSA|dollar|1|USD|21.678";
        _clientHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(query, Encoding.UTF8, "text/plain")
            })
            .Verifiable();
        
        // act
        var actual = await _sut.GetAsync(new DateTime(2023, 5, 12));

        // assert
        Assert.NotEmpty(actual);
        Assert.Equal(2, actual.Count());
        
        Assert.Contains(actual, pair =>
            pair.Country == "EMU" &&
            pair.Currency == "euro" &&
            pair.Amount == 1m &&
            pair.Code == "EUR" &&
            pair.Rate == 23.605m);
        
        Assert.Contains(actual, pair =>
            pair.Country == "USA" &&
            pair.Currency == "dollar" &&
            pair.Amount == 1m &&
            pair.Code == "USD" &&
            pair.Rate == 21.678m);
    }
    
    [Fact]
    public async Task GetAsync_Throws_ExpectedException_ReceivingBadRequest()
    {
        // arrange
        _clientHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest))
            .Verifiable();
        
        // act
        var actual = await Assert.ThrowsAsync<ExchangeRateProviderException>(async () =>
            await _sut.GetAsync(new DateTime(2023, 5, 12)));

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
    }
}