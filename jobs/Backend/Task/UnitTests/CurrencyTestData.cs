using System.Collections;
using System.Collections.Generic;
using ExchangeRateUpdater;

namespace UnitTests;

public class CurrencyTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new List<Currency>() {  }, 0 };
        yield return new object[] { new List<Currency>() { new Currency("USD") }, 1 };
        yield return new object[] { new List<Currency>() { new Currency("ZZZ") }, 0 };
        yield return new object[] { new List<Currency>() { new Currency("USD"), new Currency("EUR") }, 2 };
        yield return new object[] { new List<Currency>() { new Currency("USD"), new Currency("EUR") , new Currency("ZZZ") }, 2 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}