using System.Globalization;
using ExchangeRateUpdater.Application.GetExchangeRates;
using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.Infrastructure.Common;
using ExchangeRateUpdater.IntegrationTests.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Testcontainers.Redis;

namespace ExchangeRateUpdater.IntegrationTests.Api.Controllers.ExchangeRates;

public class GetExchangeRatesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly Mock<IRestClient> _restClientMock = new();
    private const string EurRate = "25.055";
    private const string UsdRate = "22.541";
    private const string HttpServiceResponse = $"28 Aug 2024 #167\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|15.270\nBrazil|real|1|BRL|4.070\nBulgaria|lev|1|BGN|12.811\nCanada|dollar|1|CAD|16.727\nChina|renminbi|1|CNY|3.162\nDenmark|krone|1|DKK|3.359\nEMU|euro|1|EUR|{EurRate}\nHongkong|dollar|1|HKD|2.890\nHungary|forint|100|HUF|6.368\nIceland|krona|100|ISK|16.365\nIMF|SDR|1|XDR|30.418\nIndia|rupee|100|INR|26.843\nIndonesia|rupiah|1000|IDR|1.461\nIsrael|new shekel|1|ILS|6.147\nJapan|yen|100|JPY|15.598\nMalaysia|ringgit|1|MYR|5.190\nMexico|peso|1|MXN|1.144\nNew Zealand|dollar|1|NZD|14.057\nNorway|krone|1|NOK|2.139\nPhilippines|peso|100|PHP|40.065\nPoland|zloty|1|PLN|5.826\nRomania|leu|1|RON|5.034\nSingapore|dollar|1|SGD|17.288\nSouth Africa|rand|1|ZAR|1.267\nSouth Korea|won|100|KRW|1.687\nSweden|krona|1|SEK|2.210\nSwitzerland|franc|1|CHF|26.712\nThailand|baht|100|THB|66.168\nTurkey|lira|100|TRY|66.222\nUnited Kingdom|pound|1|GBP|29.773\nUSA|dollar|1|USD|{UsdRate}";

    public GetExchangeRatesTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }

    [Fact]
    public async Task GetExchangeRates_When_requested_once_Then_response_is_returned_from_http_as_expected()
    {
        //Arrange
        var request = new GetExchangeRatesQuery
        {
            CurrencyCodes = ["EUR", "USD"],
            Date = new DateOnly(2024, 08, 29)
        };
        var httpClient = await GetHttpClientAsync();
        
        //Act
        var result = await httpClient.PostAsync<GetExchangeRatesQuery, GetExchangeRatesResponse?>(ExchangeRatesApiRoutes.Post.GetExchangeRates, request);

        //Assert
        Assert.Equal(2, result!.ExchangeRates.Count);
        var eurResult = result.ExchangeRates.Single(r => r.SourceCurrency == "EUR");
        Assert.Equal(decimal.Parse(EurRate, CultureInfo.InvariantCulture), eurResult.Rate);
        var usdResult = result.ExchangeRates.Single(r => r.SourceCurrency == "USD");
        Assert.Equal(decimal.Parse(UsdRate, CultureInfo.InvariantCulture), usdResult.Rate);
        _restClientMock.Verify(r => r.GetAsync<string?>(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task GetExchangeRates_When_requested_twice_Then_second_response_is_returned_from_redis_as_expected()
    {
        //Arrange
        var request = new GetExchangeRatesQuery
        {
            CurrencyCodes = ["EUR", "USD"],
            Date = new DateOnly(2024, 08, 29)
        };
        var httpClient = await GetHttpClientAsync();
        
        //Act
        var result = await httpClient.PostAsync<GetExchangeRatesQuery, GetExchangeRatesResponse?>(ExchangeRatesApiRoutes.Post.GetExchangeRates, request);
        result = await httpClient.PostAsync<GetExchangeRatesQuery, GetExchangeRatesResponse?>(ExchangeRatesApiRoutes.Post.GetExchangeRates, request);

        //Assert
        Assert.Equal(2, result!.ExchangeRates.Count);
        var eurResult = result.ExchangeRates.Single(r => r.SourceCurrency == "EUR");
        Assert.Equal(decimal.Parse(EurRate, CultureInfo.InvariantCulture), eurResult.Rate);
        var usdResult = result.ExchangeRates.Single(r => r.SourceCurrency == "USD");
        Assert.Equal(decimal.Parse(UsdRate, CultureInfo.InvariantCulture), usdResult.Rate);
        _restClientMock.Verify(r => r.GetAsync<string?>(It.IsAny<string>()), Times.Once);
    }

    private async Task<HttpClient> GetHttpClientAsync()
    {
        _restClientMock.Invocations.Clear();
        _restClientMock //Mocking an external dependency in an integration test is not ideal. It would be better to have a Testcontainer with the service contained in it, so we can really test the connection to an external dependency.
            .Setup(r => r.GetAsync<string>(It.IsAny<string>()))
            .ReturnsAsync(HttpServiceResponse);

        var redisContainer = new RedisBuilder().Build();
        await redisContainer.StartAsync();
        
        var httpClient = _webApplicationFactory
            .WithWebHostBuilder(builder => builder.ConfigureTestServices(testServices =>
            {
                testServices.AddScoped(_ => _restClientMock.Object);
                var redisConnectionStringBuilderMock = new Mock<IRedisConnectionStringBuilder>();
                redisConnectionStringBuilderMock.Setup(r => r.Build()).Returns(redisContainer.GetConnectionString());
                testServices.AddSingleton(redisConnectionStringBuilderMock.Object);
            }))
            .CreateClient();

        return httpClient;
    }
}