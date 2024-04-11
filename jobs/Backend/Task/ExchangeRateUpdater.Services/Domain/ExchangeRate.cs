using ExchangeRateUpdater.Services.Client.ClientModel;

namespace ExchangeRateUpdater.Services.Domain
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }

        public static implicit operator ExchangeRate(ExchangeRateResponse exchangeRateResponse)
        {
            return new ExchangeRate(
                exchangeRateResponse.CurrencyCode,
                "CZK",
                (decimal)(exchangeRateResponse.Rate / exchangeRateResponse.Amount));
        }
    }
}
