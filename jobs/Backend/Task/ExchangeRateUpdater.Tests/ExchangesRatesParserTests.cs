using ExchangeRateUpdater.WebApi.Services.ExchangeRateDownloader;
using ExchangeRateUpdater.WebApi.Services.ExchangeRateParser;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ExchangeRateUpdater.WebApi.Tests;

[TestClass]
public class ExchangesRatesParserTests
{
    private const string SourceCurrencyKeyName = "SourceCurrency";
    private const string SourceCurrency = "CZK";
    private const string ExchangeRatesFilename = "..\\..\\..\\Documents\\ExchangeRates.txt";

    private Mock<IConfiguration> ConfigurationMock { get; } = new();
    private Mock<IExchangeRateDownloader> ExchangeRateDownloaderMock { get; } = new();

    public ExchangesRatesParserTests()
    {
        //mock config
        var sourceCurrencySection = new Mock<IConfigurationSection>();
        sourceCurrencySection.Setup(x => x.Key).Returns(SourceCurrencyKeyName);
        sourceCurrencySection.Setup(x => x.Value).Returns(SourceCurrency);

        ConfigurationMock.Setup(c => c.GetSection(SourceCurrencyKeyName)).Returns(sourceCurrencySection.Object);

        //mock exchangeRateDownloader
        ExchangeRateDownloaderMock.Setup(x => x.DownloadExchangeRates()).Returns(() => Task.FromResult(File.ReadAllText(ExchangeRatesFilename)));
    }

    [TestMethod]
    public async Task ExchangeRatesParser_ReturnsRates()
    {
        //arrange
        var exchangeRatesParser = new ExchangeRateParser(ConfigurationMock.Object, ExchangeRateDownloaderMock.Object);
        //act
        var availableExchangeRates = await exchangeRatesParser.ParseExchangeRates();
        //assert
        Assert.IsNotNull(availableExchangeRates);
        Assert.IsTrue(availableExchangeRates.Any());
    }
}