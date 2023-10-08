using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.UnitTests
{
    public class ExchangeRateProviderTests
    {
        private readonly ICzechNationalBankClient _source;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProviderTests()
        {
            _source = Substitute.For<ICzechNationalBankClient>();
            _logger = Substitute.For<ILogger<ExchangeRateProvider>>();
        }

        [Theory]
        [InlineData(new string[] { "A" }, new string[] { }, new string[] { })]
        [InlineData(new string[] { "A" }, new string[] { "B" }, new string[] { })]
        [InlineData(new string[] { "A" }, new string[] { "A" }, new string[] { "A" })]
        [InlineData(new string[] { "A" }, new string[] { "A", "B" }, new string[] { "A" })]
        public async Task GetExchangeRatesAsync_MatchingAndMismatchingCurrencies_ReturnsMatchingSourceCurrencyExchangeRates(
            string[] currencyCodes, string[] sourceCurrencyCodes, string[] expectedCurrencyCodes)
        {
            // Arrange
            var sourceExchangeRates = sourceCurrencyCodes
                .Select(code => new CnbExchangeRate(Amount: 1, CurrencyCode: code, Rate: 2m));

            _source
                .GetExchangeRatesAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new CnbExchangeRateResponse(sourceExchangeRates)));

            var sut = new ExchangeRateProvider(_source, _logger);

            var currencies = currencyCodes.Select(code => new Currency(code));

            // Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, CancellationToken.None);

            // Assert
            var areEqual = exchangeRates
                .Select(rate => rate.SourceCurrency.Code)
                .SequenceEqual(expectedCurrencyCodes);

            Assert.True(areEqual);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_MatchingSourceCurrency_ReturnsCzkTargetCurrency()
        {
            // Arrange
            const string currencyCode = "A";
            var sourceExchangeRates = new CnbExchangeRate[]
            {
                new(Amount: 1, CurrencyCode: currencyCode, Rate: 2m)
            };

            _source
                .GetExchangeRatesAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new CnbExchangeRateResponse(sourceExchangeRates)));

            var sut = new ExchangeRateProvider(_source, _logger);

            var currencies = new Currency[] { new(currencyCode) };

            // Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, CancellationToken.None);

            // Assert
            Assert.Single(exchangeRates, rate => rate.TargetCurrency.Code == "CZK");
        }

        public static IEnumerable<object[]> Data1 =>
            new List<object[]>
            {
                new object[] { 0, 1m },
                new object[] { -1, 1m },
                new object[] { 1, 0m },
                new object[] { 1, -1m },
            };

        [Theory]
        [MemberData(nameof(Data1))]
        public async Task GetExchangeRatesAsync_MatchingSourceCurrency_SkipsZeroOrNegativeValues(
            long amount, decimal rate)
        {
            // Arrange
            const string currencyCode = "A";
            var sourceExchangeRates = new CnbExchangeRate[]
            {
                new(Amount: amount, CurrencyCode: currencyCode, Rate: rate)
            };

            _source
                .GetExchangeRatesAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new CnbExchangeRateResponse(sourceExchangeRates)));

            var sut = new ExchangeRateProvider(_source, _logger);

            var currencies = new Currency[] { new(currencyCode) };

            // Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, CancellationToken.None);

            // Assert
            Assert.Empty(exchangeRates);
        }

        public static IEnumerable<object[]> Data2 =>
            new List<object[]>
            {
                new object[] { 1, 40m, 40m },
                new object[] { 10, 40m, 4m },
            };

        [Theory]
        [MemberData(nameof(Data2))]
        public async Task GetExchangeRatesAsync_DifferentPositiveAmounts_CalculatesCorrectExchangeRate(
            long amount, decimal rate, decimal expectedRate)
        {
            // Arrange
            const string currencyCode = "A";
            var sourceExchangeRates = new CnbExchangeRate[]
            {
                new(Amount: amount, CurrencyCode: currencyCode, Rate: rate)
            };

            _source
                .GetExchangeRatesAsync(Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new CnbExchangeRateResponse(sourceExchangeRates)));

            var sut = new ExchangeRateProvider(_source, _logger);

            var currencies = new Currency[] { new(currencyCode) };

            // Act
            var exchangeRates = await sut.GetExchangeRatesAsync(currencies, CancellationToken.None);

            // Assert
            Assert.Single(exchangeRates, rate => rate.Value == expectedRate);
        }
    }
}