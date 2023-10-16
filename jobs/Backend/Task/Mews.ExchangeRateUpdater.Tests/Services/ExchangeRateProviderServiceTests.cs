using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services;
using Mews.ExchangeRateUpdater.Services.ExternalRateProviders;
using Mews.ExchangeRateUpdater.Services.Models;
using Mews.ExchangeRateUpdater.Services.Validators;
using Moq;

namespace Mews.ExchangeRateUpdater.Tests.Services
{
    [TestFixture]
    public class ExchangeRateProviderServiceTests
    {
        private Mock<IExchangeRateProviderResolver> _exchangeRateProviderResolverMock;

        private Mock<IExchangeRateProvider> _exchangeRateProviderMock;

        private Mock<IRequestValidator> _currencyCodesValidatorMock;

        private IEnumerable<IRequestValidator> _requestValidators;

        private List<ExchangeRateModel> _exchangeRateModelCollection;

        private List<CurrencyDto> _currencyDtoCollection;

        private List<string> _validationMessages;

        private ExchangeRateProviderService _sut;

        [SetUp]
        public void SetUp()
        {
            _currencyDtoCollection = new List<CurrencyDto>
            {
                new CurrencyDto("GBP"),
                new CurrencyDto("USD"),
                new CurrencyDto("TRY"),
                new CurrencyDto("INR"),
                new CurrencyDto("THB"),
                new CurrencyDto("KRW")
            };

            _validationMessages = new List<string>();

            _exchangeRateModelCollection = new List<ExchangeRateModel>
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
                },
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

            _exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
            _exchangeRateProviderMock.Setup(x => x.GetExchangeRates()).ReturnsAsync(_exchangeRateModelCollection);                

            _exchangeRateProviderResolverMock = new Mock<IExchangeRateProviderResolver>();
            _exchangeRateProviderResolverMock.Setup(x => x.GetExchangeRateProvider()).Returns(_exchangeRateProviderMock.Object);

            _currencyCodesValidatorMock = new Mock<IRequestValidator>();
            _currencyCodesValidatorMock.Setup(x => x.Validate(ref It.Ref<List<CurrencyDto>>.IsAny, ref It.Ref<List<string>>.IsAny)).Returns(new List<string>());

            _requestValidators = new List<IRequestValidator> { _currencyCodesValidatorMock.Object };

            _sut = new ExchangeRateProviderService(_exchangeRateProviderResolverMock.Object, _requestValidators);
        }

        [Test]
        public async Task GetExchangeRates_Always_CallsValidateOnAllValidators()
        {
            // Arrange

            // Act
            await _sut.GetExchangeRates(_currencyDtoCollection);

            // Assert
            _currencyCodesValidatorMock.Verify(x => x.Validate(ref It.Ref<List<CurrencyDto>>.IsAny, ref It.Ref<List<string>>.IsAny), Times.Exactly(_requestValidators.Count()));
        }

        [Test]
        public async Task GetExchangeRates_Always_CallsGetExchangeRateProviderOnResolver()
        {
            // Arrange

            // Act
            await _sut.GetExchangeRates(_currencyDtoCollection);

            // Assert
            _exchangeRateProviderResolverMock.Verify(x => x.GetExchangeRateProvider(), Times.Once);
        }

        [Test]
        public async Task GetExchangeRates_Always_CallsGetExchangeRateOnExchangeRateProvider()
        {
            // Arrange

            // Act
            await _sut.GetExchangeRates(_currencyDtoCollection);

            // Assert
            _exchangeRateProviderMock.Verify(x => x.GetExchangeRates(), Times.Once);
        }

        [Test]
        public async Task GetExchangeRates_Only_ReturnsTheExchangeRateForMatchingCurrenciesFromTheProvider()
        {
            // Arrange
            _currencyDtoCollection.Add(new CurrencyDto("EUR"));
            _currencyDtoCollection.Add(new CurrencyDto("SGD"));
            _currencyDtoCollection.Add(new CurrencyDto("RON"));

            // Act
            var actual = await _sut.GetExchangeRates(_currencyDtoCollection);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count(), Is.EqualTo(6));
            Assert.That(!actual.Any(x => x.SourceCurrency.Code == "EUR"), Is.True);
            Assert.That(!actual.Any(x => x.SourceCurrency.Code == "SGD"), Is.True);
            Assert.That(!actual.Any(x => x.SourceCurrency.Code == "RON"), Is.True);
        }

        [Test]
        public async Task GetExchangeRates_Always_IgnoresCaseWhileMatchingCurrenciesFromTheProvider()
        {
            // Arrange
            _currencyDtoCollection.Clear();
            _currencyDtoCollection.Add(new CurrencyDto("GBP"));
            _currencyDtoCollection.Add(new CurrencyDto("USD"));
            _currencyDtoCollection.Add(new CurrencyDto("TRY"));
            _currencyDtoCollection.Add(new CurrencyDto("inr"));
            _currencyDtoCollection.Add(new CurrencyDto("Thb"));
            _currencyDtoCollection.Add(new CurrencyDto("krw"));

            // Act
            var actual = await _sut.GetExchangeRates(_currencyDtoCollection);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count(), Is.EqualTo(6));
            Assert.That(actual.Any(x => x.SourceCurrency.Code == "GBP"), Is.True);
            Assert.That(actual.Any(x => x.SourceCurrency.Code == "USD"), Is.True);
            Assert.That(actual.Any(x => x.SourceCurrency.Code == "TRY"), Is.True);
            Assert.That(actual.Any(x => x.SourceCurrency.Code == "INR"), Is.True);
            Assert.That(actual.Any(x => x.SourceCurrency.Code == "THB"), Is.True);
            Assert.That(actual.Any(x => x.SourceCurrency.Code == "KRW"), Is.True);
        }

        [Test]
        public async Task GetExchangeRates_OnSuccess_ReturnsExchangeRateDtoCollection()
        {
            // Arrange

            // Act
            var actual = await _sut.GetExchangeRates(_currencyDtoCollection);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<IEnumerable<ExchangeRateDto>>());
            Assert.That(actual.Count(), Is.EqualTo(_exchangeRateModelCollection.Count()));
            foreach (var exchangeRateModel in _exchangeRateModelCollection)
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
