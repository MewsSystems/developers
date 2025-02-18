using ExchangeRate.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRate.Application.DTOs
{
    public class ExchangeRateProviderDTO
    {
        public ExchangeRateProviderDTO(CurrencyDTO sourceCurrency, CurrencyDTO targetCurrency, decimal value, int amount)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
            Amount = amount;
        }

        public CurrencyDTO SourceCurrency { get; }
        public CurrencyDTO TargetCurrency { get; }
        public int Amount { get; }
        public decimal Value { get; }

         public override string ToString()
        {
            return $"{SourceCurrency?.Code}/{TargetCurrency?.Code}= {Value}";
        }
        public string Display => ToString();

    }

}
