using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB.Mapping;
using Mews.ExchangeRateUpdater.Services.Models;

namespace Mews.ExchangeRateUpdater.Tests.Services.ExternalRateProviders.CNB.Mapping
{
    [TestFixture]
    public class ExchangeRatesMapperTests
    {
        private ExchangeRates _exchangeRates;

        [SetUp] 
        public void SetUp() 
        {
            _exchangeRates = new ExchangeRates
            {
                Rates = new List<ExchangeRateDetails>
                {
                    new ExchangeRateDetails
                    {
                        ValidFor = "2023-10-14",
                        Country = "United Kingdom",
                        Amount = 1,
                        Currency = "pound",
                        CurrencyCode = "GBP",
                        Rate = 28.55M
                    },
                    new ExchangeRateDetails
                    {
                        ValidFor = "2023-10-14",
                        Country = "USA",
                        Amount = 1,
                        Currency = "dollar",
                        CurrencyCode = "USD",
                        Rate = 23.44M
                    },
                    new ExchangeRateDetails
                    {
                        ValidFor = "2023-10-14",
                        Country = "India",
                        Amount = 100,
                        Currency = "rupee",
                        CurrencyCode = "INR",
                        Rate = 28.15M
                    }
                }
            };
        }

        [Test]
        public void ToExchangeRateModels_WhenPassedDtoIsNull_ReturnsNull()
        {
            // Arrange
            ExchangeRates exchangeRates = null;

            // Act
            var actual = ExchangeRatesMapper.ToExchangeRateModels(exchangeRates);

            // Assert
            Assert.That(actual, Is.Null);
        }

        [Test]
        public void ToExchangeRateModels_WhenExchangeRateDetailsCollectionIsNull_ReturnsEmptyExchangeRateModelCollection()
        {
            // Arrange
            var exchangeRates = new ExchangeRates { Rates = null };

            // Act
            var actual = ExchangeRatesMapper.ToExchangeRateModels(exchangeRates);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ToExchangeRateModels_WhenExchangeRateDetailsCollectionIsEmpty_ReturnsEmptyExchangeRateModelCollection()
        {
            // Arrange
            var exchangeRates = new ExchangeRates { Rates = new List<ExchangeRateDetails>() };

            // Act
            var actual = ExchangeRatesMapper.ToExchangeRateModels(exchangeRates);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ToExchangeRateModels_WhenCalledWithExchangeRateDetailsCollection_MapsToExchangeRateModelCollectionAppropriately()
        {
            // Arrange

            // Act
            var actual = ExchangeRatesMapper.ToExchangeRateModels(_exchangeRates);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<IEnumerable<ExchangeRateModel>>());
            Assert.That(actual.Count(), Is.EqualTo(_exchangeRates.Rates.Count()));
            foreach (var exchangeRateDetails in _exchangeRates.Rates)
            {
                var actualExchangeRateModel = actual.First(x => x.SourceCurrency.Code == exchangeRateDetails.CurrencyCode);
                Assert.Multiple(() =>
                {
                    Assert.That(actualExchangeRateModel.ValidFor, Is.EqualTo(exchangeRateDetails.ValidFor));
                    Assert.That(actualExchangeRateModel.Country, Is.EqualTo(exchangeRateDetails.Country));
                    Assert.That(actualExchangeRateModel.SourceCurrency.Currency, Is.EqualTo(exchangeRateDetails.Currency));
                    Assert.That(actualExchangeRateModel.SourceCurrency.Code, Is.EqualTo(exchangeRateDetails.CurrencyCode));
                    Assert.That(actualExchangeRateModel.TargetCurrency.Currency, Is.EqualTo("koruna"));
                    Assert.That(actualExchangeRateModel.TargetCurrency.Code, Is.EqualTo("CZK"));
                    Assert.That(actualExchangeRateModel.Amount, Is.EqualTo(exchangeRateDetails.Amount));
                    Assert.That(actualExchangeRateModel.Rate, Is.EqualTo(exchangeRateDetails.Rate));
                });
            }
        }
    }
}
