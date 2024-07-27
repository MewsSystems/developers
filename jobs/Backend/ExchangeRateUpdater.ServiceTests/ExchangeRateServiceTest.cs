using AutoFixture;
using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.DTO;
using ExchangeRateUpdater.Core.ServiceContracts;
using ExchangeRateUpdater.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Diagnostics.Metrics;

namespace ExchangeRateUpdater.ServiceTests
{
    public class ExchangeRateServiceTest
    {
        private readonly IExchangeRateGetService _exchangeRateGetService;

        private readonly Mock<IExchangeRateRepository> _exchangeRateRepositoryMock;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IFixture _fixture;

        public ExchangeRateServiceTest() 
        {
            _fixture = new Fixture();

            _exchangeRateRepositoryMock = new Mock<IExchangeRateRepository>();
            _exchangeRateRepository = _exchangeRateRepositoryMock.Object;

            var loggerMock = new Mock<ILogger<ExchangeRatesGetService>>();
            _exchangeRateGetService = new ExchangeRatesGetService(loggerMock.Object, _exchangeRateRepository);
        }

        #region GetExchangeRates
        [Fact]
        public async void GetExchangeRates_IsEmptyList()
        {
            //Arrange
            List<ExchangeRate> empty_list = new List<ExchangeRate>();
            _exchangeRateRepositoryMock.Setup(temp => temp.GetExchangeRatesAsync()).ReturnsAsync(empty_list);

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

            _exchangeRateRepositoryMock.Setup(temp => temp.GetExchangeRatesAsync()).ReturnsAsync(exchange_rate_list);

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
                _fixture.Build<ExchangeRate>().With(x => x.TargetCurrency, "USD" as string).Create(),
                _fixture.Build<ExchangeRate>().With(x => x.TargetCurrency, "EUR" as string).Create(),
                _fixture.Build<ExchangeRate>().With(x => x.TargetCurrency, "GBP" as string).Create(),
            };
            
            List<ExchangeRateResponse> exchange_rate_response_list = exchange_rate_list.Select(temp => temp.ToExchangeRateResponse()).ToList();

            _exchangeRateRepositoryMock.Setup(temp => temp.GetExchangeRatesAsync()).ReturnsAsync(exchange_rate_list);

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
                _fixture.Build<ExchangeRate>().With(x => x.TargetCurrency, "USD" as string).Create(),
                _fixture.Build<ExchangeRate>().With(x => x.TargetCurrency, "EUR" as string).Create(),
                _fixture.Build<ExchangeRate>().With(x => x.TargetCurrency, "DKK" as string).Create(),
                _fixture.Build<ExchangeRate>().With(x => x.TargetCurrency, "BRL" as string).Create(),
            };

            List<string> currencyCodes = new List<string>() { "USD", "EUR" };

            List<ExchangeRateResponse> exchange_rate_response_list = exchange_rate_list.Where(x => currencyCodes.Any(c => x.TargetCurrency.ToUpper() == c.ToUpper())).Select(temp => temp.ToExchangeRateResponse()).ToList();

            _exchangeRateRepositoryMock.Setup(temp => temp.GetExchangeRatesAsync()).ReturnsAsync(exchange_rate_list);


            //Act
            IEnumerable<ExchangeRateResponse> actual_exchange_rate_response = await _exchangeRateGetService.GetFilteredExchangeRates(currencyCodes);

            //Assert
            actual_exchange_rate_response.Should().BeEquivalentTo(exchange_rate_response_list);
        }
        #endregion
    }
}