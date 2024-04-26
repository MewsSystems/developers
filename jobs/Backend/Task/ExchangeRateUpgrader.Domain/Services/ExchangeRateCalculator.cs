using ExchangeRateFinder.Domain.Entities;

namespace ExchangeRateFinder.Domain.Services
{
    public interface IExchangeRateCalculator
    {
        ExchangeRate CalculateExchangeRate(int amount, decimal rate, string sourceCurrency, string targetCurrency);
    }

    public class ExchangeRateCalculator : IExchangeRateCalculator
    {
        public ExchangeRate CalculateExchangeRate(int amount, decimal rate, string sourceCurrency, string targetCurrency)
        {
            return new ExchangeRate
            {
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency,
                Value = CalculateRate(amount, rate)
            };
        }

        private decimal CalculateRate(int amount, decimal rate)
        {
            return (amount > 1) ? rate / amount : rate;
        }
    }
}
