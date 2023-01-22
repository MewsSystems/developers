using ExchangeRateUpdater;
using ExchangeRateUpdater.BankRatesManagers;
using ExchangeRateUpdater.Models;
using Moq;
using Tests.Helpers;

namespace Tests;

[TestClass]
public partial class ExchangeRateProviderTests
{
    private ExchangeRateProvider exchangeRateProvider;
    private readonly List<ExchangeRate> exchangeRatesFromBank = new()
    {
        new ExchangeRate(new Currency("CZ"), new Currency("USD"), 1.5m),
        new ExchangeRate(new Currency("CZ"), new Currency("BGN"), 12.23m),
        new ExchangeRate(new Currency("CZ"), new Currency("CNY"), 3.259m),
        new ExchangeRate(new Currency("CZ"), new Currency("DKK"), 3.215m)
    };

    [TestInitialize]
    public void Setup()
    {
        var handlerMock = HttpClientHelper.GetHttpMessageHandlerMock(string.Empty);
        var httpClient = new HttpClient(handlerMock.Object);

        var bankRatesMock = new Mock<IBankRatesManager>();
        bankRatesMock
            .Setup(m => m.Parse(It.IsAny<string>()))
            .Returns(exchangeRatesFromBank);
        bankRatesMock
            .Setup(m => m.GetDailyDataSourceUri())
            .Returns(new Uri("http://dummyUri.com"));

        exchangeRateProvider = new ExchangeRateProvider(httpClient, bankRatesMock.Object);
    }


    [DataTestMethod]
    public async Task GetExchangeRates_FourCurrienciesPassed_TwoCurrienciesReturned()
    {
        // arrange
        var expectedCurrencies = new List<Currency>
        {
            new Currency("USD"),
            new Currency("BGN")
        };

        // act
        var result = await exchangeRateProvider.GetExchangeRates(expectedCurrencies);

        // assert
        var expectedRates = exchangeRatesFromBank
            .Where(er => expectedCurrencies.Contains(er.TargetCurrency))
            .ToList();
        Assert.AreEqual(expectedCurrencies.Count, result.Count());
        CollectionAssert.AreEquivalent(expectedRates, result.ToList());
    }
}