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
            _mapperMock = new Mock<IMapper>();

            _sut = new GetExchangeRatesUseCase(_financialClientMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void When_Requesting_Exchange_Rates_For_Currencies_Then_Return_Available_Exchange_Rates()
        {
            // Arrange
            var currencyCodes = new string[] { "USD", "EUR" };
            var exchangeRates = CreateExchangeRates(currencyCodes, TargetCurrency);
            var expectedExchangeRates = ToExchangeRateResponses(exchangeRates);
            var exchangeRateRequest = new ExchangeRateRequest { CurrencyCodes = currencyCodes };

            _financialClientMock.Setup(client => client.GetExchangeRates(It.IsAny<DateTime>()))
                                .Returns(exchangeRates);
            _mapperMock.Setup(m => m.Map<IEnumerable<ExchangeRateResponse>>(exchangeRates))
                       .Returns(expectedExchangeRates);

            // Act
            var result = _sut.GetExchangeRates(exchangeRateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedExchangeRates);
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