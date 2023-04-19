using ExchangeRateUpdater.WebApi.Controllers;
using ExchangeRateUpdater.WebApi.Models;
using ExchangeRateUpdater.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Controllers
{
    public class ExchangeRateUpdaterControllerTests
    {
        private Mock<IExchangeRateProvider> _exchangeRateProviderMock;
        private ExchangeRateUpdaterController _exchangeRateUpdaterController;
        private IEnumerable<Currency> _inputCurrencies;
        private IEnumerable<Currency> _inputCurrenciesWithNonExistentCurrency;
        private IEnumerable<ExchangeRate> _exchangeRatesFromProvider;

        [SetUp]
        public void Setup()
        {
            _inputCurrencies = new List<Currency>() {
                new Currency("EUR"),
                new Currency("USD"),
                new Currency("NOK")
            };

            _inputCurrenciesWithNonExistentCurrency = new List<Currency>() {
                new Currency("CAD")
            };

            _exchangeRatesFromProvider = new List<ExchangeRate> {
                new ExchangeRate(new Currency("CZK"), new Currency("NOK"), Convert.ToDecimal(2.039)),
                new ExchangeRate(new Currency("CZK"), new Currency("ZAR"), Convert.ToDecimal(1.173)),
                new ExchangeRate(new Currency("CZK"), new Currency("USD"), Convert.ToDecimal(21.312))

            };
            _exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
            _exchangeRateProviderMock.Setup(exchangeRateProvider => exchangeRateProvider.GetExchangeRates(_inputCurrencies)).Returns(() => Task.FromResult(_exchangeRatesFromProvider));
            _exchangeRateUpdaterController = new ExchangeRateUpdaterController(_exchangeRateProviderMock.Object);
        }

        [Test]
        public async Task ExchangeRateUpdaterController_ShouldGetProperResponseResult_WhenCalledWithProperCurrencyList()
        {
            var controllerResponse = await _exchangeRateUpdaterController.GetExchangeRates(_inputCurrencies);

            var controllerResponseResult = controllerResponse.Result as OkObjectResult;
            var exchangeRates = controllerResponseResult.Value as IEnumerable<ExchangeRate>;

            Assert.That(exchangeRates, Is.EqualTo(_exchangeRatesFromProvider));
        }

        [Test]
        public async Task ExchangeRateUpdaterController_ShouldReturnEmptyExchangeRates_WhenNonExistentCurrency()
        {
            var controllerResponse = await _exchangeRateUpdaterController.GetExchangeRates(_inputCurrenciesWithNonExistentCurrency);

            var controllerResponseResult = controllerResponse.Result as OkObjectResult;
            var exchangeRates = controllerResponseResult.Value as IEnumerable<ExchangeRate>;

            Assert.That(exchangeRates, Is.Empty);
        }
    }
}