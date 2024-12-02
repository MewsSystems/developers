namespace ExchangeRateProvider.UnitTests;

[TestClass]
public class CNBExchangeRateProviderServiceTests
{
    [TestMethod]
    [DynamicData( nameof( TestData.ReturnsCorrectExchangeRates ), typeof( TestData ), DynamicDataSourceType.Property )]
    public async Task Given_ListOfCurrencies_ReturnsExRates( IEnumerable<Currency> currencies, IEnumerable<CNBApiExchangeRateRecord> exchangeRates )
    {
        var mockIExRateRepo = new Mock<IExchangeRateProviderRepository>();
        var mockLogger = new Mock<ILogger<CNBExchangeRateProviderService>>();
        var mockConfigSection = new Mock<IConfigurationSection>();
        mockConfigSection.Setup( cs => cs.Value ).Returns( "CSK" );
        var mockIConfiguration = new Mock<IConfiguration>();
        mockIConfiguration.Setup( x => x.GetSection( It.IsAny<string>() ) ).Returns( mockConfigSection.Object );

        mockIExRateRepo.Setup( m => m.GetDailyExchangeRatesAsync().Result ).Returns( exchangeRates );
        var sut = new CNBExchangeRateProviderService( mockLogger.Object, mockIExRateRepo.Object, mockIConfiguration.Object );

        var result = await sut.GetExchangeRatesAsync( currencies );

        Assert.IsNotNull( result );
    }
}