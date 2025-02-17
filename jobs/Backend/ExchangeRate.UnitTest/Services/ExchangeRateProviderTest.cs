using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Services;
using ExchangeRate.Application.Services.Interfaces;
using NSubstitute;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExchangeRate.UnitTest.Services
{
    public class ExchangeRateProviderServiceTest
    {
        private readonly IExchangeRateProviderService _ratesProviderService;
        private readonly IExchangeRateService _exchangeRateService;
        public ExchangeRateProviderServiceTest()
        {
            _exchangeRateService = Substitute.For<IExchangeRateService>();
            _ratesProviderService = new ExchangeRateProviderService(_exchangeRateService);
        }

        [Fact]
        public async Task GetExchangeRatesByDate_ShouldReturnRates_WhenDataIsAvailable()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var currency = new CurrencyDTO("USD");
            var listCurrenciesRate = new List<ExchangeRateBankDTO>
            {
                new ExchangeRateBankDTO
                {
                    Country = "United States",
                    Currency = "Dollar",
                    Amount = 1,
                    Code = "USD",
                    Rate = 23.45m
                }
            };
            var exchangeRates = new ExchangeRatesBankDTO(listCurrenciesRate);

            var listBankCurrency = new List<CurrencyDTO>
            {
                new CurrencyDTO("USD")
            };
            var bankCurrencies = new CurrenciesBankDTO(listBankCurrency);
            _exchangeRateService.GetCurrenciesBank(exchangeRates).Returns(bankCurrencies);

            _exchangeRateService.GetExchangeRatesByDate(date).Returns(Task.FromResult(exchangeRates));

            // Act
            var result = await _ratesProviderService.GetExchangeRatesByDate(date, currency);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("CZK/USD", result.Results.Keys);
            Assert.Equal(23.45m, result.Results["CZK/USD"].Value);
            Assert.Equal(1, result.Results["CZK/USD"].Amount);
        }

        [Fact]
        public async Task GetExchangeRatesByDate_ShouldThrowInvalidOperationException_WhenFetchingFails()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var currency = new CurrencyDTO("USD");
            var expectedExceptionMessage = "Exchange rate data is empty or null";

            _exchangeRateService.GetExchangeRatesByDate(date)
                .Returns(Task.FromException<ExchangeRatesBankDTO>(new InvalidOperationException(expectedExceptionMessage)));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _ratesProviderService.GetExchangeRatesByDate(date, currency));
        }

        [Fact]
        public async Task GetExchangeRatesByDate_ShouldThrowArgNullException_WhenNoRatesAvailableForTargetCurrency()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var currency = new CurrencyDTO("USD");
            var listCurrenciesRate = new List<ExchangeRateBankDTO>();
            var exchangeRates = new ExchangeRatesBankDTO(listCurrenciesRate);

            _exchangeRateService.GetExchangeRatesByDate(date).Returns(Task.FromResult(exchangeRates));

            // Act & Assert
            var result = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _ratesProviderService.GetExchangeRatesByDate(date, currency));

        }

        [Fact]
        public async Task GetExchangeRatesByDate_ShouldThrowKeyNotFoundException_WhenSourceCurrencyIsNotCZK()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var exchangeRatesDTO = new ExchangeRatesDTO
            {
                SourceCurrency = new CurrencyDTO("USD"),
                TargetCurrency = new CurrencyDTO("CZK"),
                Date = date
            };

            // Act & Assert
            var result = await Assert.ThrowsAsync<KeyNotFoundException>( 
                () => _ratesProviderService.GetExchangeRatesByDate(exchangeRatesDTO));

        }
        [Fact]
        public async Task GetExchangeRatesByDate_ShouldThrowKeyNotFoundException_WhenCurrencyIsInvalid()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var targetCurrency = new CurrencyDTO("EUR"); 

            var listCurrenciesRate = new List<ExchangeRateBankDTO>
            {
                new ExchangeRateBankDTO
                {
                    Country = "United States",
                    Currency = "Dollar",
                    Amount = 1,
                    Code = "USD",
                    Rate = 23.45m
                }
            };

            var exchangeRates = new ExchangeRatesBankDTO(listCurrenciesRate);

            var listBankCurrency = new List<CurrencyDTO>
            {
                new CurrencyDTO("USD")
            };
            var bankCurrencies = new CurrenciesBankDTO(listBankCurrency);
            _exchangeRateService.GetCurrenciesBank(exchangeRates).Returns(bankCurrencies);

            _exchangeRateService.GetExchangeRatesByDate(date).Returns(Task.FromResult(exchangeRates));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _ratesProviderService.GetExchangeRatesByDate(date, targetCurrency));

        }

    }
}
