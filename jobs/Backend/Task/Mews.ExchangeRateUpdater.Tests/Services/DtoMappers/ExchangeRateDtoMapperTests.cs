using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services.DtoMappers;
using Mews.ExchangeRateUpdater.Services.Models;
using Newtonsoft.Json.Linq;

namespace Mews.ExchangeRateUpdater.Tests.Services.DtoMappers
{
    [TestFixture]
    public class ExchangeRateDtoMapperTests
    {
        [Test]
        public void ToExchangeRateDtos_WhenCalledNullCollection_ReturnsEmpty()
        {
            // Arrange
            List<ExchangeRateModel> exchangeRateModelCollection = null;

            // Act
            var actual = ExchangeRateDtoMapper.ToExchangeRateDtos(exchangeRateModelCollection);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<IEnumerable<ExchangeRateDto>>());
            Assert.That(actual.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ToExchangeRateDtos_WhenCalledWithExchangeRateModelCollection_MapsSuccessfully()
        {
            // Arrange
            var exchangeRateModelCollection = new List<ExchangeRateModel>
            {
                new ExchangeRateModel
                {
                    SourceCurrency = new CurrencyModel {Code = "GBP"},
                    Rate = 28.55M,
                    Amount = 1
                },
                new ExchangeRateModel
                {
                    SourceCurrency = new CurrencyModel {Code = "USD"},
                    Rate = 23.44M,
                    Amount = 1
                },
                new ExchangeRateModel
                {
                    SourceCurrency = new CurrencyModel {Code = "TRY"},
                    Rate = 0.84M,
                    Amount = 1
                }
            };

            // Act
            var actual = ExchangeRateDtoMapper.ToExchangeRateDtos(exchangeRateModelCollection);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<IEnumerable<ExchangeRateDto>>());
            Assert.That(actual.Count(), Is.EqualTo(exchangeRateModelCollection.Count()));
            foreach (var exchangeRateModel in exchangeRateModelCollection)
            {
                var actualExchangeRateDto = actual.First(x => x.SourceCurrency.Code == exchangeRateModel.SourceCurrency.Code);
                Assert.Multiple(() =>
                {
                    Assert.That(actualExchangeRateDto.SourceCurrency.Code, Is.EqualTo(exchangeRateModel.SourceCurrency.Code));
                    Assert.That(actualExchangeRateDto.TargetCurrency.Code, Is.EqualTo(exchangeRateModel.TargetCurrency.Code));
                    Assert.That(actualExchangeRateDto.Value, Is.EqualTo(exchangeRateModel.Rate));
                    Assert.That(actualExchangeRateDto.ToString(), Is.EqualTo($"{exchangeRateModel.SourceCurrency.Code}/{exchangeRateModel.TargetCurrency.Code}={exchangeRateModel.Rate}"));
                });
            }
        }

        [Test]
        public void ToExchangeRateDtos_WhenCalledWithExchangeRateModelCollection_MapsRateToSingleCurrency()
        {
            // Arrange
            var exchangeRateModelCollection = new List<ExchangeRateModel>
            {
                new ExchangeRateModel
                {
                    SourceCurrency = new CurrencyModel {Code = "INR"},
                    Rate = 28.15M,
                    Amount = 100
                },
                new ExchangeRateModel
                {
                    SourceCurrency = new CurrencyModel {Code = "THB"},
                    Rate = 64.47M,
                    Amount = 100
                },
                new ExchangeRateModel
                {
                    SourceCurrency = new CurrencyModel {Code = "KRW"},
                    Rate = 1.73M,
                    Amount = 100
                }
            };

            // Act
            var actual = ExchangeRateDtoMapper.ToExchangeRateDtos(exchangeRateModelCollection);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<IEnumerable<ExchangeRateDto>>());
            Assert.That(actual.Count(), Is.EqualTo(exchangeRateModelCollection.Count()));
            foreach (var exchangeRateModel in exchangeRateModelCollection)
            {
                var actualExchangeRateDto = actual.First(x => x.SourceCurrency.Code == exchangeRateModel.SourceCurrency.Code);
                Assert.Multiple(() =>
                {
                    Assert.That(actualExchangeRateDto.SourceCurrency.Code, Is.EqualTo(exchangeRateModel.SourceCurrency.Code));
                    Assert.That(actualExchangeRateDto.TargetCurrency.Code, Is.EqualTo(exchangeRateModel.TargetCurrency.Code));
                    Assert.That(actualExchangeRateDto.Value, Is.EqualTo(exchangeRateModel.Rate / exchangeRateModel.Amount));
                    Assert.That(actualExchangeRateDto.ToString(), Is.EqualTo($"{exchangeRateModel.SourceCurrency.Code}/{exchangeRateModel.TargetCurrency.Code}={exchangeRateModel.Rate / exchangeRateModel.Amount}"));
                });
            }
        }
    }
}
