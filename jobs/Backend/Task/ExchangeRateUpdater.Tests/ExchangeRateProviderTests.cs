using ExchangeRateUpdater.WebApi.Models;
using ExchangeRateUpdater.WebApi.Services.ExchangeRateParser;
using ExchangeRateUpdater.WebApi.Services.ExchangeRateProvider;
using Moq;

namespace ExchangeRateUpdater.WebApi.Tests;

[TestClass]
public class ExchangeRateProviderTests
{
    private Mock<IExchangeRateParser> ExchangeRateParserMock { get; } = new();
    private const string SourceCurrency = "CZK";

    private readonly IEnumerable<Currency> _currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    private readonly IEnumerable<Currency> _emptyCurrencies = new Currency[]
    {
    };

    private readonly IEnumerable<ExchangeRate> _exchangeRates = new List<ExchangeRate>
            {
                new(new Currency(SourceCurrency), new Currency("AUD"), new decimal(15.408)),
                new(new Currency(SourceCurrency), new Currency("BRL"), new decimal(4.326)),
                new(new Currency(SourceCurrency), new Currency("BGN"), new decimal(12.278)),
                new(new Currency(SourceCurrency), new Currency("CAD"), new decimal(16.569)),
                new(new Currency(SourceCurrency), new Currency("CNY"), new decimal(3.303)),
                new(new Currency(SourceCurrency), new Currency("DKK"), new decimal(3.228)),
                new(new Currency(SourceCurrency), new Currency("EUR"), new decimal(24.015)),
                new(new Currency(SourceCurrency), new Currency("HKD"), new decimal(2.843)),
                new(new Currency(SourceCurrency), new Currency("HUF"), new decimal(6.052)),
                new(new Currency(SourceCurrency), new Currency("ISK"), new decimal(15.564)),
                new(new Currency(SourceCurrency), new Currency("XDR"), new decimal(29.850)),
                new(new Currency(SourceCurrency), new Currency("INR"), new decimal(27.256)),
                new(new Currency(SourceCurrency), new Currency("IDR"), new decimal(1.466)),
                new(new Currency(SourceCurrency), new Currency("ILS"), new decimal(6.485)),
                new(new Currency(SourceCurrency), new Currency("JPY"), new decimal(17.275)),
                new(new Currency(SourceCurrency), new Currency("MYR"), new decimal(5.121)),
                new(new Currency(SourceCurrency), new Currency("MXN"), new decimal(1.175)),
                new(new Currency(SourceCurrency), new Currency("NZD"), new decimal(14.117)),
                new(new Currency(SourceCurrency), new Currency("NOK"), new decimal(2.245)),
                new(new Currency(SourceCurrency), new Currency("PHP"), new decimal(40.399)),
                new(new Currency(SourceCurrency), new Currency("PLN"), new decimal(5.122)),
                new(new Currency(SourceCurrency), new Currency("RON"), new decimal(4.859)),
                new(new Currency(SourceCurrency), new Currency("SGD"), new decimal(16.782)),
                new(new Currency(SourceCurrency), new Currency("ZAR"), new decimal(1.316)),
                new(new Currency(SourceCurrency), new Currency("KRW"), new decimal(1.788)),
                new(new Currency(SourceCurrency), new Currency("SEK"), new decimal(2.134)),
                new(new Currency(SourceCurrency), new Currency("CHF"), new decimal(23.893)),
                new(new Currency(SourceCurrency), new Currency("THB"), new decimal(67.163)),
                new(new Currency(SourceCurrency), new Currency("TRY"), new decimal(1.182)),
                new(new Currency(SourceCurrency), new Currency("GBP"), new decimal(27.043)),
                new(new Currency(SourceCurrency), new Currency("USD"), new decimal(22.206))
            };

    public ExchangeRateProviderTests()
    {
        ExchangeRateParserMock.Setup(x => x.ParseExchangeRates()).Returns(() => Task.FromResult(_exchangeRates));
    }

    [TestMethod]
    public async Task ExchangeRates_WithCurrencies_ReturnsRates()
    {
        //arrange
        var exchangeRatesProvider = new ExchangeRateProvider(ExchangeRateParserMock.Object);
        //act
        var serviceResponse = await exchangeRatesProvider.GetExchangeRates(_currencies);

        //assert
        Assert.IsTrue(serviceResponse.Success);
        Assert.IsTrue(serviceResponse.Data!.Any());
    }

    [TestMethod]
    public async Task ExchangeRates_EmptyCurrencies_ReturnsEmptyRates()
    {
        //arrange
        var exchangeRatesProvider = new ExchangeRateProvider(ExchangeRateParserMock.Object);
        //act
        var serviceResponse = await exchangeRatesProvider.GetExchangeRates(_emptyCurrencies);

        //assert
        Assert.IsTrue(serviceResponse.Success);
        Assert.IsFalse(serviceResponse.Data!.Any());
    }
    

}