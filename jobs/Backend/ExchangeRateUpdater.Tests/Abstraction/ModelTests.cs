using ExchangeRateUpdater.Abstraction.Model;

namespace ExchangeRateUpdater.Tests.Abstraction
{
    [TestFixture]
    public class ModelTests
    {
        [Test]
        public void Currency_Constructor_NormalizesAndToString()
        {
            var c = new Currency(" usd ");
            Assert.That(c.Code, Is.EqualTo("USD"));
            Assert.That(c.ToString(), Is.EqualTo("USD"));
        }

        [Test]
        public void Currency_Constructor_InvalidInputs_Throw()
        {
            Assert.That(() => new Currency(null!), Throws.TypeOf<ArgumentException>());
            Assert.That(() => new Currency(string.Empty), Throws.TypeOf<ArgumentException>());
            Assert.That(() => new Currency("  "), Throws.TypeOf<ArgumentException>());
            Assert.That(() => new Currency("US"), Throws.TypeOf<ArgumentException>());
            Assert.That(() => new Currency("USDE"), Throws.TypeOf<ArgumentException>());
            Assert.That(() => new Currency("U$D"), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Currency_Equals_IsCaseInsensitive()
        {
            var a = new Currency("usd");
            var b = new Currency("USD");

            Assert.That(a, Is.EqualTo(b));
            Assert.That(b, Is.EqualTo(a));
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void ExchangeRate_Constructor_SetsProperties()
        {
            var src = new Currency("USD");
            var tgt = new Currency("CZK");
            var validFor = new DateTime(2026, 02, 15);

            var rate = new ExchangeRate(src, tgt, 25.5m, validFor);

            Assert.That(rate.SourceCurrency, Is.EqualTo(src));
            Assert.That(rate.TargetCurrency, Is.EqualTo(tgt));
            Assert.That(rate.Value, Is.EqualTo(25.5m));
            Assert.That(rate.ValidFor, Is.EqualTo(validFor));
            Assert.That(rate.ToString(), Does.Contain("USD/CZK="));
            Assert.That(rate.ToString(), Does.Contain(validFor.ToString()));
        }

        [Test]
        public void ExchangeRate_InvalidValue_Throws()
        {
            var src = new Currency("USD");
            var tgt = new Currency("CZK");
            var validFor = DateTime.UtcNow;

            Assert.That(() => new ExchangeRate(src, tgt, 0m, validFor), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(() => new ExchangeRate(src, tgt, -1m, validFor), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
