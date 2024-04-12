using AutoMapper;
using FluentAssertions;
using MewsFinance.Application.Clients;
using MewsFinance.Application.UseCases.ExchangeRates.Queries;
using MewsFinance.Domain.Models;
using Moq;

namespace MewsFinance.Application.UnitTests.UseCases.ExchangeRates.Queries
{
    public class GetExchangeRatesUseCaseTests
    {
        const string TargetCurrency = "CZK";

        private readonly GetExchangeRatesUseCase _sut;
        private readonly Mock<IFinancialClient> _financialClientMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetExchangeRatesUseCaseTests()
        {
            _financialClientMock = new Mock<IFinancialClient>();
            _financialClientMock.SetupGet(c => c.TargetCurrencyCode).Returns(TargetCurrency);
            _mapperMock = new Mock<IMapper>();

            _sut = new GetExchangeRatesUseCase(_financialClientMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task When_Requesting_Exchange_Rates_For_Currencies_Then_Return_Available_Exchange_Rates()
        {
            // Arrange
            var sourceCurrencyCodes = new string[] { "USD", "EUR", "JPY" };
            var providedCurrencyCodes = new string[] { "EUR", "JPY", "RUB", "TRY" };
            var expectedCurrencyCodesResult = new string[] { "EUR", "JPY" };

            var providedExchangeRates = CreateExchangeRates(providedCurrencyCodes, TargetCurrency);
            var expectedExchangeRatesResult = CreateExchangeRates(expectedCurrencyCodesResult, TargetCurrency);

            var expectedExchangeRateResponses = ToExchangeRateResponses(expectedExchangeRatesResult);            

            _financialClientMock.Setup(client => client.GetExchangeRates(It.IsAny<DateTime>()))
                                .ReturnsAsync(providedExchangeRates);
            _mapperMock.Setup(m => m.Map<IEnumerable<ExchangeRateResponse>>(It.IsAny< IEnumerable<ExchangeRate>>()))
                       .Returns(expectedExchangeRateResponses);

            var exchangeRateRequest = new ExchangeRateRequest { CurrencyCodes = sourceCurrencyCodes };

            // Act
            var result = await _sut.GetExchangeRates(exchangeRateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedExchangeRateResponses);
        }

        private static IEnumerable<ExchangeRate> CreateExchangeRates(string[] currencyCodes, string targetCurrency)
        {
            var exchangeRates = currencyCodes.Select(code => new ExchangeRate(
                new Currency(code), new Currency(targetCurrency), 3));

            return exchangeRates;
        }

        private static IEnumerable<ExchangeRateResponse> ToExchangeRateResponses(IEnumerable<ExchangeRate> exchangeRates)
        {
            var responses = exchangeRates.Select(e => new ExchangeRateResponse(e.SourceCurrency, e.TargetCurrency, e.Value));

            return responses;
        }
    }
}