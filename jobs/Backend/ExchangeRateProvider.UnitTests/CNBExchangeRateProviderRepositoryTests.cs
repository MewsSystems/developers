namespace ExchangeRateProvider.UnitTests;

[TestClass]
public class CNBExchangeRateProviderRepositoryTests
{
    [TestMethod]
    [DynamicData( nameof( TestData.ReturnsDailyExchangeRates ), typeof( TestData ), DynamicDataSourceType.Property )]
    public async Task GivenCurrenciy_GetExchangeRatesAsync_ReturnsExRates(IEnumerable<Currency> currencies, IEnumerable<CNBApiExchangeRateRecord> exchangeRates )
    {
        var mockLogger = new Mock<ILogger<CNBExchangeRateProviderRepository>>();
        var mockApiHttpClient = new Mock<IApiHttpClient>();
        mockApiHttpClient.Setup( m => m.GetDailyExchangeRatesAsync().Result ).Returns( exchangeRates );

        var sut = new CNBExchangeRateProviderRepository( mockLogger.Object, mockApiHttpClient.Object );

        var result = await sut.GetDailyExchangeRatesAsync();

        Assert.IsNotNull( result );
        mockApiHttpClient.Verify( m => m.GetDailyExchangeRatesAsync(), Times.Once );
    }
}