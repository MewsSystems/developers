using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Dto;

namespace ExchangeRateUpdater.Infrastructure.Extensions
{
    internal static class ExchangeRateExtensions
    {
        public static ExchangeRateDto ToDto(this ExchangeRate exchangeRate)
        {
            return new ExchangeRateDto(exchangeRate.SourceCurrency.Code, exchangeRate.TargetCurrency.Code, exchangeRate.Value);
        }
    }
}
