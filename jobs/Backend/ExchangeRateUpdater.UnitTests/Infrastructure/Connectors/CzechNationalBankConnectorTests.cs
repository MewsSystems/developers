using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Response;

using FluentAssertions;

using System.Net;

namespace ExchangeRateUpdater.UnitTests.Infrastructure.Connectors
{
    public class CzechNationalBankConnectorTests : BaseCzechNationalBankConnectorTests
    {
        [Fact]
        public async Task GetExchangeRates_NullExchangeRateResponse_ReturnsEmptyExchangeRates()
        {
            // Arrange
            var expectedCurrencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("GBP")
            };

            ExchangeRatesResponse? exchangeRatesResponse = null;
            MockExchangeResponse(exchangeRatesResponse, HttpStatusCode.OK);

            var czechNationalBankConnector = GetConnector();

            // Act
            var exchangeRates = await czechNationalBankConnector.GetExchangeRates(expectedCurrencies);

            // Assert
            exchangeRates.Should().BeEmpty();
            exchangeRates.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetExchangeRates_NewExchangeRateResponse_ReturnsEmptyExchangeRates()
        {
            // Arrange
            var expectedCurrencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("GBP")
            };

            var exchangeRatesResponse = new ExchangeRatesResponse();
            MockExchangeResponse(exchangeRatesResponse, HttpStatusCode.OK);

            var czechNationalBankConnector = GetConnector();

            // Act
            var exchangeRates = await czechNationalBankConnector.GetExchangeRates(expectedCurrencies);

            // Assert
            exchangeRates.Should().BeEmpty();
            exchangeRates.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetExchangeRates_NewExchangeRateResponseList_ReturnsEmptyExchangeRates()
        {
            // Arrange
            var expectedCurrencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("GBP")
            };

            var exchangeRatesResponse = new ExchangeRatesResponse { Rates = new List<ExchangeRateResponse>() };
            MockExchangeResponse(exchangeRatesResponse, HttpStatusCode.OK);

            var czechNationalBankConnector = GetConnector();

            // Act
            var exchangeRates = await czechNationalBankConnector.GetExchangeRates(expectedCurrencies);

            // Assert
            exchangeRates.Should().BeEmpty();
            exchangeRates.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetExchangeRates_ValidCurrencies_ReturnsExpectedExchangeRates()
        {
            // Arrange
            var expectedCurrencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("GBP")
            };

            var exchangeRatesResponse = new ExchangeRatesResponse
            {
                Rates = new List<ExchangeRateResponse>
                {
                    new ExchangeRateResponse { CurrencyCode = "USD", Rate = 23.128m, Amount = 1, Country = "USA" },
                    new ExchangeRateResponse { CurrencyCode = "EUR", Rate = 24.560m, Amount = 1, Country = "EMU" },
                    new ExchangeRateResponse { CurrencyCode = "GBP", Rate = 28.462m, Amount = 1, Country = "Velká Británie" }
                }
            };

            MockExchangeResponse(exchangeRatesResponse, HttpStatusCode.OK);
            var czechNationalBankConnector = GetConnector();

            // Act
            var exchangeRates = await czechNationalBankConnector.GetExchangeRates(expectedCurrencies);

            // Assert
            exchangeRates.Should().NotBeNullOrEmpty();
            exchangeRates.Should().HaveCount(expectedCurrencies.Count);
        }

        [Fact]
        public async Task GetExchangeRates_OnlyOneValidCurrencies_ReturnsOneExchangeRate()
        {
            // Arrange
            var expectedCurrencies = new List<Currency>
            {
                new Currency("USD"),
            };

            var exchangeRatesResponse = new ExchangeRatesResponse
            {
                Rates = new List<ExchangeRateResponse>
                {
                    new ExchangeRateResponse { CurrencyCode = "USD", Rate = 23.128m, Amount = 1, Country = "USA" },
                    new ExchangeRateResponse { CurrencyCode = "EUR", Rate = 24.560m, Amount = 1, Country = "EMU" },
                    new ExchangeRateResponse { CurrencyCode = "GBP", Rate = 28.462m, Amount = 1, Country = "Velká Británie" }
                }
            };

            MockExchangeResponse(exchangeRatesResponse, HttpStatusCode.OK);
            var czechNationalBankConnector = GetConnector();

            // Act
            var exchangeRates = await czechNationalBankConnector.GetExchangeRates(expectedCurrencies);

            // Assert
            exchangeRates.Should().NotBeNullOrEmpty();
            exchangeRates.Should().HaveCount(1);
        }
    }
}
