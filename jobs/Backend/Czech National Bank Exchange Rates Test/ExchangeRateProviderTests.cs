using Czech_National_Bank_ExchangeRates.Models;
using Czech_National_Bank_ExchangeRates.Repository;
using ExchangeRateUpdater;
using Moq;

namespace Czech_National_Bank_Exchange_Rates_Test
{
    public class ExchangeRateProviderTests
    {
        private Mock<ICNBExchangeRateRepo> _mockRepo;
        private ExchangeRateProvider _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ICNBExchangeRateRepo>();
            _service = new ExchangeRateProvider(_mockRepo.Object);
        }

        [Test]
        public async Task GetExchangeRatesByDate_ReturnsExchangeRates()
        {
            // Arrange
            var dateString = "2024-07-09";
            var expectedExchangeRates = new Czech_National_Bank_ExchangeRates.Models.ExchangeRates
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

            _mockRepo.Setup(repo => repo.GetExhangeRateData(dateString)).ReturnsAsync(expectedExchangeRates);

            // Act
            var result = await _service.GetExchangeRatesByDate(dateString);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Rates.Count, Is.EqualTo(expectedExchangeRates.Rates.Count));
            Assert.That(result.Rates[0].CurrencyCode, Is.EqualTo(expectedExchangeRates.Rates[0].CurrencyCode));
            Assert.That(result.Rates[1].CurrencyCode, Is.EqualTo(expectedExchangeRates.Rates[1].CurrencyCode));
        }

        [Test]
        public async Task GetExchangeRatesByDate_EmptyDate_ReturnsNull()
        {
            // Arrange
            var dateString = "";
            _mockRepo.Setup(repo => repo.GetExhangeRateData(dateString)).ReturnsAsync((ExchangeRates)null);

            // Act
            var result = await _service.GetExchangeRatesByDate(dateString);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetExchangeRatesByDate_InvalidDate_ThrowsException()
        {
            // Arrange
            var dateString = "invalid-date";
            _mockRepo.Setup(repo => repo.GetExhangeRateData(dateString)).ThrowsAsync(new Exception("Invalid date format"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.GetExchangeRatesByDate(dateString));
        }
    }
}