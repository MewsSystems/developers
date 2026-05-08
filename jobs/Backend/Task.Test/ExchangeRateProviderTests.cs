namespace ExchangeReaderUpdater.Test
{
    using ExchangeRateUpdater;
    using ExchangeRateUpdater.Exceptions;
    using ExchangeRateUpdater.ExchangeClients;
    using ExchangeRateUpdater.Models;
    using ExchangeRateUpdater.Parsers;
    using FluentAssertions;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task get_exchangeRatesAsync_returnsParsedRates_fromClientData()
        {
            var mockClient = new Mock<IExchangeRateClient>();
            var parser = new CnbExchangeRateDataParser();

            var currencies = new[] { new Currency("USD"), new Currency("CZK") };
            var rawData = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD|21.456\n";

            var expectedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"),new Currency("CZK"), 21.456m)
            };

            mockClient.Setup(c => c.GetDailyRatesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rawData);

            var provider = new ExchangeRateProvider(mockClient.Object, parser, NullLogger<ExchangeRateProvider>.Instance);

            var result = (await provider.GetExchangeRatesAsync(currencies)).ToList();

            result.Should().BeEquivalentTo(expectedRates);
            mockClient.Verify(c => c.GetDailyRatesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task getExchangeRatesAsync_nullCurrencies_throwsArgumentNullException()
        {
            var mockClient = new Mock<IExchangeRateClient>();
            var mockParser = new Mock<IExchangeRateDataParser>();
            var provider = new ExchangeRateProvider(mockClient.Object, mockParser.Object, NullLogger<ExchangeRateProvider>.Instance);

            var act = async () => await provider.GetExchangeRatesAsync(null);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task getExchangeRatesAsync_emptyCurrencyList_returnsEmpty()
        {
            var mockClient = new Mock<IExchangeRateClient>();
            var mockParser = new Mock<IExchangeRateDataParser>();
            var provider = new ExchangeRateProvider(mockClient.Object, mockParser.Object, NullLogger<ExchangeRateProvider>.Instance);

            var result = await provider.GetExchangeRatesAsync(Array.Empty<Currency>());

            result.Should().BeEmpty();
            mockClient.Verify(c => c.GetDailyRatesAsync(It.IsAny<CancellationToken>()), Times.Never);
            mockParser.Verify(p => p.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Currency>>()), Times.Never);
        }

        [Fact]
        public async Task getExchangeRatesAsync_propagatesExchangeRateUpdateExceptionFromClient()
        {
            var mockClient = new Mock<IExchangeRateClient>();
            var mockParser = new Mock<IExchangeRateDataParser>();
            var provider = new ExchangeRateProvider(mockClient.Object, mockParser.Object, NullLogger<ExchangeRateProvider>.Instance);

            mockClient.Setup(c => c.GetDailyRatesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ExchangeRateUpdateException("remote error"));

            var currencies = new[] { new Currency("USD"), new Currency("CZK") };

            var act = async () => await provider.GetExchangeRatesAsync(currencies);

            await act.Should().ThrowAsync<ExchangeRateUpdateException>();
        }
    }
}
