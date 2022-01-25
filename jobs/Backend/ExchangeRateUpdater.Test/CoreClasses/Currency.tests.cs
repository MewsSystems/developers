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
        Currency c;
        Currency cSame;
        Currency cOther;

        [SetUp]
        public void Init()
        {
            c = new Currency("RUB");
            cSame = new Currency("RUB");
            cOther = new Currency("CZK");
        }

        [Test]
        public void Constructor_Exceptions_tests()
        {
            Assert.Throws<ArgumentNullException>(() => new Currency(null));
            Assert.Throws<ArgumentException>(() => new Currency(" "));
        }

        [Test]
        public void GetHashCode_tests()
        {
            Assert.AreEqual(c.GetHashCode(), cSame.GetHashCode());
            Assert.AreNotEqual(c.GetHashCode(), cOther.GetHashCode());
        }

        [Test]
        public void Equality_tests()
        {
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
