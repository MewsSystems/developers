using Castle.Core.Logging;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Constants;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Mappers;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateProviderService.UnitTests.CNBExchangeRateProviderServiceUnitTests.Mappers;

[TestClass]
public class UnitTestExchangeRateJsonToDtoMapper
{
    private Mock<ILogger<ExchangeRateJsonToDtoMapper>> _logger;

    private ExchangeRateJsonToDtoMapper _exchangeRateMapper = default!;

    private readonly string ValidExchangeRatesTestData = "ValidExchangeRatesTestData.json";
    private readonly string InvalidExchangeRatesTestData = "FakeTestData";

    [TestInitialize]
    public void BeforeEach()
    {
        _logger = new Mock<ILogger<ExchangeRateJsonToDtoMapper>>();
        _exchangeRateMapper = new ExchangeRateJsonToDtoMapper(_logger.Object);
    }

    [TestMethod]
    public void TestExchangeRateDtoMapper_ValidJsonTestData_ReturnsExchangeRates()
    {
        var expectedExchangeRates = CreateExpectedExchangeRates();
        var exchangeRatesJson = File.ReadAllText(
            GetTestDataFilePath(ValidExchangeRatesTestData));

        var exchangeRates = _exchangeRateMapper.ExchangeRateDtoMapper(exchangeRatesJson);

        Assert.IsNotNull(exchangeRates);
        Assert.AreEqual(2, exchangeRates.Count());

        AssertExchangeRateEqual(exchangeRates.ElementAt(0), expectedExchangeRates[0]);
        AssertExchangeRateEqual(exchangeRates.ElementAt(1), expectedExchangeRates[1]);
    }

    [TestMethod]
    public void TestExchangeRateDtoMapper_InvalidJsonTestData_ReturnsEmpty()
    {
        var exchangeRatesJson = InvalidExchangeRatesTestData;

        var exchangeRates = _exchangeRateMapper.ExchangeRateDtoMapper(exchangeRatesJson);

        Assert.IsNotNull(exchangeRates);
        Assert.AreEqual(0, exchangeRates.Count());
    }

    private static string GetTestDataFilePath(string fileName)
    {
        string testDataDirectory = Path.Combine(
            Directory.GetCurrentDirectory(), "CNBExchangeRateProviderServiceUnitTests", "TestData");
        return Path.Combine(testDataDirectory, fileName);
    }

    private void AssertExchangeRateEqual(ExchangeRateDto expected, ExchangeRateDto actual)
    {
        Assert.AreEqual(expected.BaseCurrency, actual.BaseCurrency);
        Assert.AreEqual(expected.TargetCurrency, actual.TargetCurrency);
        Assert.AreEqual(expected.Rate, actual.Rate);
        Assert.AreEqual(expected.Date, actual.Date);
    }

    private ExchangeRateDto CreateExchangeRateDto(
        CurrencyDto baseCurrency, CurrencyDto targetCurrency, decimal rate, DateTime date)
    {
        return new ExchangeRateDto
        {
            BaseCurrency = baseCurrency,
            TargetCurrency = targetCurrency,
            Rate = rate,
            Date = date,
        };
    }

    private CurrencyDto CreateCurrency(string currencyCode)
    {
        return new CurrencyDto
        {
            Code = currencyCode,
        };
    }

    private List<ExchangeRateDto> CreateExpectedExchangeRates()
    {
        return new List<ExchangeRateDto>()
        {
            CreateExchangeRateDto(
                Defaults.CURRENCY.BaseCurrency, CreateCurrency("AUD"),
                new Decimal(13.899), new DateTime(2025, 04, 08)),
            CreateExchangeRateDto(
                Defaults.CURRENCY.BaseCurrency, CreateCurrency("BRL"),
                new Decimal(3.909), new DateTime(2025, 04, 08)),
        };
    }
}
