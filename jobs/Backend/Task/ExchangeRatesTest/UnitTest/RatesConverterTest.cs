using ExchangeRatesService.Models;
using ExchangeRatesService.Providers.Interfaces;

namespace ExchangeRatesTest;

[TestFixture]
public class RatesConverterTest
{

    private RatesConverter _converterService;

    [SetUp]
    public void Setup()
    {
        _converterService = new RatesConverter();
    }

    [Test]
    public async Task ConvertTenEurosToKorunaWithRate025_ShouldBe2point5Korunas()
    {
        //Arrange
        var sourceCurrency = new Currency("EUR");
        var targetCurrency = new Currency("CZK");
        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, 0.25m, 1);

        //Act 
        var result = await _converterService.GetConversion(exchangeRate.SourceCurrency, exchangeRate.TargetCurrency,
            exchangeRate.Value,
            10);

        //Assert
        Assert.That(result, Is.EqualTo(2.5));
    }

    [Test]
    public async Task ReverseConvert2point5KorunaWithRate4_ShouldBe10Euros()
    {
        //Arrange
        var sourceCurrency = new Currency("CZK");
        var targetCurrency = new Currency("EUR");
        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, 4m, 1);

        //Act 
        var result = await _converterService.GetConversion(exchangeRate.SourceCurrency, exchangeRate.TargetCurrency,
            exchangeRate.Value,
            2.5m);

        //Assert
        Assert.That(result, Is.EqualTo(10));
    }


    [Test]
    public async Task Convert10ISKToKorunaWith0point025by100Rate_ShouldBe0poin025Korunas()
    {
        //Arrange
        var sourceCurrency = new Currency("ISK");
        var targetCurrency = new Currency("CZK");
        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, 0.025m, 100);

        //Act 
        var result = await _converterService.GetConversion(exchangeRate.SourceCurrency, exchangeRate.TargetCurrency,
            exchangeRate.Value,
            10);

        //Assert
        Assert.That(result, Is.EqualTo(0.0025m));
    }


    [Test]
    public async Task ReverseConvert0point00025KorunaWithRate400000_ShouldBe10ISK()
    {
        //Arrange
        var sourceCurrency = new Currency("CZK");
        var targetCurrency = new Currency("ISK");
        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, 400000m, 100);

        //Act 
        var result = await _converterService.GetConversion(exchangeRate.SourceCurrency, exchangeRate.TargetCurrency,
            exchangeRate.Value,
            0.0025m);

        //Assert
        Assert.That(result, Is.EqualTo(10));
    }
}
