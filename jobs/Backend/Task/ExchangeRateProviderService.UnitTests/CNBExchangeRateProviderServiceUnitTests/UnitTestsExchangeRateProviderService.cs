using ExchangeRateProviderService.CNBExchangeRateProviderService.Client;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Mappers;
using FluentValidation;
using Moq;

namespace ExchangeRateProviderService.UnitTests.CNBExchangeRateProviderServiceUnitTests;

[TestClass]
public class UnitTestsExchangeRateProviderService
{
    private Mock<IApiClient> _apiClientMock = default!;
    private Mock<IExchangeRateJsonToDtoMapper> _exchangeRateMapperMock = default!;
    private InlineValidator<ExchangeRateDto> _exchangeRateModelValidator = default!;

    private CNBExchangeRateProviderService.ExchangeRateProviderService _exchangeRateProviderService = default!;

    private readonly string FakeJsonString = "FakeData";
    private readonly string FakeErrorMessage = "FakeErrorMessage";

    [TestInitialize]
    public void BeforeEach()
    {
        _apiClientMock = new Mock<IApiClient>();
        _exchangeRateMapperMock = new Mock<IExchangeRateJsonToDtoMapper>();
        _exchangeRateModelValidator = new InlineValidator<ExchangeRateDto>();

        _exchangeRateProviderService = new CNBExchangeRateProviderService.ExchangeRateProviderService(
            _apiClientMock.Object, _exchangeRateMapperMock.Object, _exchangeRateModelValidator);
    }

    [TestMethod]
    public async Task TestGetExchangeRatesAsync_CurrenciesIsEmtpy_MustNotCallApiClient()
    {
        var currencies = Enumerable.Empty<CurrencyDto>();

        await _exchangeRateProviderService.GetExchangeRatesAsync(currencies);

        _apiClientMock.Verify(
            x => x.GetDailyRatesJson(), Times.Never);
    }

    [TestMethod]
    public async Task TestGetExchangeRatesAsync_CurrenciesIsNotEmpty_MustCallApiClient()
    {
        var currencyCodes = new List<string> { "1", "2", "3" };
        var currencies = CreateCurrenciesEnumerable(currencyCodes);

        await _exchangeRateProviderService.GetExchangeRatesAsync(currencies);

        _apiClientMock.Verify(
            x => x.GetDailyRatesJson(), Times.Once);
    }

    [TestMethod]
    public async Task TestGetExchangeRatesAsync_APIClientDoesNotReturnData_MustNotCallExchangeRateMapper()
    {
        var currencyCodes = new List<string> { "1", "2", "3" };
        var currencies = CreateCurrenciesEnumerable(currencyCodes);

        MockApiClient(string.Empty);

        await _exchangeRateProviderService.GetExchangeRatesAsync(currencies);

        _exchangeRateMapperMock.Verify(
            x => x.ExchangeRateDtoMapper(It.IsAny<string>()), Times.Never());
    }

    [TestMethod]
    public async Task TestGetExchangeRatesAsync_APIClientReturnsData_MustCallExchangeRateMapper()
    {
        var currencyCodes = new List<string> { "1", "2", "3" };
        var currencies = CreateCurrenciesEnumerable(currencyCodes);

        MockApiClient(FakeJsonString);

        await _exchangeRateProviderService.GetExchangeRatesAsync(currencies);

        _exchangeRateMapperMock.Verify(
            x => x.ExchangeRateDtoMapper(FakeJsonString), Times.Once());
    }

    [TestMethod]
    public async Task TestGetExchangeRatesAsync_NoMappedExchangeRates_ReturnsEmpty()
    {
        var currencyCodes = new List<string> { "1", "2", "3" };
        var currencies = CreateCurrenciesEnumerable(currencyCodes);

        MockApiClient(FakeJsonString);
        MockExchangeRateMapper(It.IsAny<string>(), Enumerable.Empty<ExchangeRateDto>());

        var exchangeRatesResult = await _exchangeRateProviderService.GetExchangeRatesAsync(currencies);

        Assert.AreEqual(0, exchangeRatesResult.Count());
    }

    [TestMethod]
    public async Task TestGetExchangeRatesAsync_MappedExchangeRatesDoNotPassValidation_ReturnsEmpty()
    {
        var baseCurrency = CreateCurrency("0");
        var currencyCodes = new List<string> { "1", "2", "3" };
        var currencies = CreateCurrenciesEnumerable(currencyCodes);
        var exchangeRates = CreateExchangeRatesEnumerable(baseCurrency, currencies);

        MockApiClient(FakeJsonString);
        MockExchangeRateMapper(FakeJsonString, exchangeRates);
        MockValidatorFailure();

        var exchangeRatesResult = await _exchangeRateProviderService.GetExchangeRatesAsync(currencies);

        Assert.AreEqual(0, exchangeRatesResult.Count());
    }

