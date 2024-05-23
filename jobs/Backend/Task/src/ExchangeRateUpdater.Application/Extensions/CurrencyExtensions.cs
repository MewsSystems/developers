using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Application.Extensions
{
    public static class CurrencyExtensions
    {
        public static Currency ToDomain(this CurrencyDto currency) => Currency.Create(currency.Code);

        public static CurrencyDto ToDto(this Currency currency) => new CurrencyDto(currency.Code);

    }
}
