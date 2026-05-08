using FluentAssertions;
using Moq;
using ExchangeRateUpdater;
using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Parsers.Models;
using ExchangeRateUpdater.Tests.TestData;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<ICnbHttpClient> _mockHttpClient;
        private readonly Mock<ICnbDataParser> _mockParser;
        private readonly ExchangeRateProvider _provider;

        public ExchangeRateProviderTests()
        {
            _mockHttpClient = new Mock<ICnbHttpClient>();
            _mockParser = new Mock<ICnbDataParser>();
            _provider = new ExchangeRateProvider(_mockHttpClient.Object, _mockParser.Object);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ValidData_ReturnsFilteredRates()
        {
            // Build mock data
            var rawData = "mocked CNB data";
            var parsedData = new List<CnbExchangeRateData>
            {
                CnbTestDataBuilder.USD(),
                CnbTestDataBuilder.EUR(),
                CnbTestDataBuilder.JPY(),
                CnbTestDataBuilder.GBP()
            };

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("JPY")
            };

            _mockHttpClient
                .Setup(x => x.GetExchangeRatesAsync())
                .ReturnsAsync(rawData);

            _mockParser
                .Setup(x => x.Parse(rawData))
                .Returns(parsedData);

            // Call provider
            var result = await _provider.GetExchangeRatesAsync(currencies);
            var rates = result.ToList();

            // Assert
            rates.Should().HaveCount(3);
            rates.Should().Contain(r => r.TargetCurrency.Code == "USD");
            rates.Should().Contain(r => r.TargetCurrency.Code == "EUR");
            rates.Should().Contain(r => r.TargetCurrency.Code == "JPY");
            rates.Should().NotContain(r => r.TargetCurrency.Code == "GBP");
        }

        [Fact]
        public async Task GetExchangeRatesAsync_CorrectlyNormalizesRates()
        {
            // Build mock data
            var rawData = "mocked CNB data";
            var parsedData = new List<CnbExchangeRateData>
            {
                CnbTestDataBuilder.USD(),
                CnbTestDataBuilder.JPY(),
                CnbTestDataBuilder.IDR()
            };

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("JPY"),
                new Currency("IDR")
            };

            _mockHttpClient
                .Setup(x => x.GetExchangeRatesAsync())
                .ReturnsAsync(rawData);

            _mockParser
                .Setup(x => x.Parse(rawData))
                .Returns(parsedData);

            // Call provider
            var result = await _provider.GetExchangeRatesAsync(currencies);
            var rates = result.ToList();

            // Assert
            var usdRate = rates.First(r => r.TargetCurrency.Code == "USD");
            usdRate.Value.Should().BeApproximately(1m / 20.774m, 0.000001m);

            var jpyRate = rates.First(r => r.TargetCurrency.Code == "JPY");
            jpyRate.Value.Should().BeApproximately(100m / 13.278m, 0.000001m);

            var idrRate = rates.First(r => r.TargetCurrency.Code == "IDR");
            idrRate.Value.Should().BeApproximately(1000m / 1.238m, 0.000001m);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_AllRatesHaveCzkAsSource()
        {
            // Build mock data
            var rawData = "mocked CNB data";
            var parsedData = new List<CnbExchangeRateData>
            {
                CnbTestDataBuilder.USD(),
                CnbTestDataBuilder.EUR()
            };

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            _mockHttpClient
                .Setup(x => x.GetExchangeRatesAsync())
                .ReturnsAsync(rawData);

            _mockParser
                .Setup(x => x.Parse(rawData))
                .Returns(parsedData);

            // Call provider
            var result = await _provider.GetExchangeRatesAsync(currencies);
            var rates = result.ToList();

            // Assert
            rates.Should().AllSatisfy(r =>
            {
                r.SourceCurrency.Code.Should().Be("CZK");
            });
        }

        [Fact]
        public async Task GetExchangeRatesAsync_CaseInsensitiveCurrencyMatching()
        {
            // Build mock data
            var rawData = "mocked CNB data";
            var parsedData = new List<CnbExchangeRateData>
            {
                CnbTestDataBuilder.USD()
            };

            var currencies = new[]
            {
                new Currency("usd"),  // lowercase
                new Currency("USD"),  // uppercase
                new Currency("Usd")   // mixed case
            };

            _mockHttpClient
                .Setup(x => x.GetExchangeRatesAsync())
                .ReturnsAsync(rawData);

            _mockParser
                .Setup(x => x.Parse(rawData))
                .Returns(parsedData);

            // Call provider
            var result = await _provider.GetExchangeRatesAsync(currencies);
            var rates = result.ToList();

            // Assert - Should return USD only once despite multiple case variations
            rates.Should().HaveCount(1);
            rates[0].TargetCurrency.Code.Should().Be("USD");
        }

        [Fact]
        public async Task GetExchangeRatesAsync_MissingCurrency_SilentlyIgnored()
        {
            // Build mock data
            var rawData = "mocked CNB data";
            var parsedData = new List<CnbExchangeRateData>
            {
                CnbTestDataBuilder.USD(),
                CnbTestDataBuilder.EUR()
            };

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("XYZ"),  // Doesn't exist in parsed data
                new Currency("KES")   // Doesn't exist in parsed data
            };

            _mockHttpClient
                .Setup(x => x.GetExchangeRatesAsync())
                .ReturnsAsync(rawData);

            _mockParser
                .Setup(x => x.Parse(rawData))
                .Returns(parsedData);

            // Call provider
            var result = await _provider.GetExchangeRatesAsync(currencies);
            var rates = result.ToList();

            // Assert - Should only return USD, silently ignoring XYZ and KES
            rates.Should().HaveCount(1);
            rates[0].TargetCurrency.Code.Should().Be("USD");
        }

        [Fact]
        public async Task GetExchangeRatesAsync_EmptyRequestedCurrencies_ReturnsEmpty()
        {
            // Build mock data
            var rawData = "mocked CNB data";
            var parsedData = new List<CnbExchangeRateData>
            {
                CnbTestDataBuilder.USD()
            };

            var currencies = Array.Empty<Currency>();

            _mockHttpClient
                .Setup(x => x.GetExchangeRatesAsync())
                .ReturnsAsync(rawData);

            _mockParser
                .Setup(x => x.Parse(rawData))
                .Returns(parsedData);

            // Call provider
            var result = await _provider.GetExchangeRatesAsync(currencies);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetExchangeRatesAsync_HttpClientThrows_PropagatesException()
        {
            // Build mock data
            var currencies = new[] { new Currency("USD") };

            _mockHttpClient
                .Setup(x => x.GetExchangeRatesAsync())
                .ThrowsAsync(new InvalidOperationException("Network error"));

            // Call provider & Assert
            await _provider.Invoking(p => p.GetExchangeRatesAsync(currencies))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Network error");
        }

        [Fact]
        public void Constructor_NullHttpClient_ThrowsArgumentNullException()
        {
            // Call provider & Assert
            var act = () => new ExchangeRateProvider(null!, _mockParser.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("httpClient");
        }

        [Fact]
        public void Constructor_NullParser_ThrowsArgumentNullException()
        {
            // Call provider & Assert
            var act = () => new ExchangeRateProvider(_mockHttpClient.Object, null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("parser");
        }
    }
}
