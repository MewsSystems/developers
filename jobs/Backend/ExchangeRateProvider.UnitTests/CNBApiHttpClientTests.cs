namespace ExchangeRateProvider.UnitTests;

[TestClass]
public class CNBApiHttpClientTests
{
    private HttpClient _httpClient;
    private Mock<HttpMessageHandler> _mockMessageHandler;
    private Mock<ILogger<CNBApiHttpClient>> _mockLogger;
    private Mock<IHttpRetryPolicy> _mockRetryPolicy;

    private CNBApiHttpClient _systemUnderTest;

    [TestInitialize]
    public void TestInitialise()
    {
        _mockLogger = new Mock<ILogger<CNBApiHttpClient>>();
        _mockRetryPolicy = new Mock<IHttpRetryPolicy>();
        _mockMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient( _mockMessageHandler.Object ) { BaseAddress = new Uri( "http://www.test.com" ), Timeout = TimeSpan.FromMilliseconds( 300 ) };

        _systemUnderTest = new CNBApiHttpClient( _mockLogger.Object, _httpClient, _mockRetryPolicy.Object );
    }

    [TestMethod]
    [DynamicData( nameof( TestData.ApiRequestsAndResponses ), typeof( TestData ) )]
    public async Task GetDailyExchangeRatesAsync_ReturnsExchangeRates( string jsonResponse, HttpStatusCode statusCode, bool isNotNull, int? ratesCount )
    {
        var policy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
            .WaitAndRetryAsync( 0, times => TimeSpan.Zero, onRetry: ( _, _ ) => { } )
            .WithPolicyKey( It.IsAny<string>() );

        _mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>() )
            .ReturnsAsync( new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new StringContent( jsonResponse, Encoding.UTF8, "application/json" )
            } )
            .Verifiable();

        _httpClient = new HttpClient( _mockMessageHandler.Object );

        _mockRetryPolicy.Setup( policy => policy.CNBHttpPolicy ).Returns( policy );

        var result = await _systemUnderTest.GetDailyExchangeRatesAsync();

        Assert.AreEqual( isNotNull, result != null );
        Assert.AreEqual( ratesCount, result?.Count() );
        _mockMessageHandler.Protected().Verify( "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>( req => req.Method == HttpMethod.Get ),
            ItExpr.IsAny<CancellationToken>() );
    }

    [TestMethod]
    [DynamicData( nameof( TestData.ApiRequestsAndResponsesRetryPolicy ), typeof( TestData ) )]
    public async Task GetDailyExchangeRatesAsync_ThreeRetries( HttpStatusCode statusCode, bool isNotNull, int? ratesCount )
    {
        int retryCount = 0;
        int maxRetries = 3;
        var policy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
            .WaitAndRetryAsync( maxRetries, times => TimeSpan.Zero, onRetry: ( _, _ ) => { retryCount++; } )
            .WithPolicyKey( It.IsAny<string>() );

        _mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>() )
            .ReturnsAsync( () => throw new HttpRequestException() )
            .Verifiable();

        _httpClient = new HttpClient( _mockMessageHandler.Object );

        _mockRetryPolicy.Setup( policy => policy.CNBHttpPolicy ).Returns( policy );

        var result = await _systemUnderTest.GetDailyExchangeRatesAsync();

        Assert.AreEqual( maxRetries, retryCount );
        _mockMessageHandler.Protected().Verify( "SendAsync",
            Times.Exactly( maxRetries + 1 ),
            ItExpr.Is<HttpRequestMessage>( req => req.Method == HttpMethod.Get ),
            ItExpr.IsAny<CancellationToken>() );
    }
}