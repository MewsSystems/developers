using ExchangeRateFinder.Domain.Entities;

namespace ExchangeRateFinder.Domain.Services
{
    public interface IExchangeRateCalculator
    {
        CalculatedExchangeRate Calculate(int amount, decimal value, string SourceCurrencyCode, string TargetCurrencyCode);
    }

    public class ExchangeRateCalculator : IExchangeRateCalculator
    {
        public CalculatedExchangeRate Calculate(int amount, decimal value, string SourceCurrencyCode, string TargetCurrencyCode)
        {
            return new CalculatedExchangeRate
            {
                SourceCurrencyCode = SourceCurrencyCode,
                TargetCurrencyCode = TargetCurrencyCode,
                Rate = CalculateRate(amount, value)
            };
        }

        private decimal CalculateRate(int amount, decimal value)
        {
            return amount / value;
        }
    }
}
