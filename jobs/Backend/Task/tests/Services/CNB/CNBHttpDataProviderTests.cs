using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services.CNB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Threading;
using Xunit;
using System.Linq;

namespace ExchangeRateUpdater.Tests.Services.CNB
{
    public class CNBHttpDataProviderTests : IDisposable
    {
        private readonly string _tempBackupPath;
        private readonly Mock<ILogger<CNBHttpDataProvider>> _loggerMock;
        private readonly ExchangeRateOptions _options;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;

        public CNBHttpDataProviderTests()
        {
            _tempBackupPath = Path.Combine(Path.GetTempPath(), $"rates-backup-{Guid.NewGuid()}.txt");
            _loggerMock = new Mock<ILogger<CNBHttpDataProvider>>();
            _options = TestFixtures.CreateValidOptions();
            _options.BackupFilePath = _tempBackupPath;
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object);
        }

        public void Dispose()
        {
            if (File.Exists(_tempBackupPath))
            {
                File.Delete(_tempBackupPath);
            }
            _httpClient.Dispose();
        }

        [Fact]
        public void Constructor_WithNullHttpClient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CNBHttpDataProvider(
                null,
                optionsMock.Object,
                _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CNBHttpDataProvider(
                _httpClient,
                null,
                _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CNBHttpDataProvider(
                _httpClient,
                optionsMock.Object,
                null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithInvalidBaseUrl_ShouldThrowArgumentException(string baseUrl)
        {
            // Arrange
            var options = TestFixtures.CreateValidOptions();
            options.BaseUrl = baseUrl;
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(options);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CNBHttpDataProvider(
                _httpClient,
                optionsMock.Object,
                _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithNullCurrenciesToWatch_ShouldThrowArgumentException()
        {
            // Arrange
            var options = TestFixtures.CreateValidOptions();
            options.CurrenciesToWatch = null;
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(options);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CNBHttpDataProvider(
                _httpClient,
                optionsMock.Object,
                _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithEmptyCurrenciesToWatch_ShouldThrowArgumentException()
        {
            // Arrange
            var options = TestFixtures.CreateValidOptions();
            options.CurrenciesToWatch = Array.Empty<string>();
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(options);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CNBHttpDataProvider(
                _httpClient,
                optionsMock.Object,
                _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithInvalidCurrencyInWatch_ShouldThrowArgumentException()
        {
            // Arrange
            var options = TestFixtures.CreateValidOptions();
            options.CurrenciesToWatch = new[] { "", " ", null };
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(options);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CNBHttpDataProvider(
                _httpClient,
                optionsMock.Object,
                _loggerMock.Object));
        }

        [Fact]
        public async Task GetRawDataAsync_WhenHttpCallSucceeds_ShouldReturnDataAndCreateBackup()
        {
            // Arrange
            const string expectedData = @"24 Jan 2025 #17
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.145
Brazil|real|1|BRL|3.924";
            SetupHttpMockResponse(HttpStatusCode.OK, expectedData);

            var provider = CreateProvider();

            // Act
            var result = await provider.GetRawDataAsync();

            // Assert
            Assert.Equal(expectedData, result);
            Assert.True(File.Exists(_tempBackupPath));
            Assert.Equal(expectedData, await File.ReadAllTextAsync(_tempBackupPath));
        }

        [Fact]
        public async Task GetRawDataAsync_WhenHttpFailsAndBackupExists_ShouldReturnBackupData()
        {
            // Arrange
            const string backupData = @"24 Jan 2025 #17
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.145";
            await File.WriteAllTextAsync(_tempBackupPath, backupData);
            SetupHttpMockResponse(HttpStatusCode.ServiceUnavailable, "");

            var provider = CreateProvider();

            // Act
            var result = await provider.GetRawDataAsync();

            // Assert
            Assert.Equal(backupData, result);
        }

        [Fact]
        public async Task GetRawDataAsync_WhenHttpReturnsEmptyData_ShouldThrowException()
        {
            // Arrange
            SetupHttpMockResponse(HttpStatusCode.OK, "");
            var provider = CreateProvider();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => provider.GetRawDataAsync());
        }

        [Fact]
        public async Task GetRawDataAsync_WithHttpRequestException_AndNoBackup_ShouldThrowException()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var provider = new CNBHttpDataProvider(_httpClient, optionsMock.Object, _loggerMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => provider.GetRawDataAsync());
            Assert.Contains("Could not connect to CNB server and no valid backup data available", exception.Message);
        }

        [Fact]
        public async Task GetRawDataAsync_ShouldFetchDataConcurrently()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Sample data")
                });

            var provider = new CNBHttpDataProvider(_httpClient, optionsMock.Object, _loggerMock.Object);

            // Act
            var tasks = Enumerable.Range(0, 10).Select(_ => provider.GetRawDataAsync());
            var results = await Task.WhenAll(tasks);

            // Assert
            Assert.All(results, result => Assert.Equal("Sample data", result));
        }

        private CNBHttpDataProvider CreateProvider()
        {
            var optionsMock = new Mock<IOptions<ExchangeRateOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);

            return new CNBHttpDataProvider(_httpClient, optionsMock.Object, _loggerMock.Object);
        }

        private void SetupHttpMockResponse(HttpStatusCode statusCode, string content)
        {
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });
        }
    }
} 