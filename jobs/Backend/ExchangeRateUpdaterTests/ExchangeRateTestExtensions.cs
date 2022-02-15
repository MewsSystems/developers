using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Fluent;
using ExchangeRateUpdater.Structures;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests
{
    public static class ExchangeRateTestExtensions
    {
        public static List<ExchangeRate> CallExchangeRates(this Table table, params string[] currencyCodes)
        {
            return Task<Fluent<Table>>.FromResult(Fluent<Table>.Create(table))
                .ComputeExchangeRates(currencyCodes.Select(code =>  new Currency(code)))
                .ToList()
                .Result;
        }
        
        public static void ShouldBe(this IEnumerable<ExchangeRate> exchangeRates, 
            string sourceCode, string targetCode, decimal value)
        {
            exchangeRates.Should().BeEquivalentTo(new List<ExchangeRate>
            {
                new (new Currency(sourceCode), new Currency(targetCode), value)
            });
        }
        
        public static void ShouldBe(this IEnumerable<ExchangeRate> exchangeRates, 
            string sourceCode1, string targetCode1, decimal value1,
            string sourceCode2, string targetCode2, decimal value2)
        {
            exchangeRates.Should().BeEquivalentTo(new List<ExchangeRate>
            {
                new (new Currency(sourceCode1), new Currency(targetCode1), value1),
                new (new Currency(sourceCode2), new Currency(targetCode2), value2),
            });
        }
        
        public static void ShouldBe(this IEnumerable<ExchangeRate> exchangeRates, 
            (string sourceCode, string targetCode, decimal value)[] expectedExchangeRates)
        {
            exchangeRates.Should().BeEquivalentTo(expectedExchangeRates
                .Select(rate =>
                {
                    var (sourceCode, targetCode, value) = rate;
                    return new ExchangeRate(new Currency(sourceCode), new Currency(targetCode), value);
                }));
        }
        
        public static void ShouldBeEmpty(this IEnumerable<ExchangeRate> exchangeRates)
        {
            exchangeRates.Should().BeEmpty();
        }
    }
}