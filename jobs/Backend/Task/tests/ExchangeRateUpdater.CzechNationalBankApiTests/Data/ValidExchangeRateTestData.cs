using ExchangeRateUpdater.Core.Models.CzechNationalBank;
using System.Collections;

namespace ExchangeRateUpdater.CzechNationalBankTests.Data
{
    public class ValidExchangeRateTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ExchangeRateResponse
                {
                    Amount = 1,
                    CurrencyCode = "ABC",
                    Rate = 1.52m
                },
                "ABC/CZK=0.66"
            };
            yield return new object[]
            {
                new ExchangeRateResponse
                {
                    Amount = 100,
                    CurrencyCode = "BCD",
                    Rate = 0.52m
                },
                "BCD/CZK=192.31"
            };
            yield return new object[]
            {
                new ExchangeRateResponse
                {
                    Amount = 1,
                    CurrencyCode = "CDE",
                    Rate = 0.1m
                },
                "CDE/CZK=10"
            };
            yield return new object[]
            {
                new ExchangeRateResponse
                {
                    Amount = 1,
                    CurrencyCode = "DEF",
                    Rate = 1m
                },
                "DEF/CZK=1"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
