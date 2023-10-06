using System;
using Mews.ExchangeRate.Domain.Models;
using Mews.ExchangeRate.Http.Abstractions.Dtos;

namespace Mews.ExchangeRate.Updater.Services.Extensions
{
    public static class ModelExtensions
    {
        public static Domain.Models.ExchangeRate ToDomainModel(this ExchangeRateDto exchangeRate)
        {
            if(exchangeRate is null)
            {
                return null;
            }

            var value = exchangeRate.Rate/exchangeRate.Amount;
            var sourceCurrency = new Currency(exchangeRate.CurrencyCode);
            var targetCurrency = Currency.Default;

            return new Domain.Models.ExchangeRate(sourceCurrency, targetCurrency, value);
        }
    }
}
