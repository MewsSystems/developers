using NSubstitute;

using NUnit.Framework;

namespace ExchangeRateUpdater.Tests
{
    /// <summary>
    /// Tests for see <see cref="ExchangeRateCalculator"/> class.
    /// </summary>
    [TestFixture]
    public class ExchangeRateCalculatorTests
    {
        private const string CURRENCY_CODE = "CZK";

        private IExchangeRateDataSource dataSource = null!;
        private ExchangeRateCalculator calculator = null!;

        [SetUp]
        public void SetUp()
        {
            dataSource = Substitute.For<IExchangeRateDataSource>();

            calculator = new ExchangeRateCalculator(dataSource);
        }

        [Test]
        public void TryGetRetrievesDataFromDataSource()
        {
            calculator.TryGet(CURRENCY_CODE, out _);

            dataSource.Received(1).TryGet(CURRENCY_CODE, out Arg.Any<int>(), out Arg.Any<decimal>());
        }

        [Test]
        public void RateNotAvailableFalseIsReturned()
        {
            var result = calculator.TryGet(CURRENCY_CODE, out _);

            Assert.IsFalse(result);
        }

        [Test]
        public void RateNotCachedDataSourceIsUsed()
        {
            dataSource.TryGet(CURRENCY_CODE, out Arg.Any<int>(), out Arg.Any<decimal>()).Returns(x =>
            {
                x[1] = 100;
                x[2] = 20M;
                return true;
            });

            var result = calculator.TryGet(CURRENCY_CODE, out var rate);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result);
                Assert.AreEqual(100/20M, rate);
            });
        }
    }
}