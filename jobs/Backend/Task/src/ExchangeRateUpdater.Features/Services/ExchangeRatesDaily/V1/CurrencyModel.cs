using ExchangeRateUpdater.Models.Domain;

namespace ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1
{
    public class CurrencyModel
    {
        public CurrencyModel() { }
        public CurrencyModel(string code)
        {
            Code = code;
        }
        public string Code { get; set; }

        public override string ToString()
        {
            return Code;
        }
    }

    public class ExchangeRateModel
    {
        public CurrencyModel SourceCurrency { get; set; }

        public CurrencyModel TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
