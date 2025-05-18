using ExchangeRateUpdater.DataFetchers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace ExchangeRateUpdater.UnitTests
{
    [TestClass]
    public class ExchangeRateServiceTests
    {
        #region Test Data Fields
        private readonly string _validFileContent = "15 May 2025 #92\r\nCountry|Currency|Amount|Code|Rate\r\nAustralia|dollar|1|AUD|14.281\r\nBrazil|real|1|BRL|3.960\r\nBulgaria|lev|1|BGN|12.745";

        private readonly string _invalidFileContent = "15 May 2025 #92\r\nCountry;Currency;Amount;Code;Rate\r\nAustralia;dollar;1;AUD;14.281\r\nBrazil;real;1;BRL;3.960";

        private readonly ExchangeRate _audCzkExchangeRate = new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 14.281m);

        private readonly ExchangeRate _brlCzkExchangeRate = new ExchangeRate(new Currency("BRL"), new Currency("CZK"), 3.960m);

        private readonly ExchangeRate _bgnCzkExchangeRate = new ExchangeRate(new Currency("BGN"), new Currency("CZK"), 12.745m);

        private readonly Currency _aud = new("AUD");

        private readonly Currency _eur = new("EUR");

        private readonly Currency _xyz = new("XYZ");
        #endregion

        [TestMethod]
        public void GetExchangeRates_WithValidCurrencies_ReturnsMatchingExchangeRates()
        {
            // Arrange
            var mockFetcher = new Mock<IRemoteDataFetcher>();
            var mockParser = new Mock<IParser>();

            IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate> { _audCzkExchangeRate, _brlCzkExchangeRate, _bgnCzkExchangeRate };
            IEnumerable<Currency> currencies = new List<Currency>() { _aud, _eur, _xyz };

            mockFetcher.Setup(f => f.FetchData()).Returns(_validFileContent);
            mockParser.Setup(p => p.ParseData(_validFileContent)).Returns(exchangeRates);

            ExchangeRateService exchangeRateService = new(mockFetcher.Object, mockParser.Object);
            
            // Act
            var result = exchangeRateService.GetExchangeRates(currencies);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeEmpty();
                result.Should().HaveCount(1);
                result.Should().Contain(_audCzkExchangeRate);
            }
        }

        [TestMethod]
        public void GetExchangeRates_WithNoMatchingCurrencies_ReturnsEmptyList()
        {
            // Arrange
            var mockFetcher = new Mock<IRemoteDataFetcher>();
            var mockParser = new Mock<IParser>();

            IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate> { _audCzkExchangeRate, _brlCzkExchangeRate, _bgnCzkExchangeRate };
            IEnumerable<Currency> currencies = new List<Currency>() { _eur, _xyz };

            mockFetcher.Setup(f => f.FetchData()).Returns(_validFileContent);
            mockParser.Setup(p => p.ParseData(_validFileContent)).Returns(exchangeRates);

            ExchangeRateService exchangeRateService = new(mockFetcher.Object, mockParser.Object);

            // Act
            var result = exchangeRateService.GetExchangeRates(currencies);

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetExchangeRates_WithMalformedData_ThrowsException()
        {
            // Arrange
            var mockFetcher = new Mock<IRemoteDataFetcher>();
            var mockParser = new Mock<IParser>();

            IEnumerable<Currency> currencies = new List<Currency>() { _aud, _eur, _xyz };

            mockFetcher.Setup(f => f.FetchData()).Returns(_invalidFileContent);
            mockParser.Setup(p => p.ParseData(_invalidFileContent)).Throws(new Exception());

            ExchangeRateService exchangeRateService = new(mockFetcher.Object, mockParser.Object);

            // Act
            Action act = () => exchangeRateService.GetExchangeRates(currencies);

            // Assert
            act.Should().Throw<Exception>();
        }

        [TestMethod]
        public void GetExchangeRates_WithEmptyInput_ThrowsArgumentNullException()
        {
            // Arrange
            var mockFetcher = new Mock<IRemoteDataFetcher>();
            var mockParser = new Mock<IParser>();

            string fileContent = "";
            IEnumerable<Currency> currencies = new List<Currency>() { _aud, _eur, _xyz };

            mockFetcher.Setup(f => f.FetchData()).Returns(fileContent);
            mockParser.Setup(p => p.ParseData(fileContent)).Throws(new ArgumentNullException());

            ExchangeRateService exchangeRateService = new(mockFetcher.Object, mockParser.Object);

            // Act
            Action act = () => exchangeRateService.GetExchangeRates(currencies);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
