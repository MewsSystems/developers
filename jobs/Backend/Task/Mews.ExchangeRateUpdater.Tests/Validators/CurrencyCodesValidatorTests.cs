using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services.Validators;

namespace Mews.ExchangeRateUpdater.Tests.Validators
{
    [TestFixture]
    public class CurrencyCodesValidatorTests
    {
        private CurrencyCodesValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CurrencyCodesValidator();
        }

        [Test]
        public void Validate_IfProvidedCurrencyCodeCollectionIsNull()
        {
            // Arrange
            List<CurrencyDto> currencies = null;
            var validationMessages = new List<string>();

            // Act
            var actual = _sut.Validate(ref currencies, ref validationMessages);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo("Currency codes collection cannot be null or empty"));
        }

        [Test]
        public void Validate_IfProvidedCurrencyCodeCollectionIsEmpty()
        {
            // Arrange
            List<CurrencyDto> currencies = new List<CurrencyDto>();
            var validationMessages = new List<string>();

            // Act
            var actual = _sut.Validate(ref currencies, ref validationMessages);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo("Currency codes collection cannot be null or empty"));
        }

        [Test]
        public void Validate_IgnoresTheCaseOfAValidCurrencyCode()
        {
            // Arrange
            List<CurrencyDto> currencies = new List<CurrencyDto>
            {
                new CurrencyDto("GbP"),
                new CurrencyDto("inr")
            };
            var validationMessages = new List<string>();

            // Act
            var actual = _sut.Validate(ref currencies, ref validationMessages);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(0));
            Assert.That(currencies.Count(), Is.EqualTo(2));
            Assert.That(currencies.ToList()[0].Code, Is.EqualTo("GbP"));
            Assert.That(currencies.ToList()[1].Code, Is.EqualTo("inr"));
        }

        [Test]
        public void Validate_RemovesTheCurrencyCodeIfItIsInvalid()
        {
            // Arrange
            List<CurrencyDto> currencies = new List<CurrencyDto> 
            { 
                new CurrencyDto("GBP"),
                new CurrencyDto("INR"),
                new CurrencyDto("123"),
                new CurrencyDto("INRX"),
                new CurrencyDto("XYZ")
            };
            var validationMessages = new List<string>();

            // Act
            var actual = _sut.Validate(ref currencies, ref validationMessages);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(currencies.Count(), Is.EqualTo(2));
            Assert.That(currencies.ToList()[0].Code, Is.EqualTo("GBP"));
            Assert.That(currencies.ToList()[1].Code, Is.EqualTo("INR"));
            Assert.That(actual[0], Is.EqualTo("There are some invalid currency codes present in the input, removing those codes before fetching the exchange rates : [123, INRX, XYZ]"));
        }

        [Test]
        public void Validate_RemovesTheDuplicateCurrencyCodesBeforeValidating()
        {
            // Arrange
            List<CurrencyDto> currencies = new List<CurrencyDto>
            {
                new CurrencyDto("GBP"),
                new CurrencyDto("GBP"),
                new CurrencyDto("INR"),
                new CurrencyDto("INR"),
                new CurrencyDto("gbp")
            };
            var validationMessages = new List<string>();

            // Act
            var actual = _sut.Validate(ref currencies, ref validationMessages);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(0));
            Assert.That(currencies.Count(), Is.EqualTo(2));
            Assert.That(currencies.ToList()[0].Code, Is.EqualTo("GBP"));
            Assert.That(currencies.ToList()[1].Code, Is.EqualTo("INR"));
        }
    }
}
