using ExchangeRateUpdater.CoreClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.CoreClasses
{
    [TestFixture]
    public class Currency_Tests
    {
        [Test]
        public void Constructor_Exceptions_tests()
        {
            Assert.Throws<ArgumentNullException>(() => new Currency(null));
            Assert.Throws<ArgumentException>(() => new Currency(" "));
        }

        [Test]
        public void GetHashCode_tests()
        {
            Currency c = new Currency("RUB");
            Currency cSame = new Currency("RUB");
            Currency cOther = new Currency("CZK");

            Assert.AreEqual(c.GetHashCode(), cSame.GetHashCode());
            Assert.AreNotEqual(c.GetHashCode(), cOther.GetHashCode());
        }

        [Test]
        public void Equality_tests()
        {
            Currency c = new Currency("RUB");
            Currency cSame = new Currency("RUB");
            Currency cOther = new Currency("CZK");

            Assert.AreEqual(c, cSame);
            Assert.AreNotEqual(c, cOther);
        }

        [Test]
        public void ToString_tests()
        {
            string code = "RUB";
            Currency c = new Currency(code);

            Assert.AreEqual(c.ToString(), code);
        }


    }
}
