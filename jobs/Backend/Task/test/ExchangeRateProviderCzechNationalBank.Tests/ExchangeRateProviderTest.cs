using CNB = ExchangeRateProviderCzechNationalBank;
using FluentAssertions;
using Moq;
using Moq.Contrib.HttpClient;
using ExchangeRateProvider.Contracts;
using ExchangeRateProviderCzechNationalBank.Interface;
using NSubstitute;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTest
    {
        private readonly string ValidCnbExchangeRatesUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        private readonly IStoreExchangeRates _storeExchangeRates = Substitute.For<IStoreExchangeRates>();
        private readonly ILogger<CNB.ExchangeRateProvider> _logger = Substitute.For<ILogger<CNB.ExchangeRateProvider>>();

        //giacomogranzotto2022 wrote a nice set of tests so I decided to use them.
        //later during development I've extracted most of the logic to StoreExchangeRates so 
        //those tests test mostly that ExchangeProvider uses StoreExchangeRates correctly.
        //Anyway I don't see a reason to delete them.
        #region giacomogranzotto2022 tests
        [Theory]
        [InlineData("USD", "22,120")]
        [InlineData("EUR", "23,925")]
        [InlineData("CHF", "24,114")]
        public async Task Test_Get_Available_ExchangeRate(string currency, string rate)
        {
            //Arrange
            CNB.ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();
            var requestDto = new List<Currency> { new Currency(currency) };
            List<ExchangeRate> storeReturn = new List<ExchangeRate> { new ExchangeRate(new Currency(currency), new Currency("CZK"), decimal.Parse(rate)) };
            _storeExchangeRates.GetRates(requestDto).Returns(storeReturn);

            //Act
            var result = await exchangeRateProvider.GetExchangeRates(requestDto);

            //Assert
            result.Should().Contain(x => x.SourceCurrency.Code.Equals(currency, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.ToString() == rate);
        }

        [Theory]
        [InlineData("PHP", "0,40479")]
        [InlineData("IDR", "0,001465")]
        [InlineData("THB", "0,66815")]
        public async Task Test_Get_Available_ExchangeRate_Fractioned(string currency, string rate)
        {
            //Arrange
            CNB.ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();
            var requestDto = new List<Currency> { new Currency(currency) };
            List<ExchangeRate> storeReturn = new List<ExchangeRate> { new ExchangeRate(new Currency(currency), new Currency("CZK"), decimal.Parse(rate)) };
            _storeExchangeRates.GetRates(requestDto).Returns(storeReturn);

            //Act
            var result = await exchangeRateProvider.GetExchangeRates(requestDto);

            //Assert
            result.Should().Contain(x => x.SourceCurrency.Code.Equals(currency, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.ToString() == rate);
        }

        [Theory]
        [InlineData("AAA")]
        [InlineData("BBB")]
        [InlineData("CCC")]
        public async Task Test_Get_Not_Available_ExchangeRate(string currency)
        {
            //Arrange
            CNB.ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();
            var requestDto = new List<Currency> { new Currency(currency) };
            List<ExchangeRate> storeReturn = new List<ExchangeRate>();
            _storeExchangeRates.GetRates(requestDto).Returns(storeReturn);

            //Act
            var result = await exchangeRateProvider.GetExchangeRates(requestDto);

            //Assert
            result.Should().NotContain(x => x.SourceCurrency.Code.Equals(currency, StringComparison.CurrentCultureIgnoreCase));
        }

        private CNB.ExchangeRateProvider GetExchangeRateProviderMock()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest()
                .ReturnsResponse(System.Net.HttpStatusCode.OK, File.ReadAllText("mock_exchange_rate_response.xml"), "text/xml");

            var factory = handler.CreateClientFactory();

            var client = factory.CreateClient();
            client.BaseAddress = new Uri(ValidCnbExchangeRatesUrl);

            var exchangeRateProvider = new CNB.ExchangeRateProvider(client, _storeExchangeRates, _logger);
            return exchangeRateProvider;
        }
        #endregion

        [Fact]
        public async Task Null_In_GetExchangeRates_Returns_EmptyExchangeRates()
        {
            CNB.ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();

            var result = await exchangeRateProvider.GetExchangeRates(null);

            Assert.Empty(result);
        }

        [Fact]
        public async Task EmptyCurrencies_In_GetExchangeRates_Returns_EmptyExchangeRates()
        {
            CNB.ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();

            var result = await exchangeRateProvider.GetExchangeRates(new List<Currency>());

            Assert.Empty(result);
        }

        public static readonly object[][] UpdateExchangeRatesConditionsData =
        {
            new object[]{new DateTime(2023, 1, 18, 10, 0, 0)},
            new object[]{new DateTime(2023, 1, 17, 15, 0, 0)}
        };

        [Theory, MemberData(nameof(UpdateExchangeRatesConditionsData))]
        public void UpdateExchangeRatesConditions_EvaluatesTrue(DateTime now)
        {
            CNB.ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();
            _storeExchangeRates.RatesUpdatedOn.Returns(new DateTime(2023, 1, 17, 11, 0, 0));


            var result = exchangeRateProvider.UpdateExchangeRatesConditions(now);

            Assert.True(result);
        }

        [Theory, MemberData(nameof(UpdateExchangeRatesConditionsData))]
        public void UpdateExchangeRatesConditions_EvaluatesFalse(DateTime now)
        {
            CNB.ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();
            _storeExchangeRates.RatesUpdatedOn.Returns(new DateTime(2023, 1, 18, 14, 0, 0));

            var result = exchangeRateProvider.UpdateExchangeRatesConditions(now);

            Assert.False(result);
        }
    }
}