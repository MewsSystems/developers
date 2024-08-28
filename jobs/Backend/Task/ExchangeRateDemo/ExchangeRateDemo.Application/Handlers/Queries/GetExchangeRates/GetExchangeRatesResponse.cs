using ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates.Models;

namespace ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates
{
    public class GetExchangeRatesResponse(ExchangeRate rate)
    {
        public Currency? SourceCurrency { get; } = new();

        public Currency? TargetCurrency { get; } = new(rate.CurrencyCode);

        public decimal Value { get; } = rate.Rate;

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
