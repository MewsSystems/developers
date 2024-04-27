using ExchangeRateFinder.Domain.Entities;

namespace ExchangeRateFinder.Domain.Services
{
    public interface IExchangeRateCalculator
    {
        CalculatedExchangeRate Calculate(int amount, decimal rate, string sourceCurrency, string targetCurrency);
    }

    public class ExchangeRateCalculator : IExchangeRateCalculator
    {
        public CalculatedExchangeRate Calculate(int amount, decimal rate, string sourceCurrency, string targetCurrency)
        {
            return new CalculatedExchangeRate
            {
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency,
                Value = CalculateRate(amount, rate)
            };
        }

        private decimal CalculateRate(int amount, decimal rate)
        {
            return (amount > 1) ?  amount / rate : rate;
        }
    }
}
