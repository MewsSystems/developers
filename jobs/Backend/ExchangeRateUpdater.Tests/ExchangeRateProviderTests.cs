namespace ExchangeRateUpdater.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ExchangeRateUpdater.Code.Application;
    using ExchangeRateUpdater.Code.Observability;
    using ExchangeRateUpdater.Data;
    using ExchangeRateUpdater.Domain;
    using Moq;
    using Xunit;

    public class WhenWorkingWith_ExchangeRateProvider_And_AllRatesAvailable
    {
        private readonly Mock<ILogger> mockLogger;
        private readonly Mock<IExchangeRateData> mockExchangeRateData;
        private readonly Currency current = new Currency("CZK");

        public WhenWorkingWith_ExchangeRateProvider_And_AllRatesAvailable()
        {
            mockLogger = new Mock<ILogger>();
            mockExchangeRateData = new Mock<IExchangeRateData>();

            var data = GetBankDetails();

            mockExchangeRateData
                .Setup(x => x.GetExchangeRateData())
                .Returns(data);
        }

        [Theory]
        [InlineData("USD", 23)]
        [InlineData("SGD", 10)]
        [InlineData("IDR", 1.6)]
        public void Should_return_correct_exchange_rate(string currency, decimal expectedRate)
        {
            IEnumerable<Currency> currencies = new[] { new Currency(currency) };          

            var sut = new ExchangeRateProvider(mockLogger.Object, mockExchangeRateData.Object, current);

            var actual = sut.GetExchangeRates(currencies);

            Assert.Equal(expectedRate, actual.First().Value);
        }

        [Theory]
        [InlineData("ABC")]
        public void Should_return_empty_for_not_existing_rates(string currency)
        {
            IEnumerable<Currency> currencies = new[] { new Currency(currency) };

            var sut = new ExchangeRateProvider(mockLogger.Object, mockExchangeRateData.Object, current);

            var actual = sut.GetExchangeRates(currencies);

            Assert.Empty(actual);
        }

        [Theory]
        [InlineData("ABC", "USD", "IDR")]
        public void Should_return_only_rates_found(string missingCurrency, string validCurrencyOne, string validCurrencyTwo)
        {
            IEnumerable<Currency> currencies = new[] 
            { 
                new Currency(missingCurrency),
                new Currency(validCurrencyOne),
                new Currency(validCurrencyTwo)
            };

            var sut = new ExchangeRateProvider(mockLogger.Object, mockExchangeRateData.Object, current);

            var actual = sut.GetExchangeRates(currencies);

            var countMissingCurrency = actual.Count(x => x.TargetCurrency.Code == missingCurrency);

            Assert.Equal(2, actual.Count());
            Assert.Equal(0, countMissingCurrency);
        }


        private BankDetails GetBankDetails()
        {
            return new BankDetails()
            {
                Bank = "cnb",
                Date = DateTime.Now,
                Advice = 1,
                BankExchangeRateLink = new BankExchangeRateLink()
                {
                    Type = "XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU",
                    BankExchangeRateData = new List<BankExchangeRateData>()
                    {
                        new BankExchangeRateData() { Code = "USD", Country = "USA", CurrencyName = "Dollar", Rate = 23, Amount = 1},
                        new BankExchangeRateData() { Code = "SGD", Country = "Singapur", CurrencyName = "Dollar", Rate = 10, Amount = 1},
                        new BankExchangeRateData() { Code = "IDR", Country = "Indonesia", CurrencyName = "Rupie", Rate = 1600, Amount = 1000}
                    }
                }
            };
        }
    }
}
