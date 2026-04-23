using ExchangeRateUpdater.CbnApiClient.Dto;
using ExchangeRateUpdater.CbnApiClient.Mapper;

namespace ExchangeRateUpdater.Tests.CbnApiClient
{
    [TestFixture]
    public class CnbExchangeRateMapperExtensionsTests
    {
        [Test]
        public void ToDomain_SingleEntry_MapsCorrectly()
        {
            var dto = new ExchangeRateEntry
            {
                Amount = 100,
                Rate = 2500m,
                CurrencyCode = "USD",
                ValidFor = new DateTime(2026, 2, 15)
            };

            var domain = dto.ToDomain();

            Assert.That(domain.SourceCurrency.Code, Is.EqualTo("USD"));
            Assert.That(domain.TargetCurrency.Code, Is.EqualTo("CZK"));
            Assert.That(domain.Value, Is.EqualTo(25.0m));
            Assert.That(domain.ValidFor, Is.EqualTo(new DateTime(2026, 2, 15)));
        }

        [Test]
        public void ToDomain_SingleEntry_Null_Throws()
        {
            ExchangeRateEntry? dto = null;
            Assert.That(() => CnbExchangeRateMapperExtensions.ToDomain(dto!), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ToDomain_SingleEntry_AmountZero_Throws()
        {
            var dto = new ExchangeRateEntry
            {
                Amount = 0,
                Rate = 100m,
                CurrencyCode = "EUR",
                ValidFor = DateTime.UtcNow
            };

            Assert.That(() => dto.ToDomain(), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ToDomain_Enumerable_MapsAndIgnoresNulls()
        {
            var list = new List<ExchangeRateEntry?>
            {
                new ExchangeRateEntry { Amount = 1, Rate = 30m, CurrencyCode = "ABC", ValidFor = new DateTime(2026,1,1) },
                null,
                new ExchangeRateEntry { Amount = 2, Rate = 100m, CurrencyCode = "DEF", ValidFor = new DateTime(2026,1,2) }
            };

            var mapped = CnbExchangeRateMapperExtensions.ToDomain(list.Where(e => e is not null).Select(e => e!));

            var arr = mapped.ToArray();
            Assert.That(arr.Length, Is.EqualTo(2));
            Assert.That(arr[0].SourceCurrency.Code, Is.EqualTo("ABC"));
            Assert.That(arr[1].SourceCurrency.Code, Is.EqualTo("DEF"));
        }

        [Test]
        public void ToDomain_Enumerable_Null_Throws()
        {
            IEnumerable<ExchangeRateEntry>? entries = null;
            Assert.That(() => CnbExchangeRateMapperExtensions.ToDomain(entries!), Throws.TypeOf<ArgumentNullException>());
        }
    }
}
