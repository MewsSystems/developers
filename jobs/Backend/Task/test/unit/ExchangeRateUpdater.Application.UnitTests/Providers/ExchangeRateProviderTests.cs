using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application.Providers;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.ExchangeRateApiServiceClient;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Application.UnitTests.Providers
{
    public class ExchangeRateProviderTests
    {
        private ExchangeRateProvider? _exchangeRateProvider;
        private IExchangeRateApiServiceClient? _exchangeRateApiServiceClient;
        private ILogger<ExchangeRateProvider>? _logger;

        [SetUp]
        public void Setup()
        {
            _exchangeRateApiServiceClient = Substitute.For<IExchangeRateApiServiceClient>();
            _logger = Substitute.For<ILogger<ExchangeRateProvider>>();
            _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateApiServiceClient, _logger);
        }

        [Test]
        public async Task GetExchangeRates_ShouldReturnExchangeRates_WhenApiReturnsExchangeRates()
        {
            const string usdCurrencyCode = "USD";
            const string eurCurrencyCode = "EUR";
            const decimal usdEurExchangeRate = 1.1m;

            var usdCurrency = Currency.From(usdCurrencyCode);

            var apiRates = new Dictionary<string, decimal>()
            {
                { eurCurrencyCode, usdEurExchangeRate }
            };

            var apiResponse = Task.FromResult(new GetExchangeRatesResponse
            {
                Rates = apiRates
            });

            _exchangeRateApiServiceClient!.GetExchageRates(Arg.Any<string>()).Returns(apiResponse);

            var actualResult = await _exchangeRateProvider!.GetExchangeRates(new List<Currency> { usdCurrency });

            var exchangeRates = actualResult.ToList();

            exchangeRates.Count.Should().Be(1);

            var usdEurExchange = exchangeRates.First();
            usdEurExchange.SourceCurrency.Value.Should().Be(usdCurrencyCode);
            usdEurExchange.TargetCurrency.Value.Should().Be(eurCurrencyCode);
            usdEurExchange.Value.Should().Be(usdEurExchangeRate);
        }

        [Test]
        public async Task GetExchangeRates_ShouldReturnEmptyResult_WhenApiReturnsEmptyResult()
        {
            const string usdCurrencyCode = "USD";

            var usdCurrency = Currency.From(usdCurrencyCode);


            var apiResponse = Task.FromResult(new GetExchangeRatesResponse
            {
                Rates = new Dictionary<string, decimal>()
            });

            _exchangeRateApiServiceClient!.GetExchageRates(Arg.Any<string>()).Returns(apiResponse);

            var actualResult = await _exchangeRateProvider!.GetExchangeRates(new List<Currency> { usdCurrency });

            var exchangeRates = actualResult.ToList();

            exchangeRates.Should().BeEmpty();
        }

        [Test]
        public async Task GetExchangeRates_ShouldReturnEmptyResult_WhenApiReturnsNullObjectForRates()
        {
            const string usdCurrencyCode = "USD";

            var usdCurrency = Currency.From(usdCurrencyCode);


            var apiResponse = Task.FromResult(new GetExchangeRatesResponse
            {
                Rates = null
            });

            _exchangeRateApiServiceClient!.GetExchageRates(Arg.Any<string>()).Returns(apiResponse);

            var actualResult = await _exchangeRateProvider!.GetExchangeRates(new List<Currency> { usdCurrency });

            var exchangeRates = actualResult.ToList();

            exchangeRates.Should().BeEmpty();
        }

        [Test]
        public async Task GetExchangeRates_ShouldReturnEmptyResult_WhenApiThrowsException()
        {
            const string usdCurrencyCode = "USD";

            var usdCurrency = Currency.From(usdCurrencyCode);

            _exchangeRateApiServiceClient!.GetExchageRates(Arg.Any<string>()).Throws(new Exception());

            var actualResult = await _exchangeRateProvider!.GetExchangeRates(new List<Currency> { usdCurrency });

            var exchangeRates = actualResult.ToList();

            exchangeRates.Should().BeEmpty();
        }
    }
}