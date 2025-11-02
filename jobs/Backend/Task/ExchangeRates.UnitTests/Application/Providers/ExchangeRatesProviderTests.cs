using ExchangeRates.Application.Options;
using ExchangeRates.Application.Providers;
using ExchangeRates.Infrastructure.Clients.CNB;
using ExchangesRates.Infrastructure.External.CNB.Dtos;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Json;

namespace ExchangeRates.UnitTests.Application.Providers
{
    public class ExchangeRatesProviderTests
    {
        private readonly Mock<ICnbHttpClient> _cnbMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly IOptions<CnbHttpClientOptions> _cnbOptions;
        private readonly IOptions<ExchangeRatesOptions> _exchangeOptions;
        private readonly ILogger<ExchangeRatesProvider> _logger;

        public ExchangeRatesProviderTests()
        {
            _cnbMock = new Mock<ICnbHttpClient>();
            _cacheMock = new Mock<IDistributedCache>();

            _cnbOptions = Options.Create(new CnbHttpClientOptions
            {
                DailyRefreshTimeCZ = new TimeOnly(14, 30)
            });

            _exchangeOptions = Options.Create(new ExchangeRatesOptions
            {
                DefaultCurrencies = new[] { "USD", "EUR", "CZK" }
            });

            _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<ExchangeRatesProvider>.Instance;
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsRates_FromCNBClient()
        {
            // Arrange
            var cnbResponse = new CnbExRatesResponse
            {
                Rates = new List<CnbExRate>
            {
                new CnbExRate { CurrencyCode = "USD", Amount = 1, Rate = 22 },
                new CnbExRate { CurrencyCode = "EUR", Amount = 1, Rate = 24 }
            }
            };

            _cnbMock.Setup(c => c.GetDailyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cnbResponse);

            var provider = new ExchangeRatesProvider(_cnbMock.Object, _cacheMock.Object, _cnbOptions, _exchangeOptions, _logger);

            // Act
            var result = await provider.GetExchangeRatesAsync(new[] { "USD", "EUR" }, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.First(r => r.TargetCurrency.Code == "USD").Value.Should().Be(22);
            result.First(r => r.TargetCurrency.Code == "EUR").Value.Should().Be(24);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_UsesCache_WhenDataExists()
        {
            // Arrange
            var cachedResponse = new CnbExRatesResponse
            {
                Rates = new List<CnbExRate> { new CnbExRate { CurrencyCode = "USD", Amount = 1, Rate = 21 } }
            };

            var serialized = JsonSerializer.Serialize(cachedResponse);
            var bytes = System.Text.Encoding.UTF8.GetBytes(serialized);

            _cacheMock
                .Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bytes);

            var provider = new ExchangeRatesProvider(_cnbMock.Object, _cacheMock.Object, _cnbOptions, _exchangeOptions, _logger);

            // Act
            var result = await provider.GetExchangeRatesAsync(new[] { "USD" }, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result.First().Value.Should().Be(21);

            _cacheMock.Verify(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _cnbMock.Verify(c => c.GetDailyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsEmpty_WhenCurrencyNotFound()
        {
            // Arrange
            var cnbResponse = new CnbExRatesResponse
            {
                Rates = new List<CnbExRate> { new CnbExRate { CurrencyCode = "USD", Amount = 1, Rate = 22 } }
            };

            _cnbMock.Setup(c => c.GetDailyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cnbResponse);

            var provider = new ExchangeRatesProvider(_cnbMock.Object, _cacheMock.Object, _cnbOptions, _exchangeOptions, _logger);

            // Act
            var result = await provider.GetExchangeRatesAsync(new[] { "EUR" }, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetExchangeRatesAsync_UsesDefaultCurrencies_WhenInputIsNull()
        {
            // Arrange
            var cnbResponse = new CnbExRatesResponse
            {
                Rates = new List<CnbExRate>
            {
                new CnbExRate { CurrencyCode = "USD", Amount = 1, Rate = 22 },
                new CnbExRate { CurrencyCode = "EUR", Amount = 1, Rate = 24 },
                new CnbExRate { CurrencyCode = "CZK", Amount = 1, Rate = 1 }
            }
            };

            _cnbMock.Setup(c => c.GetDailyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cnbResponse);

            var provider = new ExchangeRatesProvider(_cnbMock.Object, _cacheMock.Object, _cnbOptions, _exchangeOptions, _logger);

            // Act
            var result = await provider.GetExchangeRatesAsync((IEnumerable<string>?)null, CancellationToken.None);

            // Assert
            result.Should().HaveCount(3);
            result.Select(r => r.TargetCurrency.Code).Should().Contain(new[] { "USD", "EUR", "CZK" });
        }

        [Fact]
        public async Task GetExchangeRatesAsync_Throws_WhenCancellationRequested()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel();

            _cnbMock
                .Setup(c => c.GetDailyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns<string, string, CancellationToken>((date, lang, token) =>
                {
                    token.ThrowIfCancellationRequested();
                    return Task.FromResult<CnbExRatesResponse?>(new CnbExRatesResponse
                    {
                        Rates = new List<CnbExRate> { new CnbExRate { CurrencyCode = "USD", Amount = 1, Rate = 22 } }
                    });
                });

            var provider = new ExchangeRatesProvider(_cnbMock.Object, _cacheMock.Object, _cnbOptions, _exchangeOptions, _logger);

            // Act
            Func<Task> act = async () => await provider.GetExchangeRatesAsync(new[] { "USD" }, cts.Token);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
        }


        [Fact]
        public async Task GetExchangeRatesAsync_Throws_WhenCNBClientFails()
        {
            // Arrange
            _cnbMock.Setup(c => c.GetDailyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new Exception("CNB API error"));

            var provider = new ExchangeRatesProvider(_cnbMock.Object, _cacheMock.Object, _cnbOptions, _exchangeOptions, _logger);

            // Act
            Func<Task> act = async () => await provider.GetExchangeRatesAsync(new[] { "USD" }, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("CNB API error");
        }
    }
}