    [TestMethod]
    public async Task TestGetExchangeRatesAsync_MappedExchangeRatesPassValidation_ReturnsAllExchangeRates()
    {
        var baseCurrency = CreateCurrency("0");
        var currencyCodes = new List<string> { "1", "2", "3" };
        var currencies = CreateCurrenciesEnumerable(currencyCodes);
        var exchangeRates = CreateExchangeRatesEnumerable(baseCurrency, currencies);

        MockApiClient(FakeJsonString);
        MockExchangeRateMapper(FakeJsonString, exchangeRates);

        var exchangeRatesResult = await _exchangeRateProviderService.GetExchangeRatesAsync(currencies);

        Assert.AreEqual(exchangeRates.Count(), exchangeRatesResult.Count());
    }

    [TestMethod]
    public async Task TestGetExchangeRatesAsync_ExchangeRatesReturnedNotInRequestedCurrencies_ReturnsOnlyExchangeRatesRequested()
    {
        var baseCurrency = CreateCurrency("0");
        
        var currencyCodesShort = new List<string> { "1", "2" };
        var currencyCodesLong = new List<string> { "1", "2", "3", "4" };
        
        var currenciesShort = CreateCurrenciesEnumerable(currencyCodesShort);
        var currenciesLong = CreateCurrenciesEnumerable(currencyCodesLong);

        var exchangeRatesShort = CreateExchangeRatesEnumerable(baseCurrency, currenciesShort);
        var exchangeRatesLong = CreateExchangeRatesEnumerable(baseCurrency, currenciesLong);

        MockApiClient(FakeJsonString);
        MockExchangeRateMapper(FakeJsonString, exchangeRatesLong);

        var exchangeRatesResult = await _exchangeRateProviderService.GetExchangeRatesAsync(currenciesShort);

        Assert.AreEqual(exchangeRatesShort.Count(), exchangeRatesResult.Count());

        for (int i = 0; i < exchangeRatesShort.Count(); i++)
        {
            AssertExchangeRatesAreEqual(exchangeRatesShort.ElementAt(i), exchangeRatesResult.ElementAt(i));
        }
    }

    private void AssertExchangeRatesAreEqual(ExchangeRateDto expected, ExchangeRateDto actual)
    {
        Assert.AreEqual(expected.BaseCurrency, actual.BaseCurrency);
        Assert.AreEqual(expected.TargetCurrency, actual.TargetCurrency);
        Assert.AreEqual(expected.Rate, actual.Rate);
        Assert.AreEqual(expected.Date, actual.Date);
    }

    private void MockApiClient(string returnValue)
    {
        _apiClientMock
            .Setup(x => x.GetDailyRatesJson().Result)
            .Returns(returnValue);
    }

    private void MockExchangeRateMapper(
        string jsonContent, IEnumerable<ExchangeRateDto> exchangeRates)
    {
        _exchangeRateMapperMock
            .Setup(x => x.ExchangeRateDtoMapper(jsonContent))
            .Returns(exchangeRates);
    }

    private void MockValidatorFailure()
    {
        _exchangeRateModelValidator.RuleFor(exchangeRate => exchangeRate.BaseCurrency)
            .Custom((value, context) => context.AddFailure(FakeErrorMessage));
    }

    private static CurrencyDto CreateCurrency(string currencyCode)
    {
        return new CurrencyDto
        {
            Code = currencyCode,
        };
    }

    private static ExchangeRateDto CreateExchangeRate(
        CurrencyDto baseCurrency, CurrencyDto targetCurrency)
    {
        return new ExchangeRateDto
        {
            BaseCurrency = baseCurrency,
            TargetCurrency = targetCurrency,
        };
    }

    private static IEnumerable<CurrencyDto> CreateCurrenciesEnumerable(IEnumerable<string> currencyCodes)
    {
        foreach (string currencyCode in currencyCodes)
        {
            yield return CreateCurrency(currencyCode);
        }
    }

    private static IEnumerable<ExchangeRateDto> CreateExchangeRatesEnumerable(
        CurrencyDto baseCurrency, IEnumerable<CurrencyDto> targetCurrencies)
    {
        foreach (var currency in targetCurrencies)
        {
            yield return CreateExchangeRate(baseCurrency, currency);
        }
    }
}
