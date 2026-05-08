using ExchangeRateUpdater.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests.UnitTests.Data
{
    public static class ExchangeRateProviderTestData
    {
        public static IEnumerable<ExchangeRateDTO> ValidRates => new[]
        {
            new ExchangeRateDTO
            {
                CurrencyCode = "USD",
                Rate = 22,
                Amount = 1
            },
            new ExchangeRateDTO
            {
                CurrencyCode = "EUR",
                Rate = 25,
                Amount = 1
            }
        };

        public static IEnumerable<ExchangeRateDTO> ValidRatesGreater => new[]
{
            new ExchangeRateDTO
            {
                CurrencyCode = "USD",
                Rate = 30,
                Amount = 1
            },
            new ExchangeRateDTO
            {
                CurrencyCode = "EUR",
                Rate = 32,
                Amount = 1
            }
        };

        public static IEnumerable<ExchangeRateDTO> WithInvalidRate => new[]
        {
            new ExchangeRateDTO
            {
                CurrencyCode = "USD",
                Rate = -1,
                Amount = 1
            },
            new ExchangeRateDTO
            {
                CurrencyCode = "EUR",
                Rate = 25,
                Amount = 1
            }
        };

        public static IEnumerable<ExchangeRateDTO> WithAmountGreaterThanOne => new[]
        {
            new ExchangeRateDTO
            {
                CurrencyCode = "USD",
                Rate = 220,
                Amount = 10
            }
        };

        public static IEnumerable<ExchangeRateDTO> WithNullAndInvalidEntries => new ExchangeRateDTO[]
        {
            null,
            new ExchangeRateDTO
            {
                CurrencyCode = "",
                Rate = 10,
                Amount = 1
            },
            new ExchangeRateDTO
            {
                CurrencyCode = "USD",
                Rate = 22,
                Amount = 1
            }
        };

        public static IEnumerable<ExchangeRateDTO> WithBaseCurrency => new ExchangeRateDTO[]
        {
            new ExchangeRateDTO
            {
                CurrencyCode = "CZK",
                Rate = 10,
                Amount = 1
            },
            new ExchangeRateDTO
            {
                CurrencyCode = "USD",
                Rate = 22,
                Amount = 1
            }
        };
    }
}
