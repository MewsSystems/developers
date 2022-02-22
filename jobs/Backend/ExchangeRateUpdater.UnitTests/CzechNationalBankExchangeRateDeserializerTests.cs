using ExchangeRateUpdater.Deserializers;
using NUnit.Framework;

namespace ExchangeRateUpdater.UnitTests
{
    public class CzechNationalBankExchangeRateDeserializerTests
    {
        private IExchangeRateDeserializer _deserializer;

        [OneTimeSetUp]
        public void Setup()
        {
            _deserializer = new CzechNationalBankExchangeRateDeserializer();
        }

        [Test]
        public void ShouldParseCorrectInput()
        {
            var rate = _deserializer.Deserialize("Maďarsko|forint|100|HUF|6,825");

            Assert.IsNotNull(rate);
            Assert.That(rate.SourceCurrency.Code, Is.EqualTo("HUF"));
            Assert.That(rate.TargetCurrency.Code, Is.EqualTo("CZK"));
            Assert.That(rate.Value, Is.EqualTo(0.06825m));
        }

        [Test]
        public void ShouldHandleWrongSourceAmount()
        {
            var rate = _deserializer.Deserialize("Maďarsko|forint|XXX|HUF|6,825");

            Assert.IsNull(rate);
        }

        [Test]
        public void ShouldHandleWrongTargetAmount()
        {
            var rate = _deserializer.Deserialize("Maďarsko|forint|100|HUF|XXX");

            Assert.IsNull(rate);
        }

        [Test]
        public void ShouldHandleNullInput()
        {
            var rate = _deserializer.Deserialize(null);

            Assert.IsNull(rate);
        }

        [TestCase("Maďarsko|forint|100|HUF")]
        [TestCase("Maďarsko|forint|100|HUF|6,825|XXX")]
        public void ShouldHandleWrongAmountOfParts(string serializedExchangeRate)
        {
            var rate = _deserializer.Deserialize(serializedExchangeRate);

            Assert.IsNull(rate);
        }
    }
}