using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Application.Extensions
{
    public static class ExchangeRateExtensions
    {
        public static ExchangeRate ToDomain(this ExchangeRateDto exchangeRate)
        {
            return ExchangeRate.Create(exchangeRate.SourceCurrency.ToDomain(), exchangeRate.TargetCurrency.ToDomain(), exchangeRate.Value);
        }

        public static ExchangeRateDto ToDto(this ExchangeRate exchangeRate)
        {
            return new ExchangeRateDto(exchangeRate.SourceCurrency.ToDto(), exchangeRate.TargetCurrency.ToDto(), exchangeRate.Value);
        }
    }
}
