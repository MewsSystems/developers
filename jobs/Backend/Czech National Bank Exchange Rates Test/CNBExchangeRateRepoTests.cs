using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Czech_National_Bank_ExchangeRates.Infrastructure;
using Czech_National_Bank_ExchangeRates.Models;
using Czech_National_Bank_ExchangeRates.Repository;

namespace Czech_National_Bank_Exchange_Rates_Test
{
    public class CNBExchangeRateRepoTests
    {
        private Mock<IHttpClientService> _mockHttpClientService;
        private Mock<ICNBExchangeRateConnection> _mockCnbExchangeRateConnection;
        private CNBExchangeRateRepo _service;

        [SetUp]
        public void Setup()
        {
            _mockHttpClientService = new Mock<IHttpClientService>();
            _mockCnbExchangeRateConnection = new Mock<ICNBExchangeRateConnection>();
            _service = new CNBExchangeRateRepo(_mockCnbExchangeRateConnection.Object, _mockHttpClientService.Object);
        }

        [Test]
        public async Task GetExhangeRateData_ReturnsExchangeRates()
        {
            // Arrange
            var dateString = "2024-07-09";
            var expectedUri = "https://api.example.com/rates/2024-07-09";
            var expectedExchangeRates = new ExchangeRates
            {
                Rates = new List<Rates>
            {
                new Rates
                {
                    ValidFor = "2024-07-09",
                    CurrencyCode = "USD",
                    Country = "USA",
                    Currency = "Dollar",
                    Amount = 1,
                    Rate = 1.2m,
                    Order = 1
                },
                new Rates
                {
                    ValidFor = "2024-07-09",
                    CurrencyCode = "EUR",
                    Country = "EU",
                    Currency = "Euro",
                    Amount = 1,
                    Rate = 1.1m,
                    Order = 2
                }
            }
            };

            _mockCnbExchangeRateConnection.Setup(conn => conn.Url).Returns("https://api.example.com/rates/{date}");
            _mockHttpClientService.Setup(client => client.GetAsync<ExchangeRates>(expectedUri, string.Empty, string.Empty))
                .ReturnsAsync(expectedExchangeRates);

            // Act
            var result = await _service.GetExhangeRateData(dateString);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Rates.Count, Is.EqualTo(expectedExchangeRates.Rates.Count));
            Assert.That(result.Rates[0].CurrencyCode, Is.EqualTo(expectedExchangeRates.Rates[0].CurrencyCode));
            Assert.That(result.Rates[1].CurrencyCode, Is.EqualTo(expectedExchangeRates.Rates[1].CurrencyCode));
        }

        [Test]
        public async Task GetExhangeRateData_InvalidDate_ReturnsNull()
        {
            // Arrange
            var dateString = "invalid-date";
            var expectedUri = "https://api.example.com/rates/invalid-date";

            _mockCnbExchangeRateConnection.Setup(conn => conn.Url).Returns("https://api.example.com/rates/{date}");
            _mockHttpClientService.Setup(client => client.GetAsync<ExchangeRates>(expectedUri, string.Empty, string.Empty))
                .ReturnsAsync((ExchangeRates)null);

            // Act
            var result = await _service.GetExhangeRateData(dateString);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetExhangeRateData_HttpClientThrowsException_ThrowsException()
        {
            // Arrange
            var dateString = "2024-07-09";
            var expectedUri = "https://api.example.com/rates/2024-07-09";

            _mockCnbExchangeRateConnection.Setup(conn => conn.Url).Returns("https://api.example.com/rates/{date}");
            _mockHttpClientService.Setup(client => client.GetAsync<ExchangeRates>(expectedUri, string.Empty, string.Empty))
                .ThrowsAsync(new Exception("HTTP request failed"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.GetExhangeRateData(dateString));
        }
    }

}
