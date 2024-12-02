using ExchangeRates.Core.Models;
using System.Collections.Generic;

namespace ExchangeRates.ConsoleTestProgram
{
    internal static class TestData
    {
        internal static readonly IEnumerable<Currency> Currencies =
        [
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        ];
    }
}