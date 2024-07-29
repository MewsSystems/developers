using AutoFixture;
using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.DTO;
using ExchangeRateUpdater.Core.ServiceContracts.CurrencySource;
using ExchangeRateUpdater.Core.ServiceContracts.ExchangeRate;
using ExchangeRateUpdater.Core.Services.ExchangeRate;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Serilog;
using System.Diagnostics.Metrics;

namespace ExchangeRateUpdater.ServiceTests
{
    public class ExchangeRateServiceTest
    {
        private readonly IExchangeRateGetService _exchangeRateGetService;

        private readonly Mock<IExchangeRateRepository> _exchangeRateRepositoryMock;
        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly Mock<ICurrencySourceGetService> _currencySourceGetServiceMock;

        private readonly IFixture _fixture;

        private readonly string DEFAULT_CURRENCY_CODE = "CZK";
        private readonly string DEFAULT_API_URL = "www.test.com";

        public ExchangeRateServiceTest() 
        {
            _fixture = new Fixture();

            _exchangeRateRepositoryMock = new Mock<IExchangeRateRepository>();
            _exchangeRateRepository = _exchangeRateRepositoryMock.Object;

            _currencySourceGetServiceMock = new Mock<ICurrencySourceGetService>();

            var loggerMock = new Mock<ILogger<ExchangeRatesGetService>>();
            var diagnosticContextMock = new Mock<IDiagnosticContext>();
            _exchangeRateGetService = new ExchangeRatesGetService(loggerMock.Object, diagnosticContextMock.Object, _exchangeRateRepository, _currencySourceGetServiceMock.Object);
        }

        #region GetExchangeRates
        [Fact]
        public async void GetExchangeRates_IsEmptyList()
        {
            //Arrange
            List<ExchangeRate> empty_list = new List<ExchangeRate>();
            _exchangeRateRepositoryMock.Setup(temp => temp.GetExchangeRatesAsync(DEFAULT_CURRENCY_CODE, DEFAULT_API_URL)).ReturnsAsync(empty_list);

            List<CurrencySourceResponse> currency_source_list = new List<CurrencySourceResponse>() {
                _fixture.Build<CurrencySourceResponse>().Create()
            };
            _currencySourceGetServiceMock.Setup(x => x.GetAllCurrencySources()).ReturnsAsync(currency_source_list);

            //Act
            IEnumerable<ExchangeRateResponse> actual_exchange_rate_response_list = await _exchangeRateGetService.GetExchangeRates();

            //Assert
            actual_exchange_rate_response_list.Should().BeEmpty();
        }


        [Fact]
        public async Task GetExchangeRates_HasExchangeRates()
        {
            //Arrange
            List<ExchangeRate> exchange_rate_list = new List<ExchangeRate>() {
                _fixture.Build<ExchangeRate>().Create(),
                _fixture.Build<ExchangeRate>().Create()
            };

            List<ExchangeRateResponse> exchange_rate_response_list = exchange_rate_list.Select(temp => temp.ToExchangeRateResponse()).ToList();
            _exchangeRateRepositoryMock.Setup(temp => temp.GetExchangeRatesAsync(DEFAULT_CURRENCY_CODE, DEFAULT_API_URL)).ReturnsAsync(exchange_rate_list);

            List<CurrencySourceResponse> currency_source_list = new List<CurrencySourceResponse>() {
                _fixture.Build<CurrencySourceResponse>().With(x => x.CurrencyCode, DEFAULT_CURRENCY_CODE).With(x => x.SourceUrl, DEFAULT_API_URL).Create()
            };
            _currencySourceGetServiceMock.Setup(x => x.GetAllCurrencySources()).ReturnsAsync(currency_source_list);


            //Act
            IEnumerable<ExchangeRateResponse> actual_exchange_rate_response = await _exchangeRateGetService.GetExchangeRates();

            //Assert
            actual_exchange_rate_response.Should().BeEquivalentTo(exchange_rate_response_list);
        }

        #endregion

        #region GetFilteredExchangeRates


