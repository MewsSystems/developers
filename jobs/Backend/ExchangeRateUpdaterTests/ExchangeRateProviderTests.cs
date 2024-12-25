using AutoFixture;
using AutoMapper;
using ExchangeRateUpdater.Mapping;
using FluentAssertions;
using NSubstitute;
using Xunit;

using CNB = Cnb.Api.Client;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateServiceTests
    {
        private static IEnumerable<Currency> currencies = new[]
        {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

        private readonly Fixture _fixture;
        private readonly IMapper _mapper;
        public ExchangeRateServiceTests()
        {
            // Set up AutoFixture and AutoMapper
            _fixture = new Fixture();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExRatesDailyProfile>();
            });
            _mapper = config.CreateMapper();
            
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnFilteredExchangeRates()
        {
            // Arrange
            var responseRates = new List<CNB.ExRateDailyRest>
            {
                new CNB.ExRateDailyRest { CurrencyCode = "USD", Rate = 23.048, Amount = 1 },
                new CNB.ExRateDailyRest { CurrencyCode = "EUR", Rate = 25.750, Amount = 1 },
                new CNB.ExRateDailyRest { CurrencyCode = "CZK", Rate = 1, Amount = 1 },
                new CNB.ExRateDailyRest { CurrencyCode = "GBP", Rate = 29.395, Amount = 1 },  // Should be filtered out
                new CNB.ExRateDailyRest { CurrencyCode = "JPY", Rate = 21.045, Amount = 1 }
            };

            var response = new CNB.ExRateDailyResponse { Rates = responseRates };

            // Mocks
            
            var cnbApiClientMock = Substitute.For<CNB.ICnbApiClient>();
            var cnbApiClientFactory = Substitute.For<ICnbApiClientFactory>();

            cnbApiClientMock.DailyUsingGET_ExRatesDailyAsync(null, CNB.Lang.EN)
                .Returns(Task.FromResult(response));
          
            cnbApiClientFactory.CnbApiClient.Returns(cnbApiClientMock);

            // Initialize the service under test
            var exchangeRateProvider = new ExchangeRateProvider(cnbApiClientFactory, _mapper);

            // Act
            var result = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(4); // USD, EUR, CZK, and JPY match the currencies list
            result.Select(r => r.SourceCurrency.Code).Should().BeEquivalentTo("USD", "EUR", "CZK", "JPY");
        }
    }
}