using ExchangeRateUpdater.Providers;
using FluentAssertions;
using System.Reflection;

namespace ExchangeRateUpdater.Tests.CnbExchangeRateProviderTests
{
    public class MapToExchangeRatesTests
    {
        private readonly object _provider;
        private readonly MethodInfo _parseMethod;
        private readonly MethodInfo _mapMethod;

        public MapToExchangeRatesTests()
        {
            var type = typeof(CnbExchangeRateProvider);
            _provider = Activator.CreateInstance(type)!;

            _parseMethod = type.GetMethod("ParseDailyRates", BindingFlags.NonPublic | BindingFlags.Instance)!;
            _mapMethod = type.GetMethod("MapToExchangeRates", BindingFlags.NonPublic | BindingFlags.Instance)!;
        }

        private object InvokeParse(string raw)
        {
            try
            {
                return _parseMethod.Invoke(_provider, new object[] { raw })!;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        private object InvokeMap(object rows, params Currency[] currencies)
        {
            try
            {
                return _mapMethod.Invoke(_provider, new object[] { rows, currencies })!;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        [Fact]
        public void Map_ValidRow_ReturnsNormalizedRate()
        {
            var txt = @"03 Jan 2025 #1
                        C|C|C|C|C
                        Japan|yen|100|JPY|15.0";

            var rows = InvokeParse(txt);

            var result = (System.Collections.Generic.IEnumerable<ExchangeRate>)InvokeMap(rows,
                new Currency("JPY"));

            var rate = result.Single();

            rate.Value.Should().Be(0.15m);
        }

        [Fact]
        public void Map_RequestMissingCurrency_ReturnsEmpty()
        {
            var txt = @"03 Jan 2025 #1
                        C|C|C|C|C
                        USA|dollar|1|USD|20.0";

            var rows = InvokeParse(txt);

            var result = (IEnumerable<ExchangeRate>)InvokeMap(rows,
                new Currency("EUR"));

            result.Should().BeEmpty();
        }

        [Fact]
        public void Map_MultipleRequestedSameCode_NoDuplicateRatesReturned()
        {
            var txt = @"03 Jan 2025 #1
                        C|C|C|C|C
                        USA|dollar|1|USD|20.0";

            var rows = InvokeParse(txt);

            var result = (IEnumerable<ExchangeRate>)InvokeMap(rows,
                new Currency("USD"), new Currency("USD"));

            result.Should().HaveCount(1);
        }

        [Fact]
        public void Map_RequestedCurrenciesIsEmpty_ReturnsEmpty()
        {
            var txt = @"03 Jan 2025 #1
                        C|C|C|C|C
                        USA|dollar|1|USD|20.0";

            var rows = InvokeParse(txt);

            var result = (IEnumerable<ExchangeRate>)InvokeMap(rows);

            result.Should().BeEmpty();
        }

        [Fact]
        public void Map_DuplicateCurrencyCodesInRows_Throws()
        {
            var txt = @"03 Jan 2025 #1
                        C|C|C|C|C
                        USA|dollar|1|USD|20.0
                        United States|dollar|1|USD|21.0";

            var rows = InvokeParse(txt);

            Action act = () => InvokeMap(rows, new Currency("USD"));

            act.Should().Throw<FormatException>()
                .WithMessage("*duplicate*");
        }
    }
}