        [Fact]
        public async Task GetFilteredExchangeRates_HasAllMatchingExchangeRates()
        {
            //Arrange
            List<ExchangeRate> exchange_rate_list = new List<ExchangeRate>() {
                _fixture.Build<ExchangeRate>()
                    .With(x => x.SourceCurrency, new Currency() { Code = "USD"})
                    .With(x => x.TargetCurrency, new Currency() { Code = DEFAULT_CURRENCY_CODE}).Create(),
                _fixture.Build<ExchangeRate>()
                    .With(x => x.SourceCurrency, new Currency() { Code = "EUR"})
                    .With(x => x.TargetCurrency, new Currency() { Code = DEFAULT_CURRENCY_CODE}).Create(),
                _fixture.Build<ExchangeRate>()
                    .With(x => x.SourceCurrency, new Currency() { Code = "GBP"})
                    .With(x => x.TargetCurrency, new Currency() { Code = DEFAULT_CURRENCY_CODE}).Create()
            };
            
            List<ExchangeRateResponse> exchange_rate_response_list = exchange_rate_list.Select(temp => temp.ToExchangeRateResponse()).ToList();

            _exchangeRateRepositoryMock.Setup(temp => temp.GetExchangeRatesAsync(DEFAULT_CURRENCY_CODE, DEFAULT_API_URL)).ReturnsAsync(exchange_rate_list);

            List<CurrencySourceResponse> currency_source_list = new List<CurrencySourceResponse>() {
                _fixture.Build<CurrencySourceResponse>().With(x => x.CurrencyCode, DEFAULT_CURRENCY_CODE).With(x => x.SourceUrl, DEFAULT_API_URL).Create()
            };
            _currencySourceGetServiceMock.Setup(x => x.GetAllCurrencySources()).ReturnsAsync(currency_source_list);

            List<string> currencyCodes = new List<string>() {"USD", "EUR", "GBP"};

            //Act
            IEnumerable<ExchangeRateResponse> actual_exchange_rate_response = await _exchangeRateGetService.GetFilteredExchangeRates(currencyCodes);

            //Assert
            actual_exchange_rate_response.Should().BeEquivalentTo(exchange_rate_response_list);
        }



        [Fact]
        public async Task GetFilteredExchangeRates_HasFilteredExchangeRates() 
        {
            //Arrange
            List<ExchangeRate> exchange_rate_list = new List<ExchangeRate>() {
                _fixture.Build<ExchangeRate>()
                    .With(x => x.SourceCurrency, new Currency() { Code = "USD"})
                    .With(x => x.TargetCurrency, new Currency() { Code = DEFAULT_CURRENCY_CODE}).Create(),
                _fixture.Build<ExchangeRate>()
                    .With(x => x.SourceCurrency, new Currency() { Code = "EUR"})
                    .With(x => x.TargetCurrency, new Currency() { Code = DEFAULT_CURRENCY_CODE}).Create(),
                _fixture.Build<ExchangeRate>()
                    .With(x => x.SourceCurrency, new Currency() { Code = "DKK"})
                    .With(x => x.TargetCurrency, new Currency() { Code = DEFAULT_CURRENCY_CODE}).Create(),
                _fixture.Build<ExchangeRate>()
                    .With(x => x.SourceCurrency, new Currency() { Code = "BRL"})
                    .With(x => x.TargetCurrency, new Currency() { Code = DEFAULT_CURRENCY_CODE}).Create()
            };


            List<string> currencyCodes = new List<string>() { "USD", "EUR" };

            List<ExchangeRateResponse> exchange_rate_response_list = exchange_rate_list.Where(x => currencyCodes.Any(c => x.SourceCurrency.ToString() == c)).Select(temp => temp.ToExchangeRateResponse()).ToList();

            _exchangeRateRepositoryMock.Setup(temp => temp.GetExchangeRatesAsync(DEFAULT_CURRENCY_CODE, DEFAULT_API_URL)).ReturnsAsync(exchange_rate_list);

            List<CurrencySourceResponse> currency_source_list = new List<CurrencySourceResponse>() {
                _fixture.Build<CurrencySourceResponse>().With(x => x.CurrencyCode, DEFAULT_CURRENCY_CODE).With(x => x.SourceUrl, DEFAULT_API_URL).Create()
            };
            _currencySourceGetServiceMock.Setup(x => x.GetAllCurrencySources()).ReturnsAsync(currency_source_list);

            //Act
            IEnumerable<ExchangeRateResponse> actual_exchange_rate_response = await _exchangeRateGetService.GetFilteredExchangeRates(currencyCodes);

            //Assert
            actual_exchange_rate_response.Should().BeEquivalentTo(exchange_rate_response_list);
        }
        #endregion
    }
}