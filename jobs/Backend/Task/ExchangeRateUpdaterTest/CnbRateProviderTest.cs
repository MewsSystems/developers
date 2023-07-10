using ExchangeRateUpdater.CnbProvider;
using ExchangeRateUpdater.CnbProvider.Abstractions;
using ExchangeRateUpdater.CnbProvider.CnbClientResponses;
using ExchangeRateUpdater.Domain;
using Moq;

namespace ExchangeRateUpdaterTest
{
    public class CnbRateProviderTest
    {
        private readonly Mock<ICnbRateProviderClient> _rateProviderClientMock;
        private readonly CnbRateProvider _sut;

        public CnbRateProviderTest()
        {
            _rateProviderClientMock = new Mock<ICnbRateProviderClient>();
            _sut = new CnbRateProvider(_rateProviderClientMock.Object);
        }

        [Fact]
        public async void WhenReceivesARatesEmptyListFromClientShouldReturnARatesEmptyList()
        {
            //Arrange
            _rateProviderClientMock.Setup(s => s.GetRatesByDateAsync(It.IsAny<string>())).ReturnsAsync(Enumerable.Empty<CnbRateResponseDto>());

            //Act
            var rates = await _sut.GetRatesByDateAsync(DateTime.UtcNow);

            //Assert
            _rateProviderClientMock.Verify(v => v.GetRatesByDateAsync(It.IsAny<string>()), Times.Exactly(2));
            Assert.False(rates.Any());
        }


        [Fact]
        public async void WhenReceivesAValidDateShouldReturnARatesList()
        {
            //Arrange
            IEnumerable<CnbRateResponseDto> exchangeRates = new[]
            {
                new CnbRateResponseDto(){ CurrencyCode = "EUR", Rate = 1.3m, Amount = 1 },
                new CnbRateResponseDto(){ CurrencyCode = "USD", Rate = 1.4m, Amount = 1 },
                new CnbRateResponseDto(){ CurrencyCode = "JPY", Rate = 1.4m, Amount = 0 },
                new CnbRateResponseDto(){ CurrencyCode = "KES", Rate = 58m, Amount = 100 },
            };
            _rateProviderClientMock.Setup(s => s.GetRatesByDateAsync(It.Is<string>(t => t.Contains("exrates")))).ReturnsAsync(exchangeRates);

            //Act
            var rates = await _sut.GetRatesByDateAsync(DateTime.UtcNow);

            //Assert
            _rateProviderClientMock.Verify(v => v.GetRatesByDateAsync(It.Is<string>(t => t.Contains("exrates"))), Times.Exactly(1));
            Assert.True(rates.Any());
            Assert.True(exchangeRates.First(w => w.CurrencyCode == "EUR").Rate / exchangeRates.First(w => w.CurrencyCode == "EUR").Amount
                == rates.First(w => w.SourceCurrency.Code == "EUR").Value);
            Assert.True(exchangeRates.First(w => w.CurrencyCode == "USD").Rate / exchangeRates.First(w => w.CurrencyCode == "USD").Amount
                == rates.First(w => w.SourceCurrency.Code == "USD").Value);
            Assert.True(exchangeRates.First(w => w.CurrencyCode == "KES").Rate / exchangeRates.First(w => w.CurrencyCode == "KES").Amount 
                == rates.First(w => w.SourceCurrency.Code == "KES").Value);
            Assert.True(rates.Count() == 3);
        }

        [Fact]
        public async void WhenReceivesAValisDateShouldCallBothApiAndMergeTheResult()
        {
            //Arrange
            IEnumerable<CnbRateResponseDto> exchangeExRates = new[]
            {
                new CnbRateResponseDto(){ CurrencyCode = "EUR", Rate = 1.3m, Amount = 1 },
                new CnbRateResponseDto(){ CurrencyCode = "USD", Rate = 1.4m, Amount = 1 },
                new CnbRateResponseDto(){ CurrencyCode = "JPY", Rate = 1.4m, Amount = 0 },
            };
            _rateProviderClientMock.Setup(s => s.GetRatesByDateAsync(It.Is<string>(t => t.Contains("exrates")))).ReturnsAsync(exchangeExRates);
            IEnumerable<CnbRateResponseDto> exchangeFxRates = new[]
{
                new CnbRateResponseDto(){ CurrencyCode = "KES", Rate = 58m, Amount = 100 },
                new CnbRateResponseDto(){ CurrencyCode = "THB", Rate = 1.4m, Amount = 0 },
                new CnbRateResponseDto(){ CurrencyCode = "RUB", Rate = 1.4m, Amount = 1 },
            };
            _rateProviderClientMock.Setup(s => s.GetRatesByDateAsync(It.Is<string>(t => t.Contains("fxrates")))).ReturnsAsync(exchangeFxRates);

            //Act
            var rates = await _sut.GetRatesByDateAsync(DateTime.UtcNow);

            //Assert
            _rateProviderClientMock.Verify(v => v.GetRatesByDateAsync(It.Is<string>(t => t.Contains("exrates"))), Times.Exactly(1));
            _rateProviderClientMock.Verify(v => v.GetRatesByDateAsync(It.Is<string>(t => t.Contains("fxrates"))), Times.Exactly(1));
            Assert.True(rates.Any());
            Assert.True(exchangeExRates.First(w => w.CurrencyCode == "EUR").Rate / exchangeExRates.First(w => w.CurrencyCode == "EUR").Amount
                == rates.First(w => w.SourceCurrency.Code == "EUR").Value);
            Assert.True(exchangeExRates.First(w => w.CurrencyCode == "USD").Rate / exchangeExRates.First(w => w.CurrencyCode == "USD").Amount
                == rates.First(w => w.SourceCurrency.Code == "USD").Value);
            Assert.True(exchangeFxRates.First(w => w.CurrencyCode == "KES").Rate / exchangeFxRates.First(w => w.CurrencyCode == "KES").Amount
                == rates.First(w => w.SourceCurrency.Code == "KES").Value);
            Assert.True(exchangeFxRates.First(w => w.CurrencyCode == "RUB").Rate / exchangeFxRates.First(w => w.CurrencyCode == "RUB").Amount
                == rates.First(w => w.SourceCurrency.Code == "RUB").Value);
            Assert.True(rates.Count() == 4);
        }
    }
}
