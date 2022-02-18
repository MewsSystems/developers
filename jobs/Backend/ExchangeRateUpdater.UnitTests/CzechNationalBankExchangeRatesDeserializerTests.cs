using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace ExchangeRateUpdater.UnitTests
{
    [TestFixture]
    public class CzechNationalBankExchangeRatesDeserializerTests
    {
        private IExchangeRatesDeserializer _deserializer;

        [OneTimeSetUp]
        public void CzechNationalBankExchangeRatesDeserializerTests_OneTimeSetUp()
        {
            var lineDeserializer = Substitute.For<IExchangeRateDeserializer>();
            lineDeserializer.Deserialize(Arg.Any<string>()).Returns(new ExchangeRate(new Currency("CZK"), new Currency("CZK"), 1));
            _deserializer = new CzechNationalBankExchangeRatesDeserializer(lineDeserializer);
        }

        [Test]
        public void ShouldHandleEmptyInput()
        {
            var result = _deserializer.Deserialize(null);

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldSkipFirstTwoLines()
        {
            var input = "18.02.2022 #35\r\nzemě|měna|množství|kód|kurz\r\n";
            var result = _deserializer.Deserialize(input);

            Assert.That(result.Count(), Is.EqualTo(0));
        }


        [Test]
        public void ShouldRemoveEmptyLines()
        {
            var input = "18.02.2022 #35\r\nzemě|měna|množství|kód|kurz\r\nAustrálie|dolar|1|AUD|15,453\r\nBrazílie|real|1|BRL|4,166\r\n\r\n\r\n";
            var result = _deserializer.Deserialize(input);

            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
