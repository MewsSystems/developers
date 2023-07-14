using ExchangeRateUpdater.Domain.Model;
using ExchangeRateUpdater.Interface.Configuration;
using ExchangeRateUpdater.Interface.DTOs;
using RestSharp;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Test.Helpers
{
    internal class TestObjects
    {
        internal static CnbSettings CnbSettings = new()
        {
            BaseUrl = "https://goog",//"https://api.cnb.cz/cnbapi",
            GetExchangeRatesEndpoint = "/exrates/daily?lang=EN"
        };

        internal static IEnumerable<CurrencyDto> Currencies = new[]
        {
            new CurrencyDto { Code = "USD" },
            new CurrencyDto { Code = "EUR" },
            new CurrencyDto { Code = "CZK" },
            new CurrencyDto { Code = "JPY" },
            new CurrencyDto { Code = "KES" },
            new CurrencyDto { Code = "RUB" },
            new CurrencyDto { Code = "THB" },
            new CurrencyDto { Code = "TRY" },
            new CurrencyDto { Code = "XYZ" }
        };

        internal static IEnumerable<ExchangeRateEntity> ExchangeRateEntityList = new[]
        {
            new ExchangeRateEntity
            {
                CurrencyCode = "EUR",
                Rate = 23.75m,
                Amount = 1,
                ValidFor = new System.DateOnly(2023, 07, 13)
            },
            new ExchangeRateEntity
            {
                CurrencyCode = "USD",
                Rate = 21.243m,
                Amount = 1,
                ValidFor = new System.DateOnly(2023, 07, 13)
            },           
            new ExchangeRateEntity
            {
                CurrencyCode = "JPY",
                Rate = 15.335m,
                Amount = 100,
                ValidFor = new System.DateOnly(2023, 07, 13)
            },
            new ExchangeRateEntity
            {
                CurrencyCode = "THB",
                Rate = 61.416m,
                Amount = 100,
                ValidFor = new System.DateOnly(2023, 07, 13)
            },
            new ExchangeRateEntity
            {
                CurrencyCode = "TRY",
                Rate = 0.812m,
                Amount = 1,
                ValidFor = new System.DateOnly(2023, 07, 13)
            },
        };
    }
}